using System.Reflection;
using Dapper.FluentMap.Mapping;

namespace Dapper.FluentMap.Dommel.Mapping
{
    /// <summary>
    /// A <see cref="IEntityMap"/> for Dommel.
    /// </summary>
    public interface IDommelEntityMap : IEntityMap
    {
        /// <summary>
        /// Gets or sets the name of the table of the entity.
        /// </summary>
        string TableName { get; set; }
    }

    /// <summary>
    /// A <see cref="IEntityMap"/> for Dommel.
    /// </summary>
    public class DommelEntityMap<TEntity> : EntityMap<TEntity>, IDommelEntityMap
    {
        /// <inheritdoc />
        protected override PropertyMap CreatePropertyMapping(PropertyInfo propertyInfo) => new DommelPropertyMap(propertyInfo);

        /// <inheritdoc />
        public string TableName { get; set; }

        /// <summary>
        /// Maps the current entity to the specified table name.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        public DommelEntityMap<TEntity> ToTable(string tableName)
        {
            TableName = tableName;
            return this;
        }
    }

    /// <summary>
    /// Dommel extensions for <see cref="EntityMap{TEntity}"/>.
    /// </summary>
    public static class EntityMapExtensions
    {
        /// <summary>
        /// Maps the current entity to the specified table name.
        /// </summary>
        /// <param name="mapping">The <see cref="EntityMap{TEntity}"/> instance.</param>
        /// <param name="tableName">The name of the table.</param>
        public static EntityMap<TEntity> ToTable<TEntity>(this EntityMap<TEntity> mapping, string tableName)
        {
            if (mapping is DommelEntityMap<TEntity> dommelMapping)
            {
                dommelMapping.ToTable(tableName);
            }

            return mapping;
        }
    }
}
