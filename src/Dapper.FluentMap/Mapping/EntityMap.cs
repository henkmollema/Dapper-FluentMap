using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Dapper.FluentMap.Utils;

namespace Dapper.FluentMap.Mapping
{
    /// <summary>
    ///
    /// </summary>
    public interface IEntityMap
    {
        /// <summary>
        ///
        /// </summary>
        ICollection<PropertyMap> PropertyMaps { get; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IReadOnlyDictionary<string, PropertyInfo> Compile();
    }

    /// <summary>
    /// Defines the mapping behavior of an entity.
    /// </summary>
    public class EntityMap<TEntity> : IEntityMap
    {
        private readonly List<PropertyMap> _propertyMapping = new List<PropertyMap>();

        /// <summary>
        /// Gets a collection of <see cref="PropertyMap"/> instances.
        /// </summary>
        public ICollection<PropertyMap> PropertyMaps => _propertyMapping;

        /// <summary>
        /// Gets or sets a value indicating this entity mapping is case-sensitive.
        /// </summary>
        public bool CaseSensitive { get; set; } = true;

        /// <summary>
        /// Marks the current entity mapping as case sensitive. By default <c>true</c>.
        /// </summary>
        public void IsCaseSensitive(bool caseSensitive) => CaseSensitive = caseSensitive;

        /// <summary>
        /// Compiles the current <see cref="IEntityMap"/> instance to a mapping
        /// between column names and <see cref="PropertyInfo"/> instances.
        /// </summary>
        /// <returns>A <see cref="IReadOnlyDictionary{TKey, TValue}"/> which represents the mapping.</returns>
        public IReadOnlyDictionary<string, PropertyInfo> Compile()
        {
            var mapping = PropertyMaps.ToDictionary(m => m.ColumnName, m => m.PropertyInfo, CaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase);
            return new ReadOnlyDictionary<string, PropertyInfo>(mapping);
        }

        /// <summary>
        /// Creates a mapping builder for the property specified in the expression.
        /// </summary>
        /// <param name="mapping">The expression which represents the property to map.</param>
        /// <returns>A <see cref="PropertyMap"/> instance.</returns>
        public PropertyMap Map(Expression<Func<TEntity, object>> mapping)
        {
            // Resolve property info from expression and guard against duplicate mappings.
            var propertyInfo = (PropertyInfo)ReflectionHelper.GetMemberInfo(mapping);
            if (PropertyMaps.Any(builder => builder.PropertyInfo == propertyInfo))
            {
                throw new Exception($"Duplicate mapping detected. Property '{propertyInfo.Name}' is already mapped.");
            }

            var propertyMappingBuilder = CreatePropertyMapping(propertyInfo);
            PropertyMaps.Add(propertyMappingBuilder);
            return propertyMappingBuilder;
        }

        /// <summary>
        /// Creates an instance of <see cref="PropertyMap"/> or a derived type using the specified <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>A <see cref="PropertyMap"/> (or derived) instance.</returns>
        protected virtual PropertyMap CreatePropertyMapping(PropertyInfo propertyInfo) => new PropertyMap(propertyInfo);
    }
}
