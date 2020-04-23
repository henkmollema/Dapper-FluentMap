using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Dapper.FluentMap.Dommel.Mapping;
using Dommel;
using static Dommel.DommelMapper;

namespace Dapper.FluentMap.Dommel.Resolvers
{
    /// <summary>
    /// Implements the <see cref="IKeyPropertyResolver"/> interface by using the configured mapping.
    /// </summary>
    public class DommelKeyPropertyResolver : IKeyPropertyResolver
    {
        private static readonly DefaultKeyPropertyResolver _defaultKeyPropertyResolver = new DefaultKeyPropertyResolver();

        /// <inheritdoc/>
        public ColumnPropertyInfo[] ResolveKeyProperties(Type type)
        {
            if (FluentMapper.Configuration.EntityMaps.TryGetValue(type, out var entityMap))
            {
                if (entityMap is IDommelEntityMap dommelEntityMap)
                {
                    return entityMap
                    .PropertyMaps
                    .OfType<DommelPropertyMap>()
                    .Where(m => m.Key)
                    .Select(m =>
                    {
                        if (m.Identity)
                        {
                            return new ColumnPropertyInfo(m.PropertyInfo, DatabaseGeneratedOption.Identity);
                        }
                        if (m.Computed)
                        {
                            return new ColumnPropertyInfo(m.PropertyInfo, DatabaseGeneratedOption.Computed);
                        }
                        return new ColumnPropertyInfo(m.PropertyInfo);
                    })
                    .ToArray();
                }
            }

            // Fall back to the default mapping strategy.
            return _defaultKeyPropertyResolver.ResolveKeyProperties(type);
        }
    }
}
