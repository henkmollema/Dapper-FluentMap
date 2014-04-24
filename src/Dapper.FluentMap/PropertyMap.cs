using System.Reflection;

namespace Dapper.FluentMap
{
    /// <summary>
    /// Provides a mapper for a specific entity.
    /// </summary>
    public interface IPropertyMap
    {
        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the name of the column in the data store.
        /// </summary>
        string ColumnName { get; }

        /// <summary>
        /// Gets a value indicating wether the property should be ignored when mapping.
        /// </summary>
        bool Ignored { get; }

        /// <summary>
        /// Gets the <see cref="T:System.Reflecion.PropertyInfo"/> object for the current property.
        /// </summary>
        PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Maps the current property to the specified column name.
        /// </summary>
        /// <param name="columnName">The name of the column in the data store.</param>
        /// <returns>The current <see cref="T:Dapper.FluentMap.IPropertyMap"/> instance. This enables a fluent API.</returns>
        IPropertyMap ToColumn(string columnName);

        /// <summary>
        /// Marks the current property as ignored, resulting in the property not being mapped by Dapper.
        /// </summary>
        /// <returns>The current <see cref="T:Dapper.FluentMap.PropertyMap"/> instance. This enables a fluent API.</returns>
        IPropertyMap Ignore();
    }

    public class PropertyMap : IPropertyMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Dapper.FluentMap.PropertyMap"/> using 
        /// the specified <see cref="T:System.Reflecion.PropertyInfo"/> object representing the property to map.
        /// </summary>
        /// <param name="info"></param>
        public PropertyMap(PropertyInfo info)
        {
            PropertyInfo = info;
            ColumnName = info.Name;
        }

        public string Name
        {
            get
            {
                return PropertyInfo.Name;
            }
        }

        public string ColumnName { get; private set; }

        public bool Ignored { get; private set; }

        public bool IsReadOnly { get; private set; }

        public PropertyInfo PropertyInfo { get; private set; }

        public IPropertyMap ToColumn(string columnName)
        {
            ColumnName = columnName;
            return this;
        }

        public IPropertyMap Ignore()
        {
            Ignored = true;
            return this;
        }
    }
}
