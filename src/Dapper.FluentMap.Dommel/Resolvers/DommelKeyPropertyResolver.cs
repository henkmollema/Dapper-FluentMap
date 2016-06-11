﻿using System;
using System.Linq;
using System.Reflection;
using Dapper.FluentMap.Dommel.Mapping;
using Dapper.FluentMap.Mapping;
using Dommel;

namespace Dapper.FluentMap.Dommel.Resolvers
{
    /// <summary>
    /// Implements the <see cref="DommelMapper.IKeyPropertyResolver"/> interface by using the configured mapping.
    /// </summary>
    public class DommelKeyPropertyResolver : DommelMapper.IKeyPropertyResolver
    {
        /// <inheritdoc />
        public PropertyInfo ResolveKeyProperty(Type type)
        {
            IEntityMap entityMap;
            if (FluentMapper.EntityMaps.TryGetValue(type, out entityMap))
            {
                var mapping = entityMap as IDommelEntityMap;
                if (mapping != null)
                {
                    var keyPropertyMaps = entityMap.PropertyMaps.OfType<DommelPropertyMap>().Where(e => e.Key).ToList();

                    if (keyPropertyMaps.Count == 1)
                    {
                        return keyPropertyMaps[0].PropertyInfo;
                    }

                    if (keyPropertyMaps.Count > 1)
                    {
                        var msg = string.Format("Found multiple key properties on type '{0}'. This is not yet supported. The following key properties were found:{1}{2}",
                            type.FullName,
                            Environment.NewLine,
                            string.Join(Environment.NewLine, keyPropertyMaps.Select(t => t.PropertyInfo.Name)));

                        throw new Exception(msg);
                    }
                }
            }

            return DommelMapper.Resolvers.Default.KeyPropertyResolver.ResolveKeyProperty(type);
        }
    }
}
