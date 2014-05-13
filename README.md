[Dapper.FluentMap](http://henkmollema.github.io/Dapper-FluentMap)
================

### Introduction
This [Dapper](https://github.com/SamSaffron/dapper-dot-net/) extension allows you to fluently congfigure the mapping between POCO properties and database columns. This keeps your POCO's clean of mapping attributes. The functionality is similar to [Entity Framework Fluent API](http://msdn.microsoft.com/nl-nl/data/jj591617.aspx).

<hr>

### Download
[![Download Dapper.FluentMap on NuGet](http://i.imgur.com/Rs483do.png "Download Dapper.FluentMap on NuGet")](https://www.nuget.org/packages/Dapper.FluentMap)

<hr>

### Usage
#### Mapping properties using `EntityMap<TEntity>`
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

Initialization: 
```
FluentMapper.Intialize(config =>
					   {
						   config.AddMap(new ProductMap());
					   });
```

#### Mapping properties using conventions

You can create a convention by creating a class which derives from the `Convention` class. In the contructor you can configure the property conventions:
```
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

When initializing Dapper.FluentMap with conventions, the entities on which a convention applies must be configured. You can choose to either configure the entities explicitly or scan a specified, or the current assembly.

```
FluentMapper.Intialize(config =>
                        {
                            // Configure entities explicitly.
                            config.AddConvention(new TypePrefixConvention())
                                    .ForEntity<Product>()
                                    .ForEntity<Order>;

                            // Configure all entities in a certain assembly with an optional namespaces filter.
                            config.AddConvention(new TypePrefixConvention())
                                    .ForEntitiesInAssembly(typeof (Product).Assembly, "App.Domain.Model");

                            // Configure all entities in the current assembly with an optional namespaces filter.
                            config.AddConvention(new TypePrefixConvention())
                                    .ForEntitiesInCurrentAssembly("App.Domain.Model.Catalog", "App.Domain.Model.Order");
                        });
```
