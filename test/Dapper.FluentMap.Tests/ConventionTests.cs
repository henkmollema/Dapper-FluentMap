using System.Linq;
using System.Reflection;
using Dapper.FluentMap.Conventions;
using Xunit;

namespace Dapper.FluentMap.Tests
{
    public class ConventionTests
    {
        public ConventionTests()
        {
            // Clear configurations
            FluentMapper.EntityMaps.Clear();
            FluentMapper.TypeConventions.Clear();
        }

        public class Bar
        {
            public string Name { get; set; }
        }

        [Fact]
        public void DerivedProperties()
        {
            // Arrange
            FluentMapper.Initialize(c => c.AddConvention<DerivedConvention>().ForEntity<DerivedTestEntity>());
            var typeMap = SqlMapper.GetTypeMap(typeof(DerivedTestEntity));

            // Act
            var colName = typeMap.GetMember("colName");
            var colId = typeMap.GetMember("colId");

            //Assert
            Assert.NotNull(colName);
            Assert.NotNull(colId);
            Assert.Equal(typeof(DerivedTestEntity).GetProperty(nameof(DerivedTestEntity.Name)), colName.Property);
            Assert.Equal(typeof(DerivedTestEntity).GetProperty(nameof(TestEntity.Id)), colId.Property);
        }

        [Fact]
        public void TwoEntitiesWithSamePropertyName()
        {
            // Arrange
            FluentMapper.Initialize(c =>
                c.AddConvention<DerivedConvention>()
                 .ForEntity<DerivedTestEntity>()
                 .ForEntity<Bar>());

            var typeMapFoo = SqlMapper.GetTypeMap(typeof(DerivedTestEntity));
            var typeMapBar = SqlMapper.GetTypeMap(typeof(Bar));

            // Act
            var colNameFoo = typeMapFoo.GetMember("colName");
            var colNameBar = typeMapBar.GetMember("colName");

            // Assert
            Assert.NotNull(colNameFoo);
            Assert.NotNull(colNameBar);
            Assert.Equal(typeof(DerivedTestEntity).GetProperty(nameof(Bar.Name)), colNameFoo.Property);
            Assert.Equal(typeof(Bar).GetProperty(nameof(Bar.Name)), colNameBar.Property);
        }

        [Fact]
        public void ShouldMapEntitiesInAssembly()
        {
            // Arrange & Act
            FluentMapper.Initialize(c => c.AddConvention<TestConvention>().ForEntitiesInAssembly(typeof(ConventionTests).GetTypeInfo().Assembly));

            // Assert
            var conventions = FluentMapper.TypeConventions.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            Assert.NotEmpty(conventions);
            Assert.True(conventions.ContainsKey(typeof(TestEntity)));
            var map = conventions[typeof(TestEntity)];
            Assert.True(map[0] is TestConvention);
        }

        private class TestConvention : Convention
        {
            public TestConvention()
            {
                Properties<int>().
                    Where(p => p.Name.ToLower() == "id")
                    .Configure(c => c.HasColumnName("autID"));
            }
        }

        private class DerivedConvention : Convention
        {
            public DerivedConvention()
            {
                Properties()
                    .Configure(c => c.HasPrefix("col"));
            }
        }
    }
}
