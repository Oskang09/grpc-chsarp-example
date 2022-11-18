
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace amantiq
{
    public class ValidationException : Exception
    {
        public ValidationException(string error) : base(error) { }
    }

    public class Controller
    {
        private static MemoryStream stream = new MemoryStream();
        private static Regex rAlphaNum = new Regex("^[a-zA-Z0-9]*$");
        private static Regex rAlpha = new Regex("^[a-zA-Z]*$");

        protected static Database GetDatabase
        {
            get
            {
                return new Database();
            }
        }

        protected class Rules
        {
            public object type { get; set; }
            public object childRule { get; set; }
            public string rule { get; set; }
            private Rules(string rule, object childRule, string type)
            {
                this.rule = rule;
                this.childRule = childRule;
                this.type = type;
            }

            public static Rules Array(string rule, string childRule)
            {
                return new Rules(rule, childRule, "array");
            }
            public static Rules Array(string childRule)
            {
                return new Rules("", childRule, "array");
            }

            public static Rules ArrayMap(object childRule)
            {
                return new Rules("", childRule, "arrayMap");
            }

            public static Rules ArrayMap(string rule, object childRule)
            {
                return new Rules(rule, childRule, "arrayMap");
            }

            public static Rules Map(object childRule)
            {
                return new Rules("", childRule, "map");
            }
        }

        protected void Validate(JObject jObject, object validation)
        {
            Validate(jObject, "", validation);
        }

        private void Validate(JObject jObject, string property, object validation)
        {
            var type = validation.GetType();
            foreach (PropertyInfo entry in type.GetProperties())
            {
                String currentKey = (property.Length != 0 ? property + "." : "") + entry.Name;
                JToken token = jObject.GetValue(entry.Name);
                object rules = entry.GetValue(validation, null);
                if (rules.GetType() == typeof(Rules))
                {
                    Rules customRules = (Rules)rules;
                    if (customRules.type.Equals("arrayMap"))
                    {
                        if (IsNotType(token, JTokenType.Array))
                        {
                            throw new ValidationException(String.Format("Field '{0}' is not an array", currentKey));
                        }

                        JArray array = (JArray)token;
                        if (customRules.rule != "")
                        {
                            validateArray(array, currentKey, customRules.rule.Split("|"));
                        }

                        for (int i = 0; i < array.Count; i++)
                        {
                            JToken arrayToken = token[i];
                            if (IsNotType(arrayToken, JTokenType.Object))
                            {
                                throw new ValidationException(String.Format("Field '{0}' is not an object", currentKey + "[" + i.ToString() + "]"));
                            }
                            Validate((JObject)arrayToken, currentKey + "[" + i.ToString() + "]", customRules.childRule);
                        }
                    }

                    if (customRules.type.Equals("array"))
                    {
                        if (IsNotType(token, JTokenType.Array))
                        {
                            throw new ValidationException(String.Format("Field '{0}' is not an array", currentKey));
                        }

                        JArray array = (JArray)token;
                        if (customRules.rule != "")
                        {
                            validateArray(array, currentKey, customRules.rule.Split("|"));
                        }

                        for (int i = 0; i < array.Count; i++)
                        {
                            JToken arrayToken = token[i];
                            foreach (string rule in customRules.childRule.ToString().Split("|"))
                            {
                                validateSingle(arrayToken, currentKey + "[" + i.ToString() + "]", rule);
                            }
                        }
                    }

                    if (customRules.type.Equals("map"))
                    {
                        if (IsNotType(token, JTokenType.Object))
                        {
                            throw new ValidationException(String.Format("Field '{0}' is not an object", currentKey));
                        }

                        Validate((JObject)token, currentKey, customRules.childRule);
                    }
                }

                if (rules.GetType() == typeof(string))
                {
                    foreach (string rule in rules.ToString().Split("|"))
                    {
                        validateSingle(token, currentKey, rule);
                    }
                }
            }
        }

        private void validateArray(JArray array, string currentKey, string[] rules)
        {
            foreach (string rule in rules)
            {
                string[] keys = rule.Split(":");
                switch (keys[0])
                {
                    case "required":
                        if (array.Count == 0) throw new ValidationException(String.Format("Field '{0}' is an empty array", currentKey));
                        continue;
                    case "size":
                        if (keys.Length != 1)
                        {
                            throw new ValidationException(String.Format("Validate format invalid should be 'size=1'"));
                        }

                        int size = int.Parse(keys[1]);
                        if (array.Count != size) throw new ValidationException(String.Format("Field '{0}' array size is not {1}", currentKey, size));
                        continue;
                    case "min":
                        if (keys.Length != 1)
                        {
                            throw new ValidationException(String.Format("Validate format invalid should be 'size=1'"));
                        }

                        int min = int.Parse(keys[1]);
                        if (array.Count < min) throw new ValidationException(String.Format("Field '{0}' array size is smaller than {1}", currentKey, min));
                        continue;
                    case "max":
                        if (keys.Length != 1)
                        {
                            throw new ValidationException(String.Format("Validate format invalid should be 'size=1'"));
                        }

                        int max = int.Parse(keys[1]);
                        if (array.Count != max) throw new ValidationException(String.Format("Field '{0}' array size is bigger than {1}", currentKey, max));
                        continue;
                }
            }
        }
        private void validateSingle(JToken token, string property, string validate)
        {
            string[] keys = validate.Split(":");
            switch (keys[0])
            {
                case "required":
                    if (IsEmpty(token))
                    {
                        throw new ValidationException(String.Format("Field '{0}' is required", property));
                    }
                    break;
                case "enum":
                    if (keys.Length <= 1)
                    {
                        throw new ValidationException(String.Format("Validate format invalid should be 'enum:value1,value2'"));
                    }

                    var acceptedEnums = keys[1].Split(",");
                    if (!IsEmpty(token) && (IsNotType(token, JTokenType.String) || IsNotIn(token, acceptedEnums)))
                    {
                        throw new ValidationException(String.Format("Field {0} is not equals to any value in {1}", property, string.Join(",", acceptedEnums)));
                    }
                    break;
                case "boolean":
                    if (!IsEmpty(token) && IsNotType(token, JTokenType.Boolean))
                    {
                        throw new ValidationException(String.Format("Field '{0}' is not a boolean", property));
                    }
                    break;
                case "datetime":
                    if (!IsEmpty(token) && IsNotType(token, JTokenType.Date))
                    {
                        throw new ValidationException(String.Format("Field '{0}' is not a datetime", property));
                    }
                    break;
                case "string":
                    if (!IsEmpty(token) && IsNotType(token, JTokenType.String))
                    {
                        throw new ValidationException(String.Format("Field '{0}' is not a string", property));
                    }
                    break;
                case "integer":
                    if (!IsEmpty(token) && IsNotType(token, JTokenType.Integer))
                    {
                        throw new ValidationException(String.Format("Field '{0}' is not an integer", property));
                    }
                    break;
                case "numeric":
                    if (!IsEmpty(token) && IsNotType(token, JTokenType.Integer, JTokenType.Float))
                    {
                        throw new ValidationException(String.Format("Field '{0}' is not numeric value", property));
                    }
                    break;
                case "alpha":
                    if (!IsEmpty(token) && (IsNotType(token, JTokenType.String) || !token.Value<String>().All(char.IsLetter)))
                    {
                        throw new ValidationException(String.Format("Field '{0}' is not a alphabet string", property));
                    }
                    break;
                case "alpha_num":
                    if (!IsEmpty(token) && (IsNotType(token, JTokenType.String) || !token.Value<String>().All(char.IsLetterOrDigit)))
                    {
                        throw new ValidationException(String.Format("Field '{0}' is not a alpha numeric string", property));
                    }
                    break;
            }
        }

        private bool IsNotIn(JToken token, params string[] enums)
        {
            if (token == null)
            {
                return true;
            }

            string value = token.Value<string>();
            foreach (String t in enums)
            {
                if (value.Equals(t))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsNotType(JToken token, params JTokenType[] types)
        {
            if (token == null)
            {
                return true;
            }

            foreach (JTokenType type in types)
            {
                if (token.Type == type)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsEmpty(JToken token)
        {
            return (token == null) ||
                   (token.Type == JTokenType.Array && !token.HasValues) ||
                   (token.Type == JTokenType.Object && !token.HasValues) ||
                   (token.Type == JTokenType.String && token.ToString() == String.Empty) ||
                   (token.Type == JTokenType.Null);
        }
    }
}