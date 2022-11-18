using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using KubeMQ.SDK.csharp.CommandQuery;
using KubeMQ.SDK.csharp.Subscription;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using BeetleX.Redis;
using System.Threading.Tasks;
using Jaeger.Reporters;
using Jaeger;
using Microsoft.Extensions.Logging;
using Jaeger.Samplers;
using OpenTracing;
using Jaeger.Senders.Thrift;
using OpenTracing.Util;
using OpenTracing.Propagation;
using amantiq.constant;

namespace amantiq
{

    class Server
    {
        public static void Start()
        {
            new Server();
        }

        private Dictionary<String, Service> routes = new Dictionary<string, Service>();
        public static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),

            DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ",
            Converters = new List<JsonConverter>()
            {
                new Newtonsoft.Json.Converters.StringEnumConverter(),
            }
        };


        private Server()
        {
            amantiq.constant.Environment.Initialize();

            var controllers = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsSubclassOf(typeof(Controller)));
            foreach (Type type in controllers)
            {
                object instance = Activator.CreateInstance(type);
                var methods = type.GetMethods().Where(x => x.GetCustomAttributes().OfType<Service>().Any());
                foreach (MethodInfo method in methods)
                {
                    Service service = method.GetCustomAttribute<Service>();
                    service.instance = instance;
                    service.method = method;
                    routes.Add(service.path, service);
                }
            }

            initOpenTracing();
            initRedis();

            Responder server = new Responder(amantiq.constant.Environment.KUBEMQ_ADDR);
            KubeMQ.SDK.csharp.Subscription.SubscribeRequest setting = new KubeMQ.SDK.csharp.Subscription.SubscribeRequest()
            {
                Channel = "company-service",
                ClientID = "company-service",
                Group = "company-service",
                SubscribeType = SubscribeType.Queries,
            };
            server.SubscribeToRequests(
                setting,
                (query) =>
                {
                    Task<Response> response = OnQuery(query);
                    response.Wait();
                    return response.Result;
                },
                onError
            );
        }

        async Task<Response> OnQuery(RequestReceive query)
        {
            String inJson = Encoding.UTF8.GetString(query.Body);
            JObject inMap = JObject.Parse(inJson);
            object outJson;
            Service route;
            string endpoint = inMap.Value<string>("endpoint");
            if (routes.TryGetValue(endpoint, out route))
            {
                ITracer tracer = GlobalTracer.Instance;
                ISpanBuilder spanBuilder = tracer.BuildSpan(endpoint);
                ISpanContext parentSpan = tracer.Extract(BuiltinFormats.TextMap, new TextMapExtractAdapter(query.Tags));
                if (parentSpan != null)
                {
                    spanBuilder = spanBuilder.AsChildOf(parentSpan);
                }
                ISpan span = spanBuilder.Start();
                Dictionary<string, object> traces = new Dictionary<string, object>();
                JObject jObject = inMap.Value<JObject>("payload");
                Context ctx = new Context(jObject, query);
                try
                {
                    span.SetTag("request_id", query.RequestID);
                    tracer.Inject(span.Context, BuiltinFormats.TextMap, ctx);
                    traces.Add("request", jObject.ToString());

                    object result = route.method.Invoke(route.instance, new[] { ctx });
                    if (result != null && result is Task)
                    {
                        Type resultType = result.GetType();
                        if (resultType.IsGenericType)
                        {
                            result = await (Task<object>)result;
                        }
                        else
                        {
                            await (Task)result;
                            result = null;
                        }
                    }
                    outJson = new { result = result, status = ctx.status };
                }
                catch (Exception error)
                {
                    Console.WriteLine(error);
                    traces.Add("error", error.StackTrace);
                    Exception ex = error;
                    if (error.InnerException != null)
                    {
                        traces.Add("error.inner", error.InnerException.StackTrace);
                        ex = error.InnerException;
                    }

                    string message = "";
                    if (Error.Mapper.TryGetValue(ex, out message))
                    {
                        outJson = new { error = message, status = ctx.status };
                    }
                    else
                    {
                        if (ex is ValidationException)
                        {
                            outJson = new { error = ex.Message, status = 422 };
                        }
                        else
                        {
                            outJson = new { error = ex.Message, status = 500 };
                        }
                    }
                }
                traces.Add("response", JsonConvert.SerializeObject(outJson, jsonSerializerSettings));
                span.Log(traces);
                span.Finish();
            }
            else
            {
                outJson = new { error = "No service endpoint found", status = 404 };
            }

            return new Response(query)
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(outJson, jsonSerializerSettings)),
                ClientID = "company-service",
                Error = "",
                Executed = true,
                Timestamp = DateTime.UtcNow,
            };
        }

        void onError(Exception exception)
        {
            Console.WriteLine(exception);
        }

        void initOpenTracing()
        {
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var sampler = new ConstSampler(sample: true);
            var reporter = new RemoteReporter.Builder()
             .WithLoggerFactory(loggerFactory)
             .WithSender(new UdpSender(amantiq.constant.Environment.JAEGER_HOST, amantiq.constant.Environment.JAEGER_PORT, 0))
             .Build();

            Tracer tracer = new Tracer.Builder("company-service")
                .WithLoggerFactory(loggerFactory)
                .WithSampler(sampler)
                .WithReporter(reporter)
                .Build();

            GlobalTracer.RegisterIfAbsent(tracer);
        }

        void initRedis()
        {
            Redis.Default.DataFormater = new JsonFormater();
            Redis.Default.Host.AddWriteHost("127.0.0.1", 6379);
        }
    }
}