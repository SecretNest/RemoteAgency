using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    static class DictionaryTryGetValueWithExplicitConversion
    {
        internal static bool TryGetValue<TKey, TValue, TTarget>
            (this Dictionary<TKey, TValue> source,
            TKey key, out TTarget value)
            where TTarget : TValue
        {
            TValue inner;
            if (!source.TryGetValue(key, out inner))
            {
                value = default(TTarget);
                return false;
            }
            else
            {
                value = (TTarget)inner;
                return true;
            }
        }
    }
}
