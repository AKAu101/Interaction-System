using System.Collections.Generic;


public static class IDictionaryExtension
{
    public static bool SwapEntries<TKey,TValue>(this IDictionary<TKey,TValue> dict,TKey first, TKey second)
    {
        var entryAtSecond = dict[second];
        var entryAtFirst = dict[first];
        if (entryAtSecond == null || entryAtFirst == null) return false;
        dict.Remove(first);
        dict.Remove(second);
        dict.Add(first,entryAtSecond);
        dict.Add(second,entryAtFirst);
        //dict[second] = dict[first];
        //dict[first] = entryAtSecond;
        return true;
    }
}