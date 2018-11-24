using System;
using System.Collections.Generic;
using System.Reflection;

namespace Dapper.FluentMap.TypeMaps
{
    /// <summary>
    /// Represents a Dapper type mapping strategy using the configured entity mappings.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class FluentTypeMap<TEntity> : FluentTypeMap
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="mapping"></param>
        public FluentTypeMap(IReadOnlyDictionary<string, PropertyInfo> mapping) : base(typeof(TEntity), mapping)
        {
        }
    }

    /// <summary>
    /// Represents a Dapper type mapping strategy using the configured entity mappings.
    /// </summary>
    public class FluentTypeMap : MultiTypeMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluentTypeMap{TEntity}"/> class using the specified <paramref name="mapping"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="mapping">A dictionary which represents the mapping of the current entity.</param>
        public FluentTypeMap(Type type, IReadOnlyDictionary<string, PropertyInfo> mapping)
            : base(
                  new CustomPropertyTypeMap(
                    type,
                    (t, c) => mapping.TryGetValue(c, out var propertyInfo) ? propertyInfo : null),
                  new DefaultTypeMap(type))
        {
        }
    }
}
