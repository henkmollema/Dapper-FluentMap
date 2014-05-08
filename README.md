[Dapper.FluentMap](http://henkmollema.github.io/Dapper-FluentMap)
================

### Introduction
This [Dapper](https://github.com/SamSaffron/dapper-dot-net/) extension allows you to fluently congfigure the mapping between POCO properties and database columns. This keeps your POCO's clean of mapping attributes. The functionality is similar to [Entity Framework Fluent API](http://msdn.microsoft.com/nl-nl/data/jj591617.aspx).

<hr>

### Download
[![Download Dapper.FluentMap on NuGet](http://i.imgur.com/Rs483do.png "Download Dapper.FluentMap on NuGet")](https://www.nuget.org/packages/Dapper.FluentMap)

<hr>

### Usage
Mapping properties:
```
public class ProductMap : EntityMap<Product>
{
	public ProductMap()
	{
		Map(p => p.Name)
			.ToColumn("strName");
			
		Map(p => p.LastModified)
			.Ignore();
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

That's it. When querying the database using Dapper, the product name from column `strName` will be mapped to the `Name` property of the `Product` entity. `LastModifed` won't be mapped since we marked it as 'ignored'.
