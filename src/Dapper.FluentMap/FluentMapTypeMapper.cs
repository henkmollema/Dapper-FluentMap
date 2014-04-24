using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Dapper.FluentMap
{
    public class FluentMapTypeMapper<T> : FallbackTypeMapper
    {
        public FluentMapTypeMapper()
            : base(new SqlMapper.ITypeMap[]
                       {
                           new CustomPropertyTypeMap(typeof (T), GetPropertyInfo),
                           new DefaultTypeMap(typeof (T))
                       })
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

            // Find an instance of the EntityMap<T> based on the type.
            dynamic instance = FluentMapper.Mappers.Single(mapper => mapper.GetType().IsSubclassOf(typeof (EntityMap<>).MakeGenericType(type)));

            // Get the value of the 'Properties' property which contains the mappings.
            var propertyMaps = (IList<IPropertyMap>)instance.GetType().GetProperty("Properties").GetValue(instance);

            // Find the mapping for the column name.
            var propertyMap = propertyMaps.FirstOrDefault(m => m.ColumnName == columnName);

            if (propertyMap != null)
            {
                if (!propertyMap.Ignored)
                {
                    _typePropertyMapCache.Add(cacheKey, propertyMap.PropertyInfo);
                    return propertyMap.PropertyInfo;
                }
            }

            // If we get here, the property was not mapped.
            _typePropertyMapCache.Add(cacheKey, null);
            return null;
        }
    }
}
