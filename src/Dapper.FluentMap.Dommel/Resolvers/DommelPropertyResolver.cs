using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dapper.FluentMap.Mapping;
using Dommel;

namespace Dapper.FluentMap.Dommel.Resolvers
{
    public class DommelPropertyResolver : DommelMapper.PropertyResolverBase
    {
        protected override IEnumerable<PropertyInfo> FilterComplexTypes(IEnumerable<PropertyInfo> properties)
        {
            foreach (var propertyInfo in properties)
            {
                Type type = propertyInfo.PropertyType;
                type = Nullable.GetUnderlyingType(type) ?? type;
                if (type.IsPrimitive || type.IsEnum || PrimitiveTypes.Contains(type))
                {
                    yield return propertyInfo;
                }
            }
        }

        public override IEnumerable<PropertyInfo> ResolveProperties(Type type)
        {
            IEntityMap entityMap;
            if (FluentMapper.EntityMaps.TryGetValue(type, out entityMap))
            {
                foreach (var property in FilterComplexTypes(type.GetProperties()))
                {
                    var propertyMap = entityMap.PropertyMaps.FirstOrDefault(p => p.PropertyInfo.Name == property.Name);
                    if (propertyMap != null)
                    {
                        yield return !propertyMap.Ignored ? property : null;
                    }
                    else
                    {
                        yield return property;
                    }
                }
            }
            else
            {
                foreach (var property in DommelMapper.Resolvers.Default.PropertyResolver.ResolveProperties(type))
                {
                    yield return property;
                }
            }
        }
    }
}