dotnet restore
dotnet build ./src/Dapper.FluentMap/Dapper.FluentMap.csproj -f netstandard2.0
dotnet build ./src/Dapper.FluentMap.Dommel/Dapper.FluentMap.Dommel.csproj -f netstandard2.0
dotnet test ./test/Dapper.FluentMap.Tests/Dapper.FluentMap.Tests.csproj
