Dapper-FluentMap
================

This API allows you to fluently map your POCO properties to database columns when using [Dapper](https://github.com/SamSaffron/dapper-dot-net/).

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
