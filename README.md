Dapper-FluentMap
================

This API allows you to fluently map your POCO properties to database columns when using [Dapper](https://github.com/SamSaffron/dapper-dot-net/). This allows you to keep your POCO's clean of (Dapper specific) mapping attributes. The functionality is similar to [Entity Framework Fluent API](http://msdn.microsoft.com/nl-nl/data/jj591617.aspx).

Usage
========
Mapping entity properties:
```
public class ProductMap : EntityMap<Product>
{
	public ProductMap()
	{
		Map(p => p.Name)
			.ToColumn("strName");
	}
}
```
    
Initializing Dapper.FluentMap:

```
FluentMapper.Intialize(config =>
					   {
						   config.AddEntityMap(new ProductMap());
					   });
```

That's it. When quering the database using Dapper, the product name from column `strName` will be mapped to the `Name` property of the `Product` entity.
