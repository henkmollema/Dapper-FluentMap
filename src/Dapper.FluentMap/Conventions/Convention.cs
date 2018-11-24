using System;
using System.Collections.Generic;

namespace Dapper.FluentMap.Conventions
{
    /// <summary>
    /// Represents a convention for mapping entity properties to column names.
    /// </summary>
    public abstract class Convention
    {
        private readonly List<ConventionConfiguration> _configurations = new List<ConventionConfiguration>();

        /// <summary>
        /// Gets the collection of convention configurations.
        /// </summary>
        public IReadOnlyCollection<ConventionConfiguration> ConventionConfigurations => _configurations.AsReadOnly();

        /// <summary>
        /// Configures a convention that applies on all properties of the entity.
        /// </summary>
        /// <returns>A configuration object for the convention.</returns>
        protected ConventionConfiguration Properties()
        {
            var config = new ConventionConfiguration();
            _configurations.Add(config);

            return config;
        }

        /// <summary>
        /// Configures a convention that applies on all the properties of a specified type of the entity.
        /// </summary>
        /// <typeparam name="T">The type of the properties that the convention will apply to.</typeparam>
        /// <returns>A configuration object for the convention.</returns>
        protected ConventionConfiguration Properties<T>()
        {
            // Get the underlying type for a nullale type. (int? -> int)
            var underlyingType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            var config = new ConventionConfiguration().Where(p => p.PropertyType == underlyingType);
            _configurations.Add(config);

            return config;
        }
    }
}
