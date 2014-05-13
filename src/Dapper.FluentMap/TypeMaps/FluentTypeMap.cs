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
        /// Intializes a new instance of the <see cref="T:Dapper.FluentMap.FluentMapTypeMapper"/> class 
        /// which uses the <see cref="T:Dapper.CustomPropertyTypeMap"/> and <see cref="T:Dapper.DefaultTypeMap"/>
        /// as mapping strategies.
        /// </summary>
        public FluentMapTypeMap()
            : base(new CustomPropertyTypeMap(typeof (TEntity), GetPropertyInfo), new DefaultTypeMap(typeof (TEntity)))
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

            EntityMap entityMap;
            if (FluentMapper.EntityMappers.TryGetValue(type, out entityMap))
            {
                var propertyMaps = entityMap.PropertyMaps;

                // Find the mapping for the column name.
                var propertyMap = propertyMaps.FirstOrDefault(m => m.ColumnName == columnName);

                if (propertyMap != null)
                {
                    if (!propertyMap.Ignored)
                    {
                        TypePropertyMapCache.Add(cacheKey, propertyMap.PropertyInfo);
                        return propertyMap.PropertyInfo;
                    }
                }
            }

            // If we get here, the property was not mapped.
            TypePropertyMapCache.Add(cacheKey, null);
            return null;
        }
    }
}
