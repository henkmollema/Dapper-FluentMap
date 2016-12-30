using System.Linq;
using Dapper.FluentMap.Conventions;
using Xunit;

#if COREFX
using System.Reflection;
#endif

namespace Dapper.FluentMap.Tests
{
    public class ConventionTests
    {
#if NET451
        [Fact]
        public void ShouldMapEntitiesInCurrentAssembly()
        {
            PreTest();

            // Arrange & Act
            FluentMapper.Initialize(c => c.AddConvention<TestConvention>().ForEntitiesInCurrentAssembly());

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

#elif COREFX
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
#endif

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
