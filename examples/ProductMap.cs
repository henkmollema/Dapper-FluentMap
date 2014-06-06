using Dapper.FluentMap.Mapping;

namespace App.Data.Mapping
{
    /// <summary>
    /// Represents a manual mapping for the Product entity.
    /// </summary>
    public class ProductMap : EntityMap<Product>
    {
        public ProductMap()
        {
            Map(m => m.Id)
                .ToColumn("autID");

            Map(m => m.Name)
                .ToColumn("strName");

            Map(p => p.SellerAccountRID)
                .ToColumn("intSellerAccountRID");

            Map(p => p.NameUrlOptimized)
                .ToColumn("strNameUrlOptimized");
        }
    }
}
