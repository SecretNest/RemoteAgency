using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections.Generic
{
    #if netfx
    static class DictionaryTryAddHelper
    {
        public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
                return false;
            dictionary.Add(key, value);
            return true;
        }
    }
    #endif
}
