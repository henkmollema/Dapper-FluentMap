using System.Linq;
using System.Reflection;
using Dapper.FluentMap.Conventions;
using Xunit;

namespace Dapper.FluentMap.Tests
{
    public class ConventionTests
    {
        [Fact]
        public void ShouldMapEntitiesInAssembly()
        {
            PreTest();

            // Arrange & Act
            FluentMapper.Initialize(c => c.AddConvention<TestConvention>().ForEntitiesInAssembly(typeof(ConventionTests).GetTypeInfo().Assembly));

            // Assert
            var conventions = FluentMapper.TypeConventions.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            foreach (var x in conventions)
            {            
                System.Console.WriteLine("Type: " + x.Key.ToString());
            }
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

        private static void PreTest()
        {
            FluentMapper.EntityMaps.Clear();
            FluentMapper.TypeConventions.Clear();
        }
    }
}
