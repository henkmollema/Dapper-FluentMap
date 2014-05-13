using System.Collections.Generic;
using System.Linq;

namespace Dapper.FluentMap.Utils
{
    public static class DictionairyExtensions
    {
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
        }

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

        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, IList<TValue>> dict, TKey key, IEnumerable<TValue> values)
        {
            if (dict.ContainsKey(key))
            {
                foreach (var value in values)
                {
                    dict[key].Add(value);
                }
            }
            else
            {
                dict.Add(key, values.ToList());
            }
        }
    }
}
