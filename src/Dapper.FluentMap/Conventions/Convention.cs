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

        internal IList<PropertyConventionConfiguration> ConventionConfigurations { get; }

        internal IList<PropertyMap> PropertyMaps { get; }

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
            // Get the underlying type for a nullale type. (int? -> int)
            var underlyingType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            var config = new PropertyConventionConfiguration().Where(p => p.PropertyType == underlyingType);
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
