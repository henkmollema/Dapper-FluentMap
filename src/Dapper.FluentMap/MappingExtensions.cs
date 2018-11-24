using System;
using Dapper.FluentMap.Configuration;
using Dapper.FluentMap.Mapping;

namespace Dapper.FluentMap
{
    /// <summary>
    /// Extensions for building <see cref="IMappingConfiguration"/>.
    /// </summary>
    public static class MappingExtensions
    {
        /// <summary>
        /// Configures inline mapping for <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="config">The <see cref="IMappingConfiguration"/> instance to add the mapping to.</param>
        /// <param name="action">A delegate which configures the mapping for <typeparamref name="TEntity"/>.</param>
        public static void Entity<TEntity>(this IMappingConfiguration config, Action<EntityMap<TEntity>> action)
        {
            var builder = new EntityMap<TEntity>();
            action(builder);
            config.AddMap(builder);
        }
    }
}
