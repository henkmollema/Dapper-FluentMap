using System;

namespace Dapper.FluentMap.Conventions
{
    /// <summary>
    /// Represents configuration of a property via conventions.
    /// </summary>
    public class PropertyConfiguration
    {
        /// <summary>
        /// Initializes a new instane of the <see cref="PropertyConfiguration"/>.
        /// </summary>
        public PropertyConfiguration()
        {
            CaseSensitive = true;
        }

        /// <summary>
        /// Configures the name of the database column used to store the property.
        /// </summary>
        /// <param name="columnName">The name of the database column.</param>
        /// <returns>The same instance of <see cref="PropertyConfiguration"/>.</returns>
        public PropertyConfiguration HasColumnName(string columnName)
        {
            ColumnName = columnName;
            return this;
        }

        /// <summary>
        /// Configures the prefix of the database column used to store the property.
        /// </summary>
        /// <param name="prefix">The prefix of the database column.</param>
        /// <returns>The same instance of <see cref="PropertyConfiguration"/>.</returns>
        public PropertyConfiguration HasPrefix(string prefix)
        {
            Prefix = prefix;
            return this;
        }

        /// <summary>
        /// Configures the current convention to be case insensitive.
        /// </summary>
        /// <returns>The same instance of <see cref="PropertyConfiguration"/>.</returns>
        public PropertyConfiguration IsCaseInsensitive()
        {
            CaseSensitive = false;
            return this;
        }

        /// <summary>
        /// Configures the function for transforming property names to database column names.
        /// </summary>
        /// <param name="transformer">A function which takes the property name and returns the database colum name.</param>
        /// <returns>The same instance of <see cref="PropertyConfiguration"/>.</returns>
        public PropertyConfiguration Transform(Func<string, string> transformer)
        {
            PropertyTransformer = transformer;
            return this;
        }

        internal string ColumnName { get; private set; }

        internal string Prefix { get; private set; }

        internal bool CaseSensitive { get; private set; }

        internal Func<string, string> PropertyTransformer { get; private set; }
    }
}
