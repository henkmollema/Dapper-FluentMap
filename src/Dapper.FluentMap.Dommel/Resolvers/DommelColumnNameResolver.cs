using System.Linq;
using System.Reflection;
using Dapper.FluentMap.Dommel.Mapping;
using Dapper.FluentMap.Mapping;
using Dommel;

namespace Dapper.FluentMap.Dommel.Resolvers
{
    /// <summary>
    /// Implements the <see cref="DommelMapper.IColumnNameResolver"/> interface by using the configured mapping.
    /// </summary>
    public class DommelColumnNameResolver : DommelMapper.IColumnNameResolver
    {
        private readonly DommelMapper.IColumnNameResolver DefaultResolver = new DommelMapper.DefaultColumnNameResolver();

        /// <inheritdoc/>
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
                    var mapping = entityMap as IDommelEntityMap;
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

            return DefaultResolver.ResolveColumnName(propertyInfo);
        }
    }
}
