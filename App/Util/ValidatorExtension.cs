using System;

namespace amantiq.util
{
    public static class ValidatorExtension
    {
        public static string EnumValues<T>()
        {
            return "enum:" + string.Join(",", Enum.GetNames(typeof(T)));
        }
    }
}