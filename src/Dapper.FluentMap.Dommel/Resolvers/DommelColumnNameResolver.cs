using System.Linq;
using System.Reflection;
using static Dommel.DommelMapper;

namespace Dapper.FluentMap.Dommel.Resolvers
{
    /// <summary>
    /// Implements the <see cref="IColumnNameResolver"/> interface by using the configured mapping.
    /// </summary>
    public class DommelColumnNameResolver : IColumnNameResolver
    {
        private static readonly DefaultColumnNameResolver _defaultColumnNameResolver = new DefaultColumnNameResolver();

        /// <inheritdoc/>
        public string ResolveColumnName(PropertyInfo propertyInfo)
        {
            if (propertyInfo.DeclaringType != null)
            {
                if (FluentMapper.Configuration.EntityMaps.TryGetValue(propertyInfo.ReflectedType, out var entityMap))
                {
                    var propertyMap = entityMap.PropertyMaps.FirstOrDefault(m => m.PropertyInfo.Name == propertyInfo.Name);
                    if (propertyMap != null)
                    {
                        return propertyMap.ColumnName;
                    }
                }
            }

            return _defaultColumnNameResolver.ResolveColumnName(propertyInfo);
        }
    }
}
