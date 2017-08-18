dotnet restore
dotnet build ./src/Dapper.FluentMap/Dapper.FluentMap.csproj -f netstandard1.3
dotnet build ./src/Dapper.FluentMap.Dommel/Dapper.FluentMap.Dommel.csproj -f netstandard1.3
dotnet test ./test/Dapper.FluentMap.Tests/Dapper.FluentMap.Tests.csproj
