using Dapper.FluentMap.Configuration;
using Dapper.FluentMap.Dommel.Resolvers;

using DommelMapper = Dommel.Dommel;

namespace Dapper.FluentMap.Dommel
{
    public static class FluentMapConfigurationExtensions
    {
        public static FluentMapConfiguration ForDommel(this FluentMapConfiguration config)
        {
            DommelMapper.SetColumnNameResolver(new DommelColumnNameResolver());
            DommelMapper.SetKeyPropertyResolver(new DommelKeyPropertyResolver());
            DommelMapper.SetTableNameResolver(new DommelTableNameResolver());
            return config;
        }
    }
}
