using System;
using System.Linq.Expressions;
using System.Reflection;
using Dapper.FluentMap.Mapping;
using Dapper.FluentMap.Utils;

namespace Dapper.FluentMap.Dommel.Mapping
{
    public abstract class DommelEntityMap : EntityMap
    {
        protected DommelPropertyMap Map(Expression<Func<PropertyInfo, object>> expression)
        {
            var info = (PropertyInfo)ReflectionHelper.GetMemberInfo(expression);

            var map = new DommelPropertyMap(info);
            PropertyMaps.Add(map);
            return map;
        }

        protected void ToTable(string tableName)
        {
            TableName = tableName;
        }

        internal string TableName { get; set; }
    }
}
