using System;
using System.ComponentModel;
using System.Reflection;

namespace Dapper.FluentMap.Mapping
{
    /// <summary>
    /// Represents the mapping of a property.
    /// </summary>
    public class PropertyMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Dapper.FluentMap.Mapping.PropertyMap"/> using 
        /// the specified <see cref="T:System.Reflection.PropertyInfo"/> object representing the property to map.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Reflection.PropertyInfo"/> object representing to the property to map.</param>
        public PropertyMap(PropertyInfo info)
        {
            PropertyInfo = info;
            ColumnName = info.Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Dapper.FluentMap.Mapping.PropertyMap"/> using 
        /// the specified <see cref="T:System.Reflection.PropertyInfo"/> object representing the property to map 
        /// and column name to map the property to.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Reflection.PropertyInfo"/> object representing to the property to map.</param>
        /// <param name="columnName">The column name in the database to map the property to.</param>
        internal PropertyMap(PropertyInfo info, string columnName)
        {
            PropertyInfo = info;
            ColumnName = columnName;
        }

        /// <summary>
        /// Gets the name of the column in the data store.
        /// </summary>
        public string ColumnName { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether column name mapping should be case sensitive.
        /// </summary>
        internal bool CaseSensitive { get; private set; }

        /// <summary>
        /// Gets a value indicating wether the property should be ignored when mapping.
        /// </summary>
        internal bool Ignored { get; private set; }

        /// <summary>
        /// Gets the <see cref="T:System.Reflection.PropertyInfo"/> object for the current property.
        /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }

        /// <summary>
        /// Maps the current property to the specified column name.
        /// </summary>
        /// <param name="columnName">The name of the column in the data store.</param>
        /// <param name="caseSensitive">A value indicating whether column name mapping should be case sensitive.</param>
        /// <returns>The current <see cref="T:Dapper.FluentMap.Mapping.PropertyMap"/> instance. This enables a fluent API.</returns>
        public PropertyMap ToColumn(string columnName, bool caseSensitive = true)
        {
            ColumnName = columnName;
            CaseSensitive = caseSensitive;
            return this;
        }

        /// <summary>
        /// Marks the current property as ignored, resulting in the property not being mapped by Dapper.
        /// </summary>
        /// <returns>The current <see cref="T:Dapper.FluentMap.Mapping.PropertyMap"/> instance. This enables a fluent API.</returns>
        public PropertyMap Ignore()
        {
            Ignored = true;
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
