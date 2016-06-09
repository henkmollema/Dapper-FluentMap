using System.Reflection;
using Dapper.FluentMap.Mapping;

namespace Dapper.FluentMap.Dommel.Mapping
{
    /// <summary>
    /// Represents mapping of a property for Dommel.
    /// </summary>
    public class DommelPropertyMap : PropertyMapBase<DommelPropertyMap>, IPropertyMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Dapper.FluentMap.Dommel.Mapping.DommelPropertyMap"/> class
        /// with the specified <see cref="System.Reflection.PropertyInfo"/> object.
        /// </summary>
        /// <param name="info">The information about the property.</param>
        public DommelPropertyMap(PropertyInfo info)
            : base(info)
        {
        }

        public bool Key { get; private set; }

        /// <summary>
        /// Marks the current property as key for the entity.
        /// </summary>
        /// <returns>The current instance of <see cref="T:Dapper.FluentMap.Dommel.Mapping.DommelPropertyMap"/>.</returns>
        public DommelPropertyMap IsKey()
        {
            Key = true;
            return this;
        }
    }
}
