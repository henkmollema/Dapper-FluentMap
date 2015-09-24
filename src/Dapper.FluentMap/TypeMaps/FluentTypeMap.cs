using System;
using System.Linq;
using System.Reflection;

using Dapper.FluentMap.Mapping;

namespace Dapper.FluentMap.TypeMaps
{
    /// <summary>
    /// Represents a Dapper type mapping strategy which first tries to map the type using a <see cref="T:Dapper.CustomPropertyTypeMap"/>, 
    /// if that fails, the <see cref="T:Dapper.DefaultTypeMap"/> is used as mapping strategy.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    internal class FluentMapTypeMap<TEntity> : MultiTypeMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Dapper.FluentMap.TypeMaps.FluentTypeMap"/> class 
        /// which uses the <see cref="T:Dapper.CustomPropertyTypeMap"/>
        /// as mapping strategies.
        /// </summary>
        public FluentMapTypeMap()
            : base(
                new CustomPropertyTypeMap(typeof(TEntity), GetPropertyInfo))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Dapper.FluentMap.TypeMaps.FluentTypeMap"/> class 
        /// which uses the <see cref="T:Dapper.CustomPropertyTypeMap"/> and <see cref="T:Dapper.DefaultTypeMap"/>
        /// as mapping strategies.
        /// </summary>
        public FluentMapTypeMap(DefaultTypeMap defaultTypeMap)
            : base(
                new CustomPropertyTypeMap(typeof(TEntity), GetPropertyInfo),
                defaultTypeMap)
        {

        }

        private static PropertyInfo GetPropertyInfo(Type type, string columnName)
        {
            string cacheKey = string.Format("{0};{1}", type.FullName, columnName);

            PropertyInfo info;
            if (TypePropertyMapCache.TryGetValue(cacheKey, out info))
            {
                return info;
            }

            IEntityMap entityMap;
            if (FluentMapper.EntityMaps.TryGetValue(type, out entityMap))
            {
                var propertyMaps = entityMap.PropertyMaps;

                // Find a mapping for the column name which isn't marked as ignored
                var propertyMap = propertyMaps
                    .FirstOrDefault(m => MatchColumnNames(m, columnName) && !m.Ignored);

                if (propertyMap != null)
                {
                    TypePropertyMapCache.Add(cacheKey, propertyMap.PropertyInfo);
                    return propertyMap.PropertyInfo;
                }
            }

            // If we get here, the property was not mapped.
            TypePropertyMapCache.Add(cacheKey, null);
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
