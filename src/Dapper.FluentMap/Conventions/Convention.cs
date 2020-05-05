using System;
using System.Collections.Generic;
using System.ComponentModel;
using Dapper.FluentMap.Mapping;

namespace Dapper.FluentMap.Conventions
{
    /// <summary>
    /// Represents a convention for mapping entity properties to column names.
    /// </summary>
    public abstract class Convention
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Dapper.FluentMap.Conventions.Convention"/> class.
        /// </summary>
        protected Convention()
        {
            ConventionConfigurations = new List<PropertyConventionConfiguration>();
            PropertyMaps = new List<PropertyMap>();
        }

        /// <summary>
        /// Gets the convention configurations for the properties.
        /// </summary>
        public IList<PropertyConventionConfiguration> ConventionConfigurations { get; }

        /// <summary>
        /// Gets the property mappings.
        /// </summary>
        public IList<PropertyMap> PropertyMaps { get; }

        /// <summary>
        /// Configures a convention that applies on all properties of the entity.
        /// </summary>
        /// <returns>A configuration object for the convention.</returns>
        protected PropertyConventionConfiguration Properties()
        {
            var config = new PropertyConventionConfiguration();
            ConventionConfigurations.Add(config);

            return config;
        }

        /// <summary>
        /// Configures a convention that applies on all the properties of a specified type of the entity.
        /// </summary>
        /// <typeparam name="T">The type of the properties that the convention will apply to.</typeparam>
        /// <returns>A configuration object for the convention.</returns>
        protected PropertyConventionConfiguration Properties<T>()
        {
            var type = typeof(T);
            PropertyConventionConfiguration config;
            if (Nullable.GetUnderlyingType(type) != null)
            {
                // Convention defined for nullable type, match nullable properties
                config = new PropertyConventionConfiguration().Where(p => p.PropertyType == type);
            }
            else
            {
                // Convention defined for non-nullable types, match both nullabel and non-nullable properties
                config = new PropertyConventionConfiguration().Where(p => (Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType) == type);
            }
            ConventionConfigurations.Add(config);

            return config;
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
