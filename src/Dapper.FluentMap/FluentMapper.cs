using System;
using System.Collections.Generic;

namespace Dapper.FluentMap
{
    /// <summary>
    /// Main entry point for Dapper.FluentMap for initialization.
    /// </summary>
    public static class FluentMapper
    {
        internal static readonly IList<dynamic> Mappers = new List<dynamic>();

        /// <summary>
        /// Initializes Dapper.FluentMap with the specified configuration. 
        /// This is method should be called when the application starts or when the first mapping is needed.
        /// </summary>
        /// <param name="configure">A callback containing the configuration of Dapper.FluentMap.</param>
        public static void Intialize(Action<IFluentMapConfiguration> configure)
        {
            configure(new FluentMapConfiguration());
        }

        /// <summary>
        /// Registers a Dapper type map for the specified <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        internal static void AddTypeMap<TEntity>()
        {
            SqlMapper.SetTypeMap(typeof (TEntity), new FluentMapTypeMapper<TEntity>());
        }

        /// <summary>
        /// Registers a Dapper type map for the specified <paramref name="entityType"/>.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        internal static void AddTypeMap(Type entityType)
        {
            var instance = (SqlMapper.ITypeMap)Activator.CreateInstance(typeof (FluentMapTypeMapper<>).MakeGenericType(entityType));
            SqlMapper.SetTypeMap(entityType, instance);
        }
    }
}
