using System;

using Dapper.FluentMap.Dommel.Mapping;

using DommelMapper = Dommel.Dommel;

namespace Dapper.FluentMap.Dommel.Resolvers
{
    public class DommelTableNameResolver : DommelMapper.ITableNameResolver
    {
        public string ResolveTableName(Type type)
        {
            DommelEntityMap mapping = FluentMapper.EntityMappers[type] as DommelEntityMap;
            if (mapping == null)
            {
                // todo: exception, null, fallback resolver or type.Name?
                throw new Exception(string.Format("Could not find the mapping for type '{0}'.", type.FullName));
            }

            return mapping.TableName;
        }
    }
}
