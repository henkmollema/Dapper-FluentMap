using System;
using System.Collections.Generic;
using Dapper.FluentMap.Mapping;

namespace Dapper.FluentMap.Conventions
{
    /// <summary>
    /// Represents a convention for mapping entity properties to column names.
    /// </summary>
    public abstract class Convention
    {
        protected Convention()
        {
            ConventionConfigurations = new List<PropertyConventionConfiguration>();
            PropertyMaps = new List<PropertyMap>();
        }

        internal IList<PropertyConventionConfiguration> ConventionConfigurations { get; private set; }

        internal IList<PropertyMap> PropertyMaps { get; private set; }

        protected PropertyConventionConfiguration Properties()
        {
            var config = new PropertyConventionConfiguration();
            ConventionConfigurations.Add(config);

            return config;
        }

        protected PropertyConventionConfiguration Properties<T>()
        {
            Type underlyingType = Nullable.GetUnderlyingType(typeof (T)) ?? typeof (T);
            var config = new PropertyConventionConfiguration().Where(p => p.PropertyType == underlyingType);
            ConventionConfigurations.Add(config);

            return config;
        }
    }
}
