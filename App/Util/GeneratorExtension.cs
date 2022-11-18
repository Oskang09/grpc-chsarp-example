using System;
using System.Numerics;

namespace amantiq.util
{
    public static class GeneratorExtension
    {
        private static Random rnd = new Random();

        public static string GenerateUNIQ()
        {
            return DateTime.UtcNow.ToString("yyMMddHHmmssff") + getSuffix(4);
        }

        public static string GenerateUNIQWithPrefix(string prefix)
        {
            string mid = DateTime.UtcNow.ToString("yyMMddHHmmssff");
            return prefix + mid + getSuffix(4 - prefix.Length);
        }

        private static string getSuffix(int num)
        {
            string[] array = new string[num];
            for (int i = 0; i < num; i++) array[i] = rnd.Next(10).ToString();
            return string.Join("", array);
        }
    }
}