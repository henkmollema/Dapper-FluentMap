using Dapper.FluentMap.Dommel.Mapping;
using Dapper.FluentMap.Mapping;
using Dommel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static Dommel.DommelMapper;

namespace Dapper.FluentMap.Dommel.Resolvers
{
    /// <summary>
    /// Implements the <see cref="DommelMapper.IColumnNameResolver"/> interface by using the configured mapping.
    /// </summary>
    public class DommelColumnNameResolver : IColumnNameResolver
    {
        /// <inheritdoc/>

        private DefaultColumnNameResolver _defaultColumnNameResolver = new DefaultColumnNameResolver();
        public string ResolveColumnName(PropertyInfo propertyInfo)
        {
            if (propertyInfo.DeclaringType != null)
            {
                IEntityMap entityMap;
#if NETSTANDARD1_3
                if (FluentMapper.EntityMaps.TryGetValue(propertyInfo.DeclaringType, out entityMap))

#else
                if (FluentMapper.EntityMaps.TryGetValue(propertyInfo.ReflectedType, out entityMap))
#endif
                {
                    IDommelEntityMap mapping = entityMap as IDommelEntityMap;
                    if (mapping != null)
                    {
                        var propertyMaps = entityMap.PropertyMaps.Where(m => m.PropertyInfo.Name == propertyInfo.Name).ToList();
                        if (propertyMaps.Count == 1)
                        {
                            return propertyMaps[0].ColumnName;
                        }
                    }
                }
            }

            return _defaultColumnNameResolver.ResolveColumnName(propertyInfo);
        }
    }
}
