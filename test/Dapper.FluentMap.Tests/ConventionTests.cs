using Dapper.FluentMap.Conventions;
using Xunit;

namespace Dapper.FluentMap.Tests
{
    public class ConventionTests
    {
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
