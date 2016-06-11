using System;
using System.ComponentModel;
using System.Reflection;

namespace Dapper.FluentMap.Mapping
{
    /// <summary>
    /// Represents the mapping of a property.
    /// </summary>
    public interface IPropertyMap
    {
        /// <summary>
        /// Gets the name of the column in the data store.
        /// </summary>
        string ColumnName { get; }

        /// <summary>
        /// Gets the <see cref="T:System.Reflection.PropertyInfo"/> object for the current property.
        /// </summary>
        PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Gets or sets a value indicating whether column name mapping should be case sensitive.
        /// </summary>
        bool CaseSensitive { get; }

        /// <summary>
        /// Gets a value indicating wether the property should be ignored when mapping.
        /// </summary>
        bool Ignored { get; }
    }

    /// <summary>
    /// Serves as the base class for all property mapping implementations.
    /// </summary>
    /// <typeparam name="TPropertyMap">The type of the property mapping.</typeparam>
    public abstract class PropertyMapBase<TPropertyMap>
        where TPropertyMap : class, IPropertyMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Dapper.FluentMap.Mapping.PropertyMap"/> using
        /// the specified <see cref="T:System.Reflection.PropertyInfo"/> object representing the property to map.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Reflection.PropertyInfo"/> object representing to the property to map.</param>
        protected PropertyMapBase(PropertyInfo info)
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
        internal PropertyMapBase(PropertyInfo info, string columnName)
        {
            PropertyInfo = info;
            ColumnName = columnName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Dapper.FluentMap.Mapping.PropertyMap"/> using
        /// the specified <see cref="T:System.Reflection.PropertyInfo"/> object representing the property to map,
        /// column name to map the property to and a value indicating whether the mapping should be case sensitive.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Reflection.PropertyInfo"/> object representing to the property to map.</param>
        /// <param name="columnName">The column name in the database to map the property to.</param>
        /// <param name="caseSensitive">A value indicating whether the mappig should be case sensitive.</param>
        internal PropertyMapBase(PropertyInfo info, string columnName, bool caseSensitive)
        {
            PropertyInfo = info;
            ColumnName = columnName;
            CaseSensitive = caseSensitive;
        }

        /// <summary>
        /// Gets the column name for the mapping.
        /// </summary>
        public string ColumnName { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this mapping is case sensitive.
        /// </summary>
        public bool CaseSensitive { get; private set; }

        /// <summary>
        /// Gets a value indicating the property should be ignored when mapping.
        /// </summary>
        public bool Ignored { get; private set; }

        /// <summary>
        /// Gets a reference to the <see cref="System.Reflection.PropertyInfo"/> of this mapping.
        /// </summary>
        public PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Maps the current property to the specified column name.
        /// </summary>
        /// <param name="columnName">The name of the column in the data store.</param>
        /// <param name="caseSensitive">A value indicating whether column name mapping should be case sensitive.</param>
        /// <returns>The current instance of <typeparamref name="TPropertyMap"/>.</returns>
        public TPropertyMap ToColumn(string columnName, bool caseSensitive = true)
        {
            ColumnName = columnName;
            CaseSensitive = caseSensitive;
            return this as TPropertyMap;
        }

        /// <summary>
        /// Marks the current property as ignored, resulting in the property not being mapped by Dapper.
        /// </summary>
        /// <returns>The current <see cref="T:Dapper.FluentMap.Mapping.PropertyMap"/> instance. This enables a fluent API.</returns>
        public TPropertyMap Ignore()
        {
            Ignored = true;
            return this as TPropertyMap;
        }

        #region EditorBrowsableStates
        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType()
        {
            return base.GetType();
        }
        #endregion
    }

    /// <summary>
    /// Represents the mapping of a property.
    /// </summary>
    public class PropertyMap : PropertyMapBase<PropertyMap>, IPropertyMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Dapper.FluentMap.Mapping.PropertyMap"/> class
        /// with the specified <see cref="System.Reflection.PropertyInfo"/> object.
        /// </summary>
        /// <param name="info">The information about the property.</param>
        public PropertyMap(PropertyInfo info)
            : base(info)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dapper.FluentMap.Mapping.PropertyMap"/> class
        /// with the specified <see cref="System.Reflection.PropertyInfo"/> object and column name.
        /// </summary>
        /// <param name="info">The information about the property.</param>
        /// <param name="columnName">The column name.</param>
        public PropertyMap(PropertyInfo info, string columnName)
            : base(info, columnName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dapper.FluentMap.Mapping.PropertyMap"/> class
        /// with the specified <see cref="System.Reflection.PropertyInfo"/> object, column name
        /// and a value indicating whether the mapping should be case sensitive.
        /// </summary>
        /// <param name="info">The information about the property.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="caseSensitive">A value indicating whether the mappig should be case sensitive.</param>
        public PropertyMap(PropertyInfo info, string columnName, bool caseSensitive)
            : base(info, columnName, caseSensitive)
        {
        }
    }
}
