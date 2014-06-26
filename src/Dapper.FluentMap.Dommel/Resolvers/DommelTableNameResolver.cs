using System;
using Dapper.FluentMap.Dommel.Mapping;
using Dommel;
using DommelMapper = Dommel.Dommel;

namespace Dapper.FluentMap.Dommel.Resolvers
{
    /// <summary>
    /// Implements the <see cref="Dommel.ITableNameResolver"/> interface by using the configured mapping.
    /// </summary>
    public class DommelTableNameResolver : DommelMapper.ITableNameResolver
    {
        public string ResolveTableName(Type type)
        {
            var mapping = FluentMapper.EntityMaps[type] as IDommelEntityMap;

            if (mapping == null)
            {
                // todo: exception, null, fallback resolver or type.Name?
                throw new Exception(string.Format("Could not find the mapping for type '{0}'.", type.FullName));
            }

            return mapping.TableName;
        }
    }
}
