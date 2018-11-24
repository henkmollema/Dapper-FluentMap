using System;
using System.Collections.Generic;
using Dapper.FluentMap.Conventions;
using Dapper.FluentMap.Mapping;

namespace Dapper.FluentMap.Configuration
{
    /// <summary>
    /// Contains the configuration for Dapper.FluentMap.
    /// </summary>
    public interface IMappingConfiguration
    {
        /// <summary>
        /// Gets the collection of configured entity mappings.
        /// </summary>
        IReadOnlyDictionary<Type, IEntityMap> EntityMaps { get; }

        /// <summary>
        /// Adds the specified <see cref="IEntityMap"/> instance to this configuration instance.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entityMapping">The <see cref="IEntityMap"/> instance.</param>
        void AddMap<TEntity>(EntityMap<TEntity> entityMapping);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TConvention"></typeparam>
        /// <param name="configureConvention"></param>
        void AddConvention<TConvention>(Action<FluentConventionConfiguration> configureConvention)
            where TConvention : Convention, new();
    }
}
