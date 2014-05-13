using System;
using System.Linq;
using System.Reflection;
using Dapper.FluentMap.Mapping;
using Dapper.FluentMap.Utils;

namespace Dapper.FluentMap.Conventions
{
    public class FluentMapConventionConfiguration
    {
        private readonly Convention _convention;

        public FluentMapConventionConfiguration(Convention convention)
        {
            _convention = convention;
        }

        /// <summary>
        /// Configures the current covention for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <returns>The current instance of <see cref="T:Dapper.FluentMap.Conventions.FluentMapConventionConfiguration"/>.</returns>
        public FluentMapConventionConfiguration ForEntity<T>()
        {
            Type type = typeof (T);
            MapProperties(type);

            FluentMapper.TypeConventions.AddOrUpdate(type, _convention);
            FluentMapper.AddConventionTypeMap<T>();
            return this;
        }

        /// <summary>
        /// Configures the current convention for all the entities in current assembly filtered by the specified namespaces.
        /// </summary>
        /// <param name="namespaces">An array of namespaces which filter the types in the current assembly. This parameter is optional.</param>
        /// <returns>The current instance of <see cref="T:Dapper.FluentMap.Conventions.FluentMapConventionConfiguration"/>.</returns>
        public FluentMapConventionConfiguration ForEntitiesInCurrentAssembly(params string[] namespaces)
        {
            foreach (var type in Assembly.GetCallingAssembly()
                                         .GetExportedTypes()
                                         .Where(type => namespaces.Length == 0 || namespaces.Any(n => type.Namespace == n)))
            {
                MapProperties(type);
                FluentMapper.TypeConventions.AddOrUpdate(type, _convention);
                FluentMapper.AddConventionTypeMap(type);
            }

            return this;
        }

        /// <summary>
        /// Configures the current convention for all entities in the specified assembly filtered by the specified namespaces.
        /// </summary>
        /// <param name="assembly">The assembly to scan for entities.</param>
        /// <param name="namespaces">An array of namespaces which filter the types in <paramref name="assembly"/>. This parameter is optional.</param>
        /// <returns>The current instance of <see cref="T:Dapper.FluentMap.Conventions.FluentMapConventionConfiguration"/>.</returns>
        public FluentMapConventionConfiguration ForEntitiesInAssembly(Assembly assembly, params string[] namespaces)
        {
            foreach (var type in assembly.GetExportedTypes().Where(t => namespaces.Any(n => n.Contains(t.Namespace))))
            {
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
                    if (!string.IsNullOrEmpty(config.Config.ColumnName))
                    {
                        AddConventionPropertyMap(property, config.Config.ColumnName);
                        break;
                    }

                    if (!string.IsNullOrEmpty(config.Config.Prefix))
                    {
                        AddConventionPropertyMap(property, config.Config.Prefix + property.Name);
                        break;
                    }
                }
            }
        }

        private void AddConventionPropertyMap(PropertyInfo property, string columnName)
        {
            var map = new PropertyMap(property, columnName);
            _convention.PropertyMaps.Add(map);
        }
    }
}
