using System;
using System.Linq;
using System.Reflection;
using Dapper.FluentMap.Mapping;

namespace Dapper.FluentMap.TypeMaps
{
    /// <summary>
    /// Represents a Dapper type mapping strategy which first tries to map the type using a
    /// <see cref="T:Dapper.CustomPropertyTypeMap"/>
    /// with the configured conventions. <see cref="T:Dapper.DefaultTypeMap"/> is used as fallback mapping strategy.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class FluentConventionTypeMap<TEntity> : MultiTypeMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Dapper.FluentMap.TypeMaps.FluentConventionTypeMap"/> class
        /// which uses the <see cref="T:Dapper.CustomPropertyTypeMap"/> and <see cref="T:Dapper.DefaultTypeMap"/>
        /// as mapping strategies.
        /// </summary>
        public FluentConventionTypeMap()
            : base(new CustomPropertyTypeMap(typeof(TEntity), GetPropertyInfo), new DefaultTypeMap(typeof(TEntity)))
        {
        }

        private static PropertyInfo GetPropertyInfo(Type type, string columnName)
        {
            var cacheKey = $"{type.FullName};{columnName}";
            if (TypePropertyMapCache.TryGetValue(cacheKey, out var info))
            {
                return info;
            }

            if (FluentMapper.TypeConventions.TryGetValue(type, out var conventions))
            {
                foreach (var convention in conventions)
                {
                    // Find property map for current type and column name.
                    var maps = convention.PropertyMaps
#if NETSTANDARD1_3
                                         // HACK: ReflectedType isn't available on.NET Standard 1.3,
                                         // this will cause issues when mapping derived entities.
                                         .Where(map => map.PropertyInfo.DeclaringType == type &&
                                                       MatchColumnNames(map, columnName))
#else
                                         .Where(map => map.PropertyInfo.ReflectedType == type &&
                                                       MatchColumnNames(map, columnName))
#endif
                                         .ToList();

                    if (maps.Count > 1)
                    {
                        const string msg = "Finding mappings for column '{0}' yielded more than 1 PropertyMap. The conventions should be more specific. Type: '{1}'. Convention: '{2}'.";
                        throw new Exception(string.Format(msg, columnName, type, convention));
                    }

                    if (maps.Count == 0)
                    {
                        // This convention has no property maps, continue to next convention.
                        continue;
                    }

                    info = maps[0].PropertyInfo;
                    TypePropertyMapCache.TryAdd(cacheKey, info);
                    return info;
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
