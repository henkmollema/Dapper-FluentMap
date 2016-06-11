using System;
using Dapper.FluentMap.Dommel.Mapping;
using Dapper.FluentMap.Mapping;
using Dommel;

namespace Dapper.FluentMap.Dommel.Resolvers
{
    /// <summary>
    /// Implements the <see cref="DommelMapper.ITableNameResolver"/> interface by using the configured mapping.
    /// </summary>
    public class DommelTableNameResolver : DommelMapper.ITableNameResolver
    {
        /// <inheritdoc />
        public string ResolveTableName(Type type)
        {
            IEntityMap entityMap;
            if (FluentMapper.EntityMaps.TryGetValue(type, out entityMap))
            {
                var mapping = entityMap as IDommelEntityMap;

                if (mapping != null)
                {
                    return mapping.TableName;
                }
            }

            return DommelMapper.Resolvers.Default.TableNameResolver.ResolveTableName(type);
        }
    }
}
