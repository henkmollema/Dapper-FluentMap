using System;
using System.Collections.Generic;
using System.Reflection;

namespace Dapper.FluentMap.Conventions
{
    /// <summary>
    /// Represents the configuration for a convention.
    /// </summary>
    public class ConventionConfiguration
    {
        private readonly List<Func<PropertyInfo, bool>> _propertyPredicates = new List<Func<PropertyInfo, bool>>();

        /// <summary>
        /// Gets the collection of predicates to apply to the properties.
        /// </summary>
        public IReadOnlyCollection<Func<PropertyInfo, bool>> PropertyPredicates => _propertyPredicates;

        /// <summary>
        /// Gets the property configuration.
        /// </summary>
        public PropertyConfiguration PropertyConfiguration { get; private set; }

        /// <summary>
        /// Filters the properties that this convention applies to based on a predicate.
        /// </summary>
        /// <param name="predicate">A function to test each property for a condition.</param>
        /// <returns>The same instance of <see cref="ConventionConfiguration"/>.</returns>
        public ConventionConfiguration Where(Func<PropertyInfo, bool> predicate)
        {
            _propertyPredicates.Add(predicate);
            return this;
        }

        /// <summary>
        /// Configures the properties that this convention applies to.
        /// </summary>
        /// <param name="configure">
        /// An action that performs configuration against <see cref="Conventions.PropertyConfiguration"/>.
        /// </param>
        public void Configure(Action<PropertyConfiguration> configure)
        {
            var config = new PropertyConfiguration();
            PropertyConfiguration = config;
            configure(config);
        }
    }
}
