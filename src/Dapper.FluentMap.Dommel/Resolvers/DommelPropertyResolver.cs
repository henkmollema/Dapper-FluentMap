using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dapper.FluentMap.Dommel.Mapping;
using Dommel;

namespace Dapper.FluentMap.Dommel.Resolvers
{
    public class DommelPropertyResolver : DommelMapper.PropertyResolverBase
    {
        public override IEnumerable<PropertyInfo> ResolveProperties(Type type)
        {
            var entityMap = FluentMapper.EntityMaps[type] as IDommelEntityMap;
            if (entityMap == null)
            {
                foreach (var property in FilterComplexTypes(type.GetProperties()))
                {
                    yield return property;
                }
            }
            else
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
        }
    }
}