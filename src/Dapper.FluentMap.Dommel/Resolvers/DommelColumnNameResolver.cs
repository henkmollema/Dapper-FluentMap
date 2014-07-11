using System;
using System.Linq;
using System.Reflection;
using Dapper.FluentMap.Dommel.Mapping;
using Dommel;

namespace Dapper.FluentMap.Dommel.Resolvers
{
    /// <summary>
    /// Implements the <see cref="DommelMapper.IColumnNameResolver"/> interface by using the configured mapping.
    /// </summary>
    public class DommelColumnNameResolver : DommelMapper.IColumnNameResolver
    {
        public string ResolveColumnName(PropertyInfo propertyInfo)
        {
            var entityMap = FluentMapper.EntityMaps[propertyInfo.DeclaringType] as IDommelEntityMap;

            if (entityMap == null)
            {
                // todo: exception, null, fallback resolver or type.Name?
                throw new Exception(string.Format("Could not find the mapping for type '{0}'.", propertyInfo.DeclaringType.FullName));
            }

            var propertyMaps = entityMap.PropertyMaps.Where(m => m.PropertyInfo.Name == propertyInfo.Name).ToList();

            if (propertyMaps.Count == 0)
            {
                return null;
                // todo
            }

            if (propertyMaps.Count > 1)
            {
                // todo
            }

            return propertyMaps[0].ColumnName;
        }
    }
}
