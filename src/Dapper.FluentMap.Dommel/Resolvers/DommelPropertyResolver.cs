using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dapper.FluentMap.Dommel.Mapping;
using Dapper.FluentMap.Mapping;
using Dommel;

namespace Dapper.FluentMap.Dommel.Resolvers
{
    /// <summary>
    /// Implements the <see cref="IPropertyResolver"/> interface by using the configured mapping.
    /// </summary>
    public class DommelPropertyResolver : DefaultPropertyResolver
    {
        private static readonly IPropertyResolver DefaultResolver = new DefaultPropertyResolver();

        /// <inheritdoc/>
        protected override IEnumerable<PropertyInfo> FilterComplexTypes(IEnumerable<PropertyInfo> properties)
        {
            foreach (var propertyInfo in properties)
            {
                var type = propertyInfo.PropertyType;
                type = Nullable.GetUnderlyingType(type) ?? type;

                if (type.GetTypeInfo().IsPrimitive || type.GetTypeInfo().IsEnum || PrimitiveTypes.Contains(type))
                {
                    yield return propertyInfo;
                }
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<ColumnPropertyInfo> ResolveProperties(Type type)
        {
            IEntityMap entityMap;
            if (FluentMapper.EntityMaps.TryGetValue(type, out entityMap))
            {
                foreach (var property in FilterComplexTypes(type.GetProperties()))
                {
                    // Determine whether the property should be ignored.
                    var propertyMap = entityMap.PropertyMaps.FirstOrDefault(p => p.PropertyInfo.Name == property.Name);
                    if (propertyMap == null || !propertyMap.Ignored)
                    {
                        var dommelPropertyMap = propertyMap as DommelPropertyMap;
                        if (dommelPropertyMap != null)
                        {
                            yield return new ColumnPropertyInfo(property, isKey: dommelPropertyMap.Key);
                        }
                        else
                        {
                            yield return new ColumnPropertyInfo(property);
                        }
                    }
                }
            }
            else
            {
                foreach (var property in DefaultResolver.ResolveProperties(type))
                {
                    yield return property;
                }
            }
        }
    }
}
