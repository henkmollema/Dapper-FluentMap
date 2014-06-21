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
			// Map property 'Name' to column 'strName'.
			Map(p => p.Name)
				.ToColumn("strName");
	
			// Map property 'Description' to 'strdescription', ignoring casing.
			Map(p => p.Description)
				.ToColumn("strdescription", caseSensitive: false);
	
			// Ignore the 'LastModified' property when mapping.
			Map(p => p.LastModified)
				.Ignore();
		}
	}
}
