using System.Linq;
using Dapper.FluentMap.Configuration;
using Dapper.FluentMap.Dommel.Mapping;
using Xunit;

namespace Dapper.FluentMap.Dommel.Tests
{
    public class DommelExtensionsTests
    {
        [Fact]
        public void InlineDommelConfiguration_MapsDommelSpecificConfiguration()
        {
            // Arrange
            var config = new FluentMapConfiguration();

            // Act
            config.DommelEntity<TestEntity>(builder =>
            {
                builder.ToTable("test");
                builder.Map(p => p.Id)
                    .ToColumn("id")
                    .IsKey();
            });

            // Assert
            AssertMapping(config);
        }

        [Fact]
        public void DommelEntityMappingBuilder_MapsDommelSpecificConfiguration()
        {
            // Arrange
            var config = new FluentMapConfiguration();

            // Act
            config.AddMap(new TestEntityMap());

            // Assert
            AssertMapping(config);
        }

        private void AssertMapping(FluentMapConfiguration config)
        {
            Assert.Single(config.EntityMaps);
            var entityMapping = config.EntityMaps.Values.First();
            var dommelEntityMapping = Assert.IsAssignableFrom<DommelEntityMap<TestEntity>>(entityMapping);
            Assert.Equal("test", dommelEntityMapping.TableName);

            Assert.Single(dommelEntityMapping.PropertyMaps);
            var propertyMapping = dommelEntityMapping.PropertyMaps.First();
            var dommelPropertyMapping = Assert.IsType<DommelPropertyMap>(propertyMapping);
            Assert.Equal("id", dommelPropertyMapping.ColumnName);
            Assert.True(dommelPropertyMapping.Key);
            Assert.Equal(typeof(TestEntity).GetProperty("Id"), dommelPropertyMapping.PropertyInfo);
        }
    }

    public class TestEntityMap : DommelEntityMap<TestEntity>
    {
        public TestEntityMap()
        {
            ToTable("test");
            Map(p => p.Id)
                .ToColumn("id")
                .IsKey();
        }
    }

    public class TestEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }
    }
}
