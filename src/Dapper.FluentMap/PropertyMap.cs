using System.Reflection;

namespace Dapper.FluentMap
{
    public class PropertyMap
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

        /// <summary>
        /// Gets the name of the column in the data store.
        /// </summary>
        internal string ColumnName { get; private set; }

        /// <summary>
        /// Gets a value indicating wether the property should be ignored when mapping.
        /// </summary>
        internal bool Ignored { get; private set; }

        /// <summary>
        /// Gets the <see cref="T:System.Reflecion.PropertyInfo"/> object for the current property.
        /// </summary>
        internal PropertyInfo PropertyInfo { get; private set; }

        /// <summary>
        /// Maps the current property to the specified column name.
        /// </summary>
        /// <param name="columnName">The name of the column in the data store.</param>
        /// <returns>The current <see cref="T:Dapper.FluentMap.PropertyMap"/> instance. This enables a fluent API.</returns>
        public PropertyMap ToColumn(string columnName)
        {
            ColumnName = columnName;
            return this;
        }

        /// <summary>
        /// Marks the current property as ignored, resulting in the property not being mapped by Dapper.
        /// </summary>
        /// <returns>The current <see cref="T:Dapper.FluentMap.PropertyMap"/> instance. This enables a fluent API.</returns>
        public PropertyMap Ignore()
        {
            Ignored = true;
            return this;
        }
    }
}
