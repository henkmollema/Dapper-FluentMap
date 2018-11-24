using System.Diagnostics;
using System.Reflection;

namespace Dapper.FluentMap.Mapping
{
    /// <summary>
    /// Specifies the mapping between a property and a column name.
    /// </summary>
    [DebuggerDisplay("{GetDebuggerDisplay(), nq}")]
    public class PropertyMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMap"/> class.
        /// </summary>
        /// <param name="propertyInfo">The property to apply the mapping to.</param>
        public PropertyMap(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }

        /// <summary>
        /// Gets the property to apply the mapping to.
        /// </summary>
        public PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Gets the column name to map the property to.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool Ignored { get; set; }

        /// <summary>
        /// Specifies the column name for this property.
        /// </summary>
        /// <param name="columnName">The name of the column in the underlying data store.</param>
        /// <returns>This <see cref="PropertyMap"/> instance.</returns>
        public PropertyMap ToColumn(string columnName)
        {
            ColumnName = columnName;
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public PropertyMap Ignore()
        {
            Ignored = true;
            return this;
        }

        private string GetDebuggerDisplay() => $"{PropertyInfo.Name} -> {ColumnName}";
    }
}
