using Dapper.FluentMap.Mapping;
using Dapper.FluentMap.TypeMaps;
using Xunit;

namespace Dapper.FluentMap.Tests
{
    public class FluentMapTypeMapTests
    {
        [Fact]
        public void FluentMapTypeMap_ReturnsCorrectMemberMap()
        {
            // Arrange
            var mapping = new EntityMap<TestEntity>();
            mapping.Map(p => p.Id).ToColumn("id");
            mapping.Map(p => p.Description).ToColumn("desc");
            var dict = mapping.Compile();
            var typeMap = new FluentTypeMap<TestEntity>(dict);

            // Act
            var memberMap = typeMap.GetMember("desc");

            // Assert
            Assert.Equal("desc", memberMap.ColumnName);
            Assert.Equal(typeof(string), memberMap.MemberType);
            Assert.Equal(typeof(TestEntity).GetProperty("Description"), memberMap.Property);
        }

        [Fact]
        public void FluentMapTypeMap_ReturnsCorrectMemberMap_Nullable()
        {
            // Arrange
            var mapping = new EntityMap<TestEntityWithNullable>();
            mapping.Map(p => p.Value).ToColumn("some_value");
            var dict = mapping.Compile();
            var typeMap = new FluentTypeMap<TestEntityWithNullable>(dict);

            // Act
            var memberMap = typeMap.GetMember("some_value");

            // Assert
            Assert.NotNull(memberMap);
            Assert.Equal("some_value", memberMap.ColumnName);
            Assert.Equal(typeof(decimal?), memberMap.MemberType);
            Assert.Equal(typeof(TestEntityWithNullable).GetProperty(nameof(TestEntityWithNullable.Value)), memberMap.Property);
            Assert.NotNull(memberMap.Property.GetSetMethod());
        }
    }
}
