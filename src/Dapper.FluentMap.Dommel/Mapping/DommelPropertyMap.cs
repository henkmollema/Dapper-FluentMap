using System.Reflection;
using Dapper.FluentMap.Mapping;

namespace Dapper.FluentMap.Dommel.Mapping
{
    /// <summary>
    /// A Dommel <see cref="PropertyMap"/>.
    /// </summary>
    public class DommelPropertyMap : PropertyMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DommelPropertyMap"/> class.
        /// </summary>
        /// <param name="propertyInfo">The property info to apply the mapping to.</param>
        public DommelPropertyMap(PropertyInfo propertyInfo) : base(propertyInfo)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether the current property is considered a key property.
        /// </summary>
        public bool Key { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current property is considered a identity property.
        /// </summary>
        public bool Identity { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the current property is considered a computed property.
        /// </summary>
        public bool Computed { get; set; }

        /// <summary>
        /// Marks the property as a key property.
        /// </summary>
        public DommelPropertyMap IsKey()
        {
            Key = true;
            return this;
        }
        
        /// <summary>
        /// Marks the property as a key property.
        /// </summary>
        public DommelPropertyMap IsIdentity()
        {
            Identity = true;
            return this;
        }
        
        /// <summary>
        /// Marks the property as a computed property.
        /// </summary>
        public DommelPropertyMap IsComputed()
        {
            Computed = true;
            return this;
        }
    }

    /// <summary>
    /// Dommel extensions for <see cref="PropertyMap"/>.
    /// </summary>
    public static class PropertyMapExtensions
    {
        /// <summary>
        /// Marks the property as a key property.
        /// </summary>
        public static PropertyMap IsKey(this PropertyMap propertyMapping)
        {
            if (propertyMapping is DommelPropertyMap dommelPropertyMapping)
            {
                dommelPropertyMapping.IsKey();
            }

            return propertyMapping;
        }
        
        /// <summary>
        /// Marks the property as a identity property.
        /// </summary>
        public static PropertyMap IsIdentity(this PropertyMap propertyMapping)
        {
            if (propertyMapping is DommelPropertyMap dommelPropertyMapping)
            {
                dommelPropertyMapping.IsIdentity();
            }

            return propertyMapping;
        }
        
        /// <summary>
        /// Marks the property as a computed property.
        /// </summary>
        public static PropertyMap IsComputed(this PropertyMap propertyMapping)
        {
            if (propertyMapping is DommelPropertyMap dommelPropertyMapping)
            {
                dommelPropertyMapping.IsComputed();
            }

            return propertyMapping;
        }
    }
}
