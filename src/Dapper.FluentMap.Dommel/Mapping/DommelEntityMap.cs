using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Dapper.FluentMap.Mapping;
using Dapper.FluentMap.Utils;

namespace Dapper.FluentMap.Dommel.Mapping
{
    public abstract class DommelEntityMap : EntityMap
    {
        protected void ToTable(string tableName)
        {
            TableName = tableName;
        }

        internal string TableName { get; private set; }
    }

    public abstract class DommelEntityMap<TEntity> : DommelEntityMap, IEntityMap<TEntity>
    {
        protected DommelPropertyMap Map(Expression<Func<TEntity, object>> expression)
        {
            PropertyInfo info = (PropertyInfo)ReflectionHelper.GetMemberInfo(expression);

            DommelPropertyMap map = new DommelPropertyMap(info);
            ThrowIfDuplicateMapping(map);
            PropertyMaps.Add(map);
            return map;
        }

        private void ThrowIfDuplicateMapping(PropertyMap map)
        {
            if (PropertyMaps.Any(p => p.PropertyInfo.Name == map.PropertyInfo.Name))
            {
                throw new Exception(string.Format("Duplicate mapping. Property '{0}' is already mapped to column '{1}'.", map.PropertyInfo.Name, map.ColumnName));
            }
        }
    }
}
