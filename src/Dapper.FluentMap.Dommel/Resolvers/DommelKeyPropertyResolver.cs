using Dapper.FluentMap.Dommel.Mapping;
using Dapper.FluentMap.Mapping;
using Dommel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static Dommel.DommelMapper;

namespace Dapper.FluentMap.Dommel.Resolvers
{
    /// <summary>
    /// Implements the <see cref="DommelMapper.IKeyPropertyResolver"/> interface by using the configured mapping.
    /// </summary>
    public class DommelKeyPropertyResolver : IKeyPropertyResolver
    {
        private DefaultKeyPropertyResolver _defaultKeyPropertyResolver = new DefaultKeyPropertyResolver();

        /// <inheritdoc/>
        public KeyPropertyInfo[] ResolveKeyProperties(Type type)
        {
            return _defaultKeyPropertyResolver.ResolveKeyProperties(type);
        }
    }
}
