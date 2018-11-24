using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Dapper.FluentMap.Conventions;
using Dapper.FluentMap.Mapping;

namespace Dapper.FluentMap.Configuration
{
    /// <summary>
    /// Defines methods for configuring conventions.
    /// </summary>
    public class FluentConventionConfiguration
    {
        private class EntityMap : EntityMap<FluentConventionConfiguration> { }

        private readonly Dictionary<Type, IEntityMap> _entityMaps = new Dictionary<Type, IEntityMap>();
        private readonly Convention _convention;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentConventionConfiguration"/> class,
        /// allowing configuration of conventions.
        /// </summary>
        /// <param name="convention">The convention.</param>
        public FluentConventionConfiguration(Convention convention)
        {
            _convention = convention;
        }

        /// <summary>
        /// Gets a mapping of types and their <see cref="IEntityMap"/> for this convention.
        /// </summary>
        public IReadOnlyDictionary<Type, IEntityMap> EntityMaps => new ReadOnlyDictionary<Type, IEntityMap>(_entityMaps);

        /// <summary>
        /// Configures the current covention for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <returns>The current instance of <see cref="FluentConventionConfiguration"/>.</returns>
        public FluentConventionConfiguration ForEntity<T>()
        {
            var type = typeof(T);
            CreateEntityMap(type);
            return this;
        }

        /// <summary>
        /// Configures the current convention for all the entities in current assembly filtered by the specified namespaces.
        /// </summary>
        /// <param name="namespaces">
        /// An array of namespaces which filter the types in the current assembly.
        /// This parameter is optional.
        /// </param>
        /// <returns>The current instance of <see cref="FluentConventionConfiguration"/>.</returns>
        public FluentConventionConfiguration ForEntitiesInCurrentAssembly(params string[] namespaces)
        {
            foreach (var type in Assembly.GetCallingAssembly().GetExportedTypes())
            {
                if (namespaces != null &&
                    namespaces.Length > 0 &&
                    namespaces.All(n => n != type.Namespace))
                {
                    // Filter by namespace.
                    continue;
                }

                CreateEntityMap(type);
            }

            return this;
        }

        /// <summary>
        /// Configures the current convention for all entities in the specified assembly filtered by the specified namespaces.
        /// </summary>
        /// <param name="assembly">The assembly to scan for entities.</param>
        /// <param name="namespaces">
        /// An array of namespaces which filter the types in <paramref name="assembly"/>.
        /// This parameter is optional.
        /// </param>
        /// <returns>The current instance of <see cref="FluentConventionConfiguration"/>.</returns>
        public FluentConventionConfiguration ForEntitiesInAssembly(Assembly assembly, params string[] namespaces)
        {
            foreach (var type in assembly.GetExportedTypes())
            {
                if (namespaces != null &&
                    namespaces.Length > 0 &&
                    namespaces.All(n => n != type.Namespace))
                {
                    // Filter by namespace.
                    continue;
                }

                CreateEntityMap(type);
            }

            return this;
        }

        private void CreateEntityMap(Type t)
        {
            var entityMap = new EntityMap();
            MapProperties(t, entityMap);
            _entityMaps[t] = entityMap;
        }

        private void MapProperties(Type type, IEntityMap entityMap)
        {
            foreach (var property in type.GetProperties())
            {
                foreach (var config in _convention.ConventionConfigurations)
                {
                    if (!config.PropertyPredicates.All(p => p(property)))
                    {
                        continue;
                    }

                    PropertyMap propertyMap;
                    if (!string.IsNullOrEmpty(config.PropertyConfiguration.ColumnName))
                    {
                        propertyMap = new PropertyMap(property)
                        {
                            ColumnName = config.PropertyConfiguration.ColumnName
                        };
                    }
                    else if (!string.IsNullOrEmpty(config.PropertyConfiguration.Prefix))
                    {
                        propertyMap = new PropertyMap(property)
                        {
                            ColumnName = config.PropertyConfiguration.Prefix + property.Name
                        };
                    }
                    else if (config.PropertyConfiguration.PropertyTransformer != null)
                    {
                        propertyMap = new PropertyMap(property)
                        {
                            ColumnName = config.PropertyConfiguration.PropertyTransformer(property.Name)
                        };
                    }
                    else
                    {
                        continue;
                    }

                    entityMap.PropertyMaps.Add(propertyMap);
                }
            }
        }
    }
}
