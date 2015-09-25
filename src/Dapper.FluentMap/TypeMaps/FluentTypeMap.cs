using System;
using System.Linq;
using System.Reflection;

using Dapper.FluentMap.Mapping;
using System.Collections.Generic;

namespace Dapper.FluentMap.TypeMaps
{
    /// <summary>
    /// Represents a Dapper type mapping strategy which first tries to map the type using a <see cref="T:Dapper.CustomPropertyTypeMap"/>, 
    /// if that fails, the <see cref="T:Dapper.DefaultTypeMap"/> is used as mapping strategy.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    internal class FluentMapTypeMap<TEntity> : MultiTypeMap
    {
        private static readonly Dictionary<string, bool> _ignoredColumnCache = new Dictionary<string, bool>();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Dapper.FluentMap.TypeMaps.FluentTypeMap"/> class 
        /// which uses the <see cref="T:Dapper.CustomPropertyTypeMap"/> and <see cref="T:Dapper.DefaultTypeMap"/>
        /// as mapping strategies.
        /// </summary>
        public FluentMapTypeMap()
            : base(
                new CustomPropertyTypeMap(typeof(TEntity), GetPropertyInfo),
                new DefaultTypeMap(typeof(TEntity)))
        {
        }

        private static string GetCacheKey(Type type, string columnName)
        {
            return string.Format("{0};{1}", type.FullName, columnName);
        }

        /// <summary>
        /// Fills the TypePropertyMapCache and the _ignoredColumnCache for the given type and column
        /// </summary>
        /// <returns>The cache key</returns>
        private static string CacheColumn(Type type, string columnName)
        {
            string cacheKey = GetCacheKey(type, columnName);

            if (!TypePropertyMapCache.ContainsKey(cacheKey))
            {
                _ignoredColumnCache[cacheKey] = true;
                TypePropertyMapCache.Add(cacheKey, null);

                IEntityMap entityMap;
                if (FluentMapper.EntityMaps.TryGetValue(type, out entityMap))
                {
                    var propertyMaps = entityMap.PropertyMaps;

                    foreach (var propertyMap in propertyMaps.Where(m => MatchColumnNames(m, columnName)))
                    {
                        if (!propertyMap.Ignored)
                        {
                            _ignoredColumnCache[cacheKey] = false;
                            TypePropertyMapCache[cacheKey] = propertyMap.PropertyInfo;
                        }
                    }
                }
            }

            return cacheKey;
        }

        private static bool ShallIgnoreColumn(Type type, string columnName)
        {
            // ensure that the column info got cached
            string key = CacheColumn(type, columnName);
            return _ignoredColumnCache[key];
        }

        private static PropertyInfo GetPropertyInfo(Type type, string columnName)
        {
            // ensure that the column info got cached
            string key = CacheColumn(type, columnName);
            return TypePropertyMapCache[key];
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

        public override SqlMapper.IMemberMap GetMember(string columnName)
        {
            // check fluent type map first
            if (ShallIgnoreColumn(typeof(TEntity), columnName))
            {
                return null;
            }

            return base.GetMember(columnName);
        }
    }
}
