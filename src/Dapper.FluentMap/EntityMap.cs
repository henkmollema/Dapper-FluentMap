using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Dapper.FluentMap
{
    /// <summary>
    /// Represents an entity mapping class.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to configure the mapping for.</typeparam>
    public abstract class EntityMap<TEntity>
        where TEntity : class
    {
        protected EntityMap()
        {
            Properties = new List<IPropertyMap>();
        }

        /// <summary>
        /// Gets the collection of mapper properties on <typeparamref name="TEntity"/>.
        /// </summary>
        public IList<IPropertyMap> Properties { get; private set; }

        /// <summary>
        /// Maps a specific property of <typeparamref name="TEntity"/> to a column name in the datastore.
        /// </summary>
        /// <param name="expression">Expression to the property on <typeparamref name="TEntity"/>.</param>
        /// <returns>The same instance of <see cref="T:Dapper.FluentMap.PropertyMap"/>. This enabled a fluent API.</returns>
        protected IPropertyMap Map(Expression<Func<TEntity, object>> expression)
        {
            var info = (PropertyInfo)ReflectionHelper.GetMemberInfo(expression);

            // todo: validate duplicate mappings.
            IPropertyMap propertyMap = new PropertyMap(info);
            Properties.Add(propertyMap);
            return propertyMap;
        }
    }
}
