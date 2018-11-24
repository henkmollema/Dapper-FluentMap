using System;
using Dapper.FluentMap.Configuration;
using Dapper.FluentMap.Mapping;
using Xunit;

[assembly:CollectionBehavior(DisableTestParallelization = true)]
namespace Dapper.FluentMap.Tests
{
    [Collection("FluentMapper")]
    public class FluentMapperTests
    {
        public FluentMapperTests()
        {
            // Clear any existing configuration first.
            FluentMapper.Configuration = null;
        }

        [Fact]
        public void GetConfiguration_WithoutInitialize_ThrowsException()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => { var x = FluentMapper.Configuration; });
            var msg = $"FluentMapper is not initialized. Use {nameof(FluentMapper.Initialize)} to configure your mappings.";
            Assert.Equal(msg, ex.Message);
        }

        [Fact]
        public void Initialize_WithNullConfiguration_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>("configure", () => FluentMapper.Initialize(null));
        }

        [Fact]
        public void Initialize_WithoutConfiguration_ResultsInEmptyMappings()
        {
            FluentMapper.Initialize(config => { });
            Assert.IsType<FluentMapConfiguration>(FluentMapper.Configuration);
            Assert.Empty(FluentMapper.Configuration.EntityMaps);
        }

        [Fact]
        public void Initialize_WithConfiguration_ConfiguresCorrectly()
        {
            var config = FluentMapper.Initialize(c =>
            {
                c.Entity<TestEntity>(builder =>
                {
                    builder.Map(e => e.Id).ToColumn("autID");
                    builder.Map(e => e.Description).ToColumn("strDescription");
                });

                c.Entity<TestEntity2>(builder =>
                {
                    builder.IsCaseSensitive(false);
                    builder.Map(e => e.EntityId).ToColumn("Id");
                    builder.Map(e => e.Amount).ToColumn("EntityAmount");
                });

                c.AddMap(new TestEntity3Map());
            });

            var mapping = config.EntityMaps;

            var first = Assert.IsType<EntityMap<TestEntity>>(mapping[typeof(TestEntity)]);
            Assert.True(first.CaseSensitive);
            Assert.Equal(2, first.PropertyMaps.Count);


            var second = Assert.IsType<EntityMap<TestEntity2>>(mapping[typeof(TestEntity2)]);
            Assert.False(second.CaseSensitive);
            Assert.Equal(2, second.PropertyMaps.Count);

            var third = Assert.IsAssignableFrom<EntityMap<TestEntity3>>(mapping[typeof(TestEntity3)]);
            Assert.True(third.CaseSensitive);
            Assert.Equal(2, third.PropertyMaps.Count);
        }
    }

    public class TestEntity2
    {
        public Guid EntityId { get; set; }

        public decimal? Amount { get; set; }
    }

    public class TestEntity3
    {
        public int EntityId { get; set; }

        public string Description { get; set; }
    }

    public class TestEntity3Map : EntityMap<TestEntity3>
    {
        public TestEntity3Map()
        {
            Map(e => e.EntityId).ToColumn("entity_id");
            Map(e => e.Description).ToColumn("description");
        }
    }
}
