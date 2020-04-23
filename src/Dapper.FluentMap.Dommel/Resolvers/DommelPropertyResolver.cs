using Dapper.FluentMap.Dommel.Mapping;
using Dommel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using static Dommel.DommelMapper;

namespace Dapper.FluentMap.Dommel.Resolvers
{
    /// <summary>
    /// Implements the <see cref="IPropertyResolver"/> interface by using the configured mapping.
    /// </summary>
    public class DommelPropertyResolver : IPropertyResolver
    {
        private static readonly DefaultPropertyResolver _defaultPropertyResolver = new DefaultPropertyResolver();

        /// <inheritdoc/>
        public IEnumerable<ColumnPropertyInfo> ResolveProperties(Type type)
        {
            if (FluentMapper.Configuration.EntityMaps.TryGetValue(type, out var entityMap))
            {
                foreach (var columnPropertyInfo in _defaultPropertyResolver.ResolveProperties(type))
                {
                    var propertyMap = entityMap.PropertyMaps.FirstOrDefault(p => p.PropertyInfo.Name == columnPropertyInfo.Property.Name);

                    // Determine whether the property is generated
                    var generatedOption = DatabaseGeneratedOption.None;
                    if (propertyMap is DommelPropertyMap dommelProperyMap)
                    {
                        if (dommelProperyMap.Identity)
                        {
                            generatedOption = DatabaseGeneratedOption.Identity;
                        }
                        else if (dommelProperyMap.Computed)
                        {
                            generatedOption = DatabaseGeneratedOption.Computed;
                        }
                    }

                    // Determine whether the property should be ignored.
                    if (propertyMap == null || !propertyMap.Ignored)
                    {
                        yield return new ColumnPropertyInfo(columnPropertyInfo.Property, generatedOption);
                    }
                }
            }
            else
            {
                foreach (var property in _defaultPropertyResolver.ResolveProperties(type))
                {
                    yield return property;
                }
            }
        }
    }
}
