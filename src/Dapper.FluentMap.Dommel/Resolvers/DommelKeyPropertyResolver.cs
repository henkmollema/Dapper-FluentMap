using System;
using System.Linq;
using System.Reflection;
using Dapper.FluentMap.Dommel.Mapping;
using Dommel;
using DommelMapper = Dommel.Dommel;

namespace Dapper.FluentMap.Dommel.Resolvers
{
    /// <summary>
    /// Implements the <see cref="Dommel.IKeyPropertyResolver"/> interface by using the configured mapping.
    /// </summary>
    public class DommelKeyPropertyResolver : DommelMapper.IKeyPropertyResolver
    {
        public PropertyInfo ResolveKeyProperty(Type type)
        {
            var entityMap = FluentMapper.EntityMaps[type] as IDommelEntityMap;
            if (entityMap == null)
            {
                // todo: exception, null, fallback resolver or type.Name?
                throw new Exception(string.Format("Could not find the mapping for type '{0}'.", type.FullName));
            }

            var keyPropertyMaps = entityMap.PropertyMaps.OfType<DommelPropertyMap>().Where(e => e.Key).ToList();
            if (keyPropertyMaps.Count == 0)
            {
                // todo: exception, null or fallback?
                throw new Exception(string.Format("Could not find the key property for type '{0}'.", type.FullName));
            }

            if (keyPropertyMaps.Count > 1)
            {
                // todo: exception, null or fallback?
                string msg = string.Format("Found multiple key properties on type '{0}'. This is not yet supported. The following key properties were found:{1}{2}",
                                           type.FullName,
                                           Environment.NewLine,
                                           string.Join(Environment.NewLine, keyPropertyMaps.Select(t => t.PropertyInfo.Name)));

                throw new Exception(msg);
            }

            return keyPropertyMaps[0].PropertyInfo;
        }
    }
}
