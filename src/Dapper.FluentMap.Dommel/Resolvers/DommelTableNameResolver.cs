using System;
using Dapper.FluentMap.Dommel.Mapping;
using static Dommel.DommelMapper;

namespace Dapper.FluentMap.Dommel.Resolvers
{
    /// <summary>
    /// Implements the <see cref="ITableNameResolver"/> interface by using the configured mapping.
    /// </summary>
    public class DommelTableNameResolver : ITableNameResolver
    {
        private static readonly DefaultTableNameResolver _defaultTableNameResolver = new DefaultTableNameResolver();

        /// <inheritdoc />
        public string ResolveTableName(Type type)
        {
            if (FluentMapper.Configuration.EntityMaps.TryGetValue(type, out var entityMap) &&
                entityMap is IDommelEntityMap mapping)
            {
                return mapping.TableName;
            }

            return _defaultTableNameResolver.ResolveTableName(type);
        }
    }
}
