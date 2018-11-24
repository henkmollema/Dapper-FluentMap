using System;
using Dapper.FluentMap.Configuration;
using Dapper.FluentMap.Dommel.Mapping;

namespace Dapper.FluentMap.Dommel
{
    /// <summary>
    /// Extensions to configure Dommel mappings.
    /// </summary>
    public static class DommelFluentMapExtensions
    {
        /// <summary>
        /// Configures the Dommel mapping for the specified <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="config">The configuration to add the mapping to.</param>
        /// <param name="action">A callback to configure the mapping.</param>
        public static void DommelEntity<TEntity>(this IMappingConfiguration config, Action<DommelEntityMap<TEntity>> action)
        {
            var mapping = new DommelEntityMap<TEntity>();
            action(mapping);
            config.AddMap(mapping);
        }
    }
}
