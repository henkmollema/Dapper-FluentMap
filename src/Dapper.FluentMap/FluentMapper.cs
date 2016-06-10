using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Dapper.FluentMap.Configuration;
using Dapper.FluentMap.Conventions;
using Dapper.FluentMap.Mapping;
using Dapper.FluentMap.TypeMaps;

namespace Dapper.FluentMap
{
    /// <summary>
    /// Main entry point for Dapper.FluentMap configuration.
    /// </summary>
    public static class FluentMapper
    {
        private static readonly FluentMapConfiguration _configuration = new FluentMapConfiguration();

        /// <summary>
        /// Gets the dictionary containing the entity mapping per entity type.
        /// </summary>
        public static readonly ConcurrentDictionary<Type, IEntityMap> EntityMaps = new ConcurrentDictionary<Type, IEntityMap>();

        /// <summary>
        /// Gets the dictionary containing the conventions per entity type.
        /// </summary>
        public static readonly ConcurrentDictionary<Type, IList<Convention>> TypeConventions = new ConcurrentDictionary<Type, IList<Convention>>();

        /// <summary>
        /// Initializes Dapper.FluentMap with the specified configuration.
        /// This is method should be called when the application starts or when the first mapping is needed.
        /// </summary>
        /// <param name="configure">A callback containing the configuration of Dapper.FluentMap.</param>
        public static void Initialize(Action<FluentMapConfiguration> configure)
        {
            configure(_configuration);
        }

        /// <summary>
        /// Registers a Dapper type map using fluent mapping for the specified <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        internal static void AddTypeMap<TEntity>()
        {
            SqlMapper.SetTypeMap(typeof(TEntity), new FluentMapTypeMap<TEntity>());
        }

        /// <summary>
        /// Registers a Dapper type map using fluent mapping for the specified <paramref name="entityType"/>.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        internal static void AddTypeMap(Type entityType)
        {
            var instance = (SqlMapper.ITypeMap)Activator.CreateInstance(typeof(FluentMapTypeMap<>).MakeGenericType(entityType));
            SqlMapper.SetTypeMap(entityType, instance);
        }

        /// <summary>
        /// Registers a Dapper type map using conventions for the specified <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        internal static void AddConventionTypeMap<TEntity>()
        {
            SqlMapper.SetTypeMap(typeof(TEntity), new FluentConventionTypeMap<TEntity>());
        }

        /// <summary>
        /// Registers a Dapper type map using conventions for the specified <paramref name="entityType"/>.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        internal static void AddConventionTypeMap(Type entityType)
        {
            var instance = (SqlMapper.ITypeMap)Activator.CreateInstance(typeof(FluentConventionTypeMap<>).MakeGenericType(entityType));
            SqlMapper.SetTypeMap(entityType, instance);
        }
    }
}
