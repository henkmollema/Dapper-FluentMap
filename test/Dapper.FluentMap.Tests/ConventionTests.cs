using System;
using System.Linq;
using System.Reflection;
using Dapper.FluentMap.Configuration;
using Dapper.FluentMap.Conventions;
using Dapper.FluentMap.Mapping;
using Dapper.FluentMap.TypeMaps;
using Xunit;

namespace Dapper.FluentMap.Tests
{
    public class ConventionTests
    {
        public ConventionTests()
        {
            FluentMapper.Configuration = null;
        }

        public class Bar
        {
            public string Name { get; set; }
        }

        [Fact]
        public void DerivedProperties()
        {
            // Arrange
            var conventionConfig = new FluentConventionConfiguration(new DerivedConvention());
            conventionConfig.ForEntity<DerivedTestEntity>();

            SqlMapper.ITypeMap CreateTypMap(Type t) => new FluentTypeMap(t, conventionConfig.EntityMaps[t].Compile());
            var typeMap = CreateTypMap(typeof(DerivedTestEntity));

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
            var conventionConfig = new FluentConventionConfiguration(new DerivedConvention());
            conventionConfig
                .ForEntity<DerivedTestEntity>()
                .ForEntity<Bar>();

            SqlMapper.ITypeMap CreateTypMap(Type t) => new FluentTypeMap(t, conventionConfig.EntityMaps[t].Compile());

            var derivedTypeMap = CreateTypMap(typeof(DerivedTestEntity));
            var typeMapBar = CreateTypMap(typeof(Bar));

            // Act
            var colNameFoo = derivedTypeMap.GetMember("colName");
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
            // Arrange
            var conventionConfig = new FluentConventionConfiguration(new TestConvention());

            // Act
            conventionConfig.ForEntitiesInAssembly(typeof(ConventionTests).Assembly);

            // Assert
            var entityMappings = conventionConfig.EntityMaps.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            Assert.NotEmpty(entityMappings);
            Assert.True(entityMappings.ContainsKey(typeof(TestEntity)));
        }

        private class TestConvention : Convention
        {
            public TestConvention()
            {
                Properties<int>()
                    .Where(p => p.Name.ToLower() == "id")
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
