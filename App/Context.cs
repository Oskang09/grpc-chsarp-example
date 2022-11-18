using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KubeMQ.SDK.csharp.CommandQuery;
using Newtonsoft.Json.Linq;
using OpenTracing;
using OpenTracing.Propagation;
using OpenTracing.Util;

namespace amantiq
{
    public class Context : JObject, ITextMap
    {
        public int status;
        private RequestReceive query;
        private Dictionary<string, string> tracer = new Dictionary<string, string>();

        public Context(JObject payload, RequestReceive query) : base(payload)
        {
            this.query = query;
            this.status = 200;
        }

        public void Trace(string operation)
        {
            // ISpanContext parent = GlobalTracer.Instance.Extract(BuiltinFormats.TextMap, this);
            // ISpan span = GlobalTracer.Instance.BuildSpan(operation).AsChildOf(parent).Start();
        }

        public IDictionary<string, string> getJaegerTracer()
        {
            return tracer;
        }

        public void Set(string key, string value)
        {
            tracer.Add(key, value);
        }

        IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
        {
            return tracer.GetEnumerator();
        }

        public void Background(string operationName, Action<Context> operation)
        {
            ITracer tracer = GlobalTracer.Instance;
            ISpanContext parentSpan = tracer.Extract(BuiltinFormats.TextMap, this);
            ISpanBuilder spanBuilder = tracer.BuildSpan(operationName).AsChildOf(parentSpan);
            Context ctx = new Context(new JObject(), null);
            ISpan span = spanBuilder.Start();
            tracer.Inject(span.Context, BuiltinFormats.TextMap, ctx);
            Task.Run(() =>
            {
                operation(ctx);
                span.Finish();
            });
        }
    }
}