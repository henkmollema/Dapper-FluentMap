using System.Reflection;
using Dapper.FluentMap.Mapping;

namespace Dapper.FluentMap.Dommel.Mapping
{
    public class DommelPropertyMap : PropertyMap
    {
        public DommelPropertyMap(PropertyInfo info)
            : base(info)
        {
        }

        internal bool Key { get; private set; }

        public DommelPropertyMap IsKey()
        {
            Key = true;
            return this;
        }
    }
}
