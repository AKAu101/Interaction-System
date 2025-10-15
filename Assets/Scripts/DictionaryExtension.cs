using System.Collections.Generic;

public static class IDictionaryExtension
{
    public static bool SwapEntries<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey first, TKey second)
    {
        if (!dict.ContainsKey(first) || !dict.ContainsKey(second)) return false;

        var entryAtSecond = dict[second];
        var entryAtFirst = dict[first];

        dict.Remove(first);
        dict.Remove(second);
        dict.Add(first, entryAtSecond);
        dict.Add(second, entryAtFirst);

        return true;
    }
}