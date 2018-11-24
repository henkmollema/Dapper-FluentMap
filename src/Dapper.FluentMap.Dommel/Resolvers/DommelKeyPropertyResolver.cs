using System;
using System.Linq;
using System.Reflection;
using Dapper.FluentMap.Dommel.Mapping;
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
        public PropertyInfo[] ResolveKeyProperties(Type type) => ResolveKeyProperties(type, out var isIdentity);

        /// <inheritdoc/>
        public PropertyInfo[] ResolveKeyProperties(Type type, out bool isIdentity)
        {
            if (!FluentMapper.Configuration.EntityMaps.TryGetValue(type, out var entityMap))
            {
                return _defaultKeyPropertyResolver.ResolveKeyProperties(type, out isIdentity);
            }

            if (entityMap is IDommelEntityMap dommelEntityMap)
            {
                isIdentity = true;
                return entityMap
                    .PropertyMaps
                    .OfType<DommelPropertyMap>()
                    .Where(m => m.Key)
                    .Select(m => m.PropertyInfo)
                    .ToArray();
            }

            // Fall back to the default mapping strategy.
            return _defaultKeyPropertyResolver.ResolveKeyProperties(type, out isIdentity);
        }
    }
}
