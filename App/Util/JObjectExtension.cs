using System;
using Newtonsoft.Json.Linq;

namespace amantiq.util
{
    public static class JObjectExtension
    {

        public static bool IsNullOrEmpty(this JObject json, string key)
        {
            var token = json.SelectToken(key);
            return (token == null) ||
                   (token.Type == JTokenType.Array && !token.HasValues) ||
                   (token.Type == JTokenType.Object && !token.HasValues) ||
                   (token.Type == JTokenType.String && token.ToString() == String.Empty) ||
                   (token.Type == JTokenType.Null);
        }

        public static T ValueWithFallback<T>(this JObject json, string key, T fallback)
        {
            if (!json.ContainsKey(key) || IsNullOrEmpty(json, key))
            {
                return fallback;
            }

            T value = json.ObjectValue<T>(key);
            if (value == null)
            {
                return fallback;
            }
            return value;
        }

        public static T ObjectValue<T>(this JObject json, string key)
        {
            return json.SelectToken(key).ToObject<T>();
        }
    }
}