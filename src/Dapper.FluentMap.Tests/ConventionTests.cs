using System.Diagnostics;
using Dapper.FluentMap.Conventions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dapper.FluentMap.Tests
{
    [TestClass]
    public class ConventionTests
    {
        [TestMethod]
        public void ShouldMapEntitiesInCurrentAssembly()
        {
            PreTest();
            FluentMapper.Initialize(c => c.AddConvention<TestConvention>()
                                         .ForEntitiesInCurrentAssembly());

            Debug.Assert(FluentMapper.TypeConventions.ContainsKey(typeof (TestEntity)));
            var map = FluentMapper.TypeConventions[typeof (TestEntity)];
            Debug.Assert(map[0] is TestConvention);
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
