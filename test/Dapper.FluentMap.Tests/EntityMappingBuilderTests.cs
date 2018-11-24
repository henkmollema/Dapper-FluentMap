using System;
using System.Linq;
using Dapper.FluentMap.Mapping;
using Xunit;

namespace Dapper.FluentMap.Tests
{
    public class EntityMappingBuilderTests
    {
        [Fact]
        public void Map_WithDuplicateMapping_ThrowsException()
        {
            // Arrange
            var builder = new EntityMap<TestEntity>();
            builder.Map(p => p.Id).ToColumn("id");

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => builder.Map(p => p.Id).ToColumn("id"));
            Assert.Equal("Duplicate mapping detected. Property 'Id' is already mapped.", ex.Message);
        }

        [Fact]
        public void Maps_SingleColumnName()
        {
            // Arrange
            var mapping = new EntityMap<TestEntity>();

            // Act
            mapping.Map(p => p.Id).ToColumn("id");

            // Assert
            Assert.Single(mapping.PropertyMaps);
            var propertyMap = mapping.PropertyMaps.Single();
            Assert.Equal("id", propertyMap.ColumnName);
        }

        [Fact]
        public void Maps_MultipleColumnNames()
        {
            // Arrange
            var mapping = new EntityMap<TestEntity>();

            // Act
            mapping.Map(p => p.Id).ToColumn("id");
            mapping.Map(p => p.Description).ToColumn("desc");

            // Assert
            Assert.Equal(2, mapping.PropertyMaps.Count);
            var first = mapping.PropertyMaps.First();
            Assert.Equal("id", first.ColumnName);

            var second = mapping.PropertyMaps.Skip(1).First();
            Assert.Equal("desc", second.ColumnName);
        }

        [Fact]
        public void IsCaseSensitive_SpecifiesCaseSensitivity()
        {
            // Arrange
            var mapping = new EntityMap<TestEntity>();

            // Act
            mapping.IsCaseSensitive(false);

            // Assert
            Assert.False(mapping.CaseSensitive);
            Assert.Empty(mapping.PropertyMaps);
        }

        [Fact]
        public void Compile_CompilesDictionary()
        {
            // Arrange
            var mapping = new EntityMap<TestEntity>();
            mapping.Map(p => p.Id).ToColumn("id");
            mapping.Map(p => p.Description).ToColumn("desc");

            // Act
            var dict = mapping.Compile();

            // Assert
            Assert.Equal(2, dict.Count);
            var first = dict.First();
            Assert.Equal("id", first.Key);
            Assert.Equal(typeof(TestEntity).GetProperty("Id"), first.Value);

            var second = dict.Skip(1).First();
            Assert.Equal("desc", second.Key);
            Assert.Equal(typeof(TestEntity).GetProperty("Description"), second.Value);
        }
    }
}
