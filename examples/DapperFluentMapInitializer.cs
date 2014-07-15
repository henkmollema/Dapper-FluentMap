using Dapper.FluentMap;

namespace App
{
    public class DapperFluentMapInitializer
    {
        public static void Init()
        {
            FluentMapper.Intialize(config =>
                                   {
                                       // Configure entities explicitly.
                                       config.AddConvention<TypePrefixConvention>()
                                             .ForEntity<Product>()
                                             .ForEntity<Order>;

                                       // Configure all entities in a certain assembly with an optional namespaces filter.
                                       config.AddConvention<TypePrefixConvention>()
                                             .ForEntitiesInAssembly(typeof(Product).Assembly, "App.Domain.Model");

                                       // Configure all entities in the current assembly with an optional namespaces filter.
                                       config.AddConvention<TypePrefixConvention>()
                                             .ForEntitiesInCurrentAssembly("App.Domain.Model.Catalog", "App.Domain.Model.Order");
                                   });
        }
    }
}
