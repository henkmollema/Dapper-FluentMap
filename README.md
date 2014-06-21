[Dapper.FluentMap](http://henkmollema.github.io/Dapper-FluentMap)
================

[![Build status](https://ci.appveyor.com/api/projects/status/x6grw3cjuyud9c76)](https://ci.appveyor.com/project/HenkMollema/dapper-fluentmap)
### Introduction

This [Dapper](https://github.com/SamSaffron/dapper-dot-net/) extension allows you to fluently congfigure the mapping between POCO properties and database columns. This keeps your POCO's clean of mapping attributes. The functionality is similar to [Entity Framework Fluent API](http://msdn.microsoft.com/nl-nl/data/jj591617.aspx). If you have any questions, suggestions or bugs, please don't hesitate to [contact me](mailto:henkmollema@gmail.com) or create an issue.

<hr>

### Download
[![Download Dapper.FluentMap on NuGet](http://i.imgur.com/Rs483do.png "Download Dapper.FluentMap on NuGet")](https://www.nuget.org/packages/Dapper.FluentMap)

<hr>

### Usage
#### Manually mapping properties
You can map property names manually using the [`EntityMap<TEntity>`](https://github.com/HenkMollema/Dapper-FluentMap/blob/master/src/Dapper.FluentMap/Mapping/EntityMap.cs) class. When creating a derived class, the constructor gives you access to the `Map` method, allowing you to specify to which database column name a certain property of `TEntity` should map to.
```csharp
public class ProductMap : EntityMap<Product>
{
	public ProductMap()
	{
		// Map property 'Name' to column 'strName'.
		Map(p => p.Name)
			.ToColumn("strName");
			
		// Ignore the 'LastModified' property when mapping.
		Map(p => p.LastModified)
			.Ignore();
	}
}
```

Column names are mapped case sensitive by default. You can change this by specifying the `caseSensitive` parameter in the `ToColumn()` method: `Map(p => p.Name).ToColumn("strName", caseSensitive: false)`.

**Initialization:**
```csharp
FluentMapper.Intialize(config =>
					   {
						   config.AddMap(new ProductMap());
					   });
```

#### Mapping properties using conventions

When you have a lot of entity types, creating manual mapping classes can become plumbing. If your column names adhere to some kind of naming convention, you might be better off by configuring a mapping convention.

You can create a convention by creating a class which derives from the [`Convention`](https://github.com/HenkMollema/Dapper-FluentMap/blob/master/src/Dapper.FluentMap/Conventions/Convention.cs) class. In the contructor you can configure the property conventions:
```csharp
public class TypePrefixConvention : Convention
{
    public TypePrefixConvention()
    {
        // Map all properties of type int and with the name 'id' to column 'autID'.
        Properties<int>()
            .Where(c => c.Name.ToLower() == "id")
            .Configure(c => c.HasColumnName("autID"));

        // Prefix all properties of type string with 'str' when mapping to column names.
        Properties<string>()
            .Configure(c => c.HasPrefix("str"));

        // Prefix all properties of type int with 'int' when mapping to column names.
        Properties<int>()
            .Configure(c => c.HasPrefix("int"));
    }
}
```

When initializing Dapper.FluentMap with conventions, the entities on which a convention applies must be configured. You can choose to either configure the entities explicitly or use assembly scanning.

```csharp
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
```

##### Transformations
The convention API allows you to configure transformation of property names to database column names. An implementation would look like this:
```csharp
public class PropertyTransformConvention : Convention
{
    public PropertyTransformConvention()
    {
        Properties()
            .Configure(c => c.Transform(s => Regex.Replace(input: s, pattern: "([A-Z])([A-Z][a-z])|([a-z0-9])([A-Z])", replacement: "$1$3_$2$4")));
    }
}
```

This configuration will map camel case property names to underscore seperated database column names (`UrlOptimizedName` -> `Url_Optimized_Name`).
