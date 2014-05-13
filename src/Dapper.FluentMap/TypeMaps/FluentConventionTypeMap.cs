using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dapper.FluentMap.Conventions;

namespace Dapper.FluentMap.TypeMaps
{
    public class FluentConventionTypeMap<T> : MultiTypeMap
    {
        public FluentConventionTypeMap()
            : base(new CustomPropertyTypeMap(typeof (T), GetPropertyInfo))
        {
        }

        private static PropertyInfo GetPropertyInfo(Type type, string columnName)
        {
            string cacheKey = string.Format("{0};{1}", type.FullName, columnName);

            PropertyInfo info;
            if (_typePropertyMapCache.TryGetValue(cacheKey, out info))
            {
                return info;
            }

            IList<Convention> conventions;
            if (FluentMapper.TypeConventions.TryGetValue(type, out conventions))
            {
                foreach (var convention in conventions)
                {
                    var maps = convention.PropertyMaps.Where(c => c.ColumnName == columnName).ToList();

                    if (maps.Count > 1)
                    {
                        const string msg = "Finding mappings for column '{0}' yielded more than 1 PropertyMap. The conventions should be more specific. Type: '{1}'. Convention: '{2}'.";
                        throw new Exception(string.Format(msg, columnName, type, convention));
                    }

                    if (maps.Count == 0)
                    {
                        return null;
                    }

                    info = maps[0].PropertyInfo;
                    _typePropertyMapCache.Add(cacheKey, info);
                    return info;
                }
            }

            // If we get here, the property was not mapped.
            _typePropertyMapCache.Add(cacheKey, null);
            return null;
        }
    }
}
