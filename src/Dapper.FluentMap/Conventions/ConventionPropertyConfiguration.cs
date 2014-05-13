using System;
using System.ComponentModel;

namespace Dapper.FluentMap.Conventions
{
    /// <summary>
    /// Represents configuration of a property via conventions.
    /// </summary>
    public class ConventionPropertyConfiguration
    {
        internal string ColumnName { get; private set; }

        internal string Prefix { get; private set; }

        /// <summary>
        /// Configures the name of the database column used to store the property.
        /// </summary>
        /// <param name="columnName">The name of the database column.</param>
        /// <returns>The same instance of <see cref="T:Dapper.FluentMap.Conventions.ConventionPropertyConfiguration"/>.</returns>
        public ConventionPropertyConfiguration HasColumnName(string columnName)
        {
            ColumnName = columnName;
            return this;
        }

        /// <summary>
        /// Configures the prefix of the database column used to store the property.
        /// </summary>
        /// <param name="prefix">The prefix of the database column.</param>
        /// <returns>The same instance of <see cref="T:Dapper.FluentMap.Conventions.ConventionPropertyConfiguration"/>.</returns>
        public ConventionPropertyConfiguration HasPrefix(string prefix)
        {
            Prefix = prefix;
            return this;
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
