using System.ComponentModel.DataAnnotations.Schema;
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
        /// Initializes a new instance of the <see cref="DommelPropertyMap"/> class
        /// with the specified <see cref="PropertyInfo"/> object.
        /// </summary>
        /// <param name="info">The information about the property.</param>
        public DommelPropertyMap(PropertyInfo info) : base(info)
        {
        }

        /// <summary>
        /// Gets a value indicating whether this property is a primary key.
        /// </summary>
        public bool Key { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this primary key is an identity.
        /// </summary>
        public bool Identity { get; set; }

        /// <summary>
        /// Gets a value indicating how the column is generated.
        /// </summary>
        public DatabaseGeneratedOption? GeneratedOption { get; set; }

        /// <summary>
        /// Specifies the current property as key for the entity.
        /// </summary>
        /// <returns>The current instance of <see cref="DommelPropertyMap"/>.</returns>
        public DommelPropertyMap IsKey()
        {
            Key = true;
            return this;
        }

        /// <summary>
        /// Specifies the current property as an identity.
        /// </summary>
        /// <returns>The current instance of <see cref="DommelPropertyMap"/>.</returns>
        public DommelPropertyMap IsIdentity()
        {
            Identity = true;
            return this;
        }

        /// <summary>
        /// Specifies how the property is generated.
        /// </summary>
        public DommelPropertyMap SetGeneratedOption(DatabaseGeneratedOption option)
        {
            GeneratedOption = option;
            return this;
        }
    }
}
