using System.Collections.Generic;

public static class DictionaryExtension
{
    public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> me, IDictionary<TKey, TValue> target)
    {
        foreach (var item in target)
        {
            me[item.Key] = item.Value;
        }
    }
}