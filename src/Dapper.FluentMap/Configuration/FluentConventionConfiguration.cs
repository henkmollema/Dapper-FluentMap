using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Dapper.FluentMap.Conventions;
using Dapper.FluentMap.Mapping;
using Dapper.FluentMap.Utils;

namespace Dapper.FluentMap.Configuration
{
    /// <summary>
    /// Defines methods for configuring conventions.
    /// </summary>
    public class FluentConventionConfiguration
    {
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
        /// Configures the current covention for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <returns>The current instance of <see cref="T:Dapper.FluentMap.Configuration.FluentConventionConfiguration"/>.</returns>
        public FluentConventionConfiguration ForEntity<T>()
        {
            var type = typeof(T);
            MapProperties(type);

            FluentMapper.TypeConventions.AddOrUpdate(type, _convention);
            FluentMapper.AddConventionTypeMap<T>();
            return this;
        }

#if !NETSTANDARD1_3
        /// <summary>
        /// Configures the current convention for all the entities in current assembly filtered by the specified namespaces.
        /// </summary>
        /// <param name="namespaces">
        /// An array of namespaces which filter the types in the current assembly.
        /// This parameter is optional.
        /// </param>
        /// <returns>The current instance of <see cref="T:Dapper.FluentMap.Configuration.FluentConventionConfiguration"/>.</returns>
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

                MapProperties(type);
                FluentMapper.TypeConventions.AddOrUpdate(type, _convention);
                FluentMapper.AddConventionTypeMap(type);
            }

            return this;
        }
#endif

        /// <summary>
        /// Configures the current convention for all entities in the specified assembly filtered by the specified namespaces.
        /// </summary>
        /// <param name="assembly">The assembly to scan for entities.</param>
        /// <param name="namespaces">
        /// An array of namespaces which filter the types in <paramref name="assembly"/>.
        /// This parameter is optional.
        /// </param>
        /// <returns>The current instance of <see cref="T:Dapper.FluentMap.Configuration.FluentConventionConfiguration"/>.</returns>
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

                MapProperties(type);
                FluentMapper.TypeConventions.AddOrUpdate(type, _convention);
                FluentMapper.AddConventionTypeMap(type);
            }

            return this;
        }

        private void MapProperties(Type type)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                // Find the convention configurations for the convetion with either none or matching property predicates.
                foreach (var config in _convention.ConventionConfigurations
                                                  .Where(c => c.PropertyPredicates.Count <= 0 ||
                                                              c.PropertyPredicates.All(e => e(property))))
                {
                    if (!string.IsNullOrEmpty(config.PropertyConfiguration.ColumnName))
                    {
                        AddConventionPropertyMap(
                            property,
                            config.PropertyConfiguration.ColumnName,
                            config.PropertyConfiguration.CaseSensitive);
                        break;
                    }

                    if (!string.IsNullOrEmpty(config.PropertyConfiguration.Prefix))
                    {
                        AddConventionPropertyMap(
                            property,
                            config.PropertyConfiguration.Prefix + property.Name,
                            config.PropertyConfiguration.CaseSensitive);
                        break;
                    }

                    if (config.PropertyConfiguration.PropertyTransformer != null)
                    {
                        AddConventionPropertyMap(
                            property,
                            config.PropertyConfiguration.PropertyTransformer(property.Name),
                            config.PropertyConfiguration.CaseSensitive);
                    }
                }
            }
        }

        private void AddConventionPropertyMap(PropertyInfo property, string columnName, bool caseSensitive)
        {
            var map = new PropertyMap(property, columnName, caseSensitive);
            _convention.PropertyMaps.Add(map);
        }

        #region EditorBrowsableStates
        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType()
        {
            return base.GetType();
        }
        #endregion
    }
}
