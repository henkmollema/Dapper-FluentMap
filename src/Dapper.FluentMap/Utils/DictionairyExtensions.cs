using System.Collections.Generic;

namespace Dapper.FluentMap.Utils
{
    public static class DictionairyExtensions
    {
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, IList<TValue>> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key].Add(value);
            }
            else
            {
                dict.Add(key, new[] { value });
            }
        }
    }
}
