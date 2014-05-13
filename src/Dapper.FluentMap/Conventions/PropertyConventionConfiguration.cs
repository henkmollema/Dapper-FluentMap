using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Dapper.FluentMap.Conventions
{
    public class PropertyConventionConfiguration
    {
        public PropertyConventionConfiguration()
        {
            PropertyPredicates = new List<Func<PropertyInfo, bool>>();
        }

        internal IList<Func<PropertyInfo, bool>> PropertyPredicates { get; private set; }

        internal ConventionPropertyConfiguration Config { get; private set; }

        public PropertyConventionConfiguration Where(Func<PropertyInfo, bool> predicate)
        {
            PropertyPredicates.Add(predicate);
            return this;
        }

        public void Configure(Action<ConventionPropertyConfiguration> configure)
        {
            var config = new ConventionPropertyConfiguration();
            Config = config;
            configure(config);
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
