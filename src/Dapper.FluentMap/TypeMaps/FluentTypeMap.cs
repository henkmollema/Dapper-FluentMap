using System;
using System.Linq;
using System.Reflection;
using Dapper.FluentMap.Mapping;

namespace Dapper.FluentMap.TypeMaps
{
    /// <summary>
    /// Represents a Dapper type mapping strategy which first tries to map the type using a
    /// <see cref="T:Dapper.CustomPropertyTypeMap"/>,
    /// if that fails, the <see cref="T:Dapper.DefaultTypeMap"/> is used as mapping strategy.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class FluentMapTypeMap<TEntity> : MultiTypeMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Dapper.FluentMap.TypeMaps.FluentTypeMap"/> class
        /// which uses the <see cref="T:Dapper.CustomPropertyTypeMap"/> and <see cref="T:Dapper.DefaultTypeMap"/>
        /// as mapping strategies.
        /// </summary>
        public FluentMapTypeMap()
            : base(new CustomPropertyTypeMap(typeof(TEntity), GetPropertyInfo), new DefaultTypeMap(typeof(TEntity)))
        {
        }

        private static PropertyInfo GetPropertyInfo(Type type, string columnName)
        {
            var cacheKey = $"{type.FullName};{columnName}";

            PropertyInfo info;
            if (TypePropertyMapCache.TryGetValue(cacheKey, out info))
            {
                return info;
            }

            IEntityMap entityMap;
            if (FluentMapper.EntityMaps.TryGetValue(type, out entityMap))
            {
                var propertyMaps = entityMap.PropertyMaps;

                // Find the mapping for the column name.
                var propertyMap = propertyMaps.FirstOrDefault(m => MatchColumnNames(m, columnName));

                if (propertyMap != null)
                {
                    if (!propertyMap.Ignored)
                    {
                        TypePropertyMapCache.TryAdd(cacheKey, propertyMap.PropertyInfo);
                        return propertyMap.PropertyInfo;
                    }
#if !NETSTANDARD1_3
                    else
                    {
                        var ignoredPropertyInfo = new IgnoredPropertyInfo();
                        TypePropertyMapCache.TryAdd(cacheKey, ignoredPropertyInfo);
                        return ignoredPropertyInfo;
                    }
#endif
                }
            }

            // If we get here, the property was not mapped.
            TypePropertyMapCache.TryAdd(cacheKey, null);
            return null;
        }

        private static bool MatchColumnNames(IPropertyMap map, string columnName)
        {
            var comparison = StringComparison.Ordinal;
            if (!map.CaseSensitive)
            {
                comparison = StringComparison.OrdinalIgnoreCase;
            }

            return string.Equals(map.ColumnName, columnName, comparison);
        }
    }
}
