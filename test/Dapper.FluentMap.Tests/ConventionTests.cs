using Dapper.FluentMap.Conventions;
using Xunit;
using System.Reflection;

namespace Dapper.FluentMap.Tests
{
    public class ConventionTests
    {
#if DNX451
        [Fact]
        public void ShouldMapEntitiesInCurrentAssembly()
        {
            // Arrange & Act
            FluentMapper.Initialize(c => c.AddConvention<TestConvention>().ForEntitiesInCurrentAssembly());

            // Asert
            Assert.True(FluentMapper.TypeConventions.ContainsKey(typeof(TestEntity)));
            var map = FluentMapper.TypeConventions[typeof(TestEntity)];
            Assert.True(map[0] is TestConvention);
        }
#elif DNXCORE50
        [Fact]
        public void ShouldMapEntitiesInCurrentAssembly()
        {
            // Arrange & Act
            FluentMapper.Initialize(c => c.AddConvention<TestConvention>().ForEntitiesInAssembly(typeof(ConventionTests).GetTypeInfo().Assembly));

            // Asert
            Assert.True(FluentMapper.TypeConventions.ContainsKey(typeof(TestEntity)));
            var map = FluentMapper.TypeConventions[typeof(TestEntity)];
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
