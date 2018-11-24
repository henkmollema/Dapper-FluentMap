using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dapper.FluentMap.Conventions;
using Dapper.FluentMap.Mapping;

namespace Dapper.FluentMap.Configuration
{
    /// <summary>
    /// <see cref="IMappingConfiguration"/> implementation for fluent mapping configuration.
    /// </summary>
    public class FluentMapConfiguration : IMappingConfiguration
    {
        private ConcurrentDictionary<Type, IEntityMap> _entityMappings = new ConcurrentDictionary<Type, IEntityMap>();

        /// <inheritdoc />
        public IReadOnlyDictionary<Type, IEntityMap> EntityMaps => new ReadOnlyDictionary<Type, IEntityMap>(_entityMappings);

        /// <inheritdoc />
        public void AddMap<TEntity>(EntityMap<TEntity> entityMapping) =>
            _entityMappings.TryAdd(typeof(TEntity), entityMapping);

        /// <inheritdoc />
        public void AddConvention<TConvention>(Action<FluentConventionConfiguration> configureConvention)
            where TConvention : Convention, new()
        {
            var conventionConfig = new FluentConventionConfiguration(new TConvention());
            configureConvention(conventionConfig);

            // Add the entity maps created by the convention
            foreach (var kvp in conventionConfig.EntityMaps)
            {
                _entityMappings.TryAdd(kvp.Key, kvp.Value);
            }
        }
    }
}
