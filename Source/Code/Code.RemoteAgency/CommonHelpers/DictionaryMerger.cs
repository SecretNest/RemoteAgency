using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    static class DictionaryMerger
    {
        internal static void Merge<TKey, TResult>(this Dictionary<TKey, TResult> main, Dictionary<TKey, TResult> delta)
        {
            foreach (var item in delta)
                if (!main.ContainsKey(item.Key))
                    main.Add(item.Key, item.Value);
        }
    }
}
