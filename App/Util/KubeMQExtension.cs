using System;
using System.Collections.Generic;
using System.Text;
using amantiq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace amamtiq.util
{
    public static class KubeMQExtension
    {
        public static T InternalService<T>(this Context ctx, string service, string endpoint, object payload)
        {
            return ctx.InternalService<T>(service, endpoint, payload, null);
        }

        public static JObject InternalService(this Context ctx, string service, string endpoint, object payload)
        {
            return ctx.InternalService(service, endpoint, payload, null);
        }

        public static JObject InternalService(this Context ctx, string service, string endpoint, object payload, Dictionary<string, string> incomingTags)
        {
            Dictionary<string, string> tags = new Dictionary<string, string>();
            if (incomingTags != null) tags.Merge(incomingTags);
            tags.Merge(ctx.getJaegerTracer());
            return InternalService(service, endpoint, payload, tags);
        }

        public static T InternalService<T>(this Context ctx, string service, string endpoint, object payload, Dictionary<string, string> incomingTags)
        {
            Dictionary<string, string> tags = new Dictionary<string, string>();
            if (incomingTags != null) tags.Merge(incomingTags);
            tags.Merge(ctx.getJaegerTracer());
            return InternalService<T>(service, endpoint, payload, tags);
        }

        public static JObject InternalService(string service, string endpoint, object payload, Dictionary<string, string> incomingTags)
        {
            Dictionary<string, string> tags = new Dictionary<string, string>();
            if (incomingTags != null) tags.Merge(incomingTags);

            var channelParams = new KubeMQ.SDK.csharp.CommandQuery.ChannelParameters
            {
                RequestsType = KubeMQ.SDK.csharp.CommandQuery.RequestType.Query,
                Timeout = 1000,
                ChannelName = service,
                ClientID = "company-service-sender",
                KubeMQAddress = amantiq.constant.Environment.KUBEMQ_ADDR
            };
            var requestPayload = new
            {
                endpoint = endpoint,
                payload = payload,
            };
            var request = new KubeMQ.SDK.csharp.CommandQuery.Request
            {
                Body = KubeMQ.SDK.csharp.Tools.Converter.ToUTF8(JsonConvert.SerializeObject(requestPayload, Server.jsonSerializerSettings)),
                Tags = tags,
            };
            var channel = new KubeMQ.SDK.csharp.CommandQuery.Channel(channelParams);
            var result = channel.SendRequest(request);
            if (!result.Executed)
            {
                throw new Exception($"Service execution failed: {result.Error}");
            }

            String inJson = Encoding.UTF8.GetString(result.Body);
            return JObject.Parse(inJson);
        }

        public static T InternalService<T>(string service, string endpoint, object payload, Dictionary<string, string> incomingTags)
        {
            Dictionary<string, string> tags = new Dictionary<string, string>();
            if (incomingTags != null) tags.Merge(incomingTags);

            var channelParams = new KubeMQ.SDK.csharp.CommandQuery.ChannelParameters
            {
                RequestsType = KubeMQ.SDK.csharp.CommandQuery.RequestType.Query,
                Timeout = 30000,
                ChannelName = service,
                ClientID = "company-service-sender",
                KubeMQAddress = amantiq.constant.Environment.KUBEMQ_ADDR
            };
            var requestPayload = new
            {
                endpoint = endpoint,
                payload = payload,
            };
            var request = new KubeMQ.SDK.csharp.CommandQuery.Request
            {
                Body = KubeMQ.SDK.csharp.Tools.Converter.ToUTF8(JsonConvert.SerializeObject(requestPayload, Server.jsonSerializerSettings)),
                Tags = tags,
            };
            var channel = new KubeMQ.SDK.csharp.CommandQuery.Channel(channelParams);
            var result = channel.SendRequest(request);
            if (!result.Executed)
            {
                throw new Exception($"Service execution failed: {result.Error}");
            }

            String inJson = Encoding.UTF8.GetString(result.Body);
            Dictionary<string, dynamic> response = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(inJson, Server.jsonSerializerSettings);
            if (response.ContainsKey("error"))
            {
                throw new Exception(response.GetValueOrDefault("error", ""));
            }

            JObject jObject = response.GetValueOrDefault("result", "");
            return jObject.ToObject<T>();
        }
    }
}