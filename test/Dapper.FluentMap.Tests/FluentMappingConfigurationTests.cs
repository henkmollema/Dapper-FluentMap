using System.Linq;
using Dapper.FluentMap.Configuration;
using Dapper.FluentMap.Mapping;
using Dapper.FluentMap.TypeMaps;
using Xunit;

namespace Dapper.FluentMap.Tests
{
    public class FluentMappingConfigurationTests
    {
        [Fact]
        public void AddMap_AddsDapperTypeMap()
        {
            // Act
            FluentMapper.Initialize(c =>
                c.Entity<TestEntity2>(e => e.Map(p => p.Id).ToColumn("entity_id")));

            // Assert
            var typeMap = SqlMapper.GetTypeMap(typeof(TestEntity2));
            Assert.IsType<FluentTypeMap>(typeMap);

            var idMap = typeMap.GetMember("entity_id");
            Assert.NotNull(idMap);
            Assert.Equal(typeof(TestEntity2).GetProperty("Id"), idMap.Property);
        }

        private class TestEntity2
        {
            public string Id { get; set; }
        }
    }
}
