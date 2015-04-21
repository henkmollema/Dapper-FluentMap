using System;
using System.Linq;
using Dapper.FluentMap.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dapper.FluentMap.Tests
{
    [TestClass]
    public class ManualMappingTests
    {
        [TestMethod]
        [ExpectedException(typeof (Exception))]
        public void Duplicate_Mapping_Should_Throw_Exception()
        {
            PreTest();
            var map = new MapWithDuplicateMapping();
        }

        [TestMethod]
        public void EntityMap_Should_Have_Empty_PropertyMap_Collection()
        {
            PreTest();
            var map = new EmptyMap();
            Assert.IsNotNull(map.PropertyMaps, "PropertyMaps collection is not initialized with an empty list.");
        }

        [TestMethod]
        public void EntityMap_Should_Have_PropertyMap()
        {
            PreTest();
            var map = new MapWithOnePropertyMap();
            Assert.IsNotNull(map.PropertyMaps, "PropertyMaps collection is not initialized with an empty list.");
            Assert.IsTrue(map.PropertyMaps.Count == 1, "PropertyMaps has less or more than 1 item.");
        }

        [TestMethod]
        public void PropertyMap_Should_Be_CaseInsensitive()
        {
            PreTest();
            var map = new CaseInsensitveMap();
            var propertyMap = map.PropertyMaps.Single();
            Assert.IsTrue(propertyMap.ColumnName == "test", "ColumnName of PropertyMap is incorrect.");
            Assert.IsFalse(propertyMap.CaseSensitive, "PropertyMap is not case insensitive.");
        }

        [TestMethod]
        public void Property_Should_Be_Ignored()
        {
            PreTest();
            var map = new IgnoreMap();
            var propertyMap = map.PropertyMaps.Single();
            Assert.IsTrue(propertyMap.Ignored, "Property is not ignored.");
        }

        [TestMethod]
        public void PropertyInfo_Name_Should_Be_Id()
        {
            PreTest();
            var map = new MapWithOnePropertyMap();
            var propertyMap = map.PropertyMaps.Single();
            Assert.IsTrue(propertyMap.PropertyInfo.Name == "Id", "PropertyInfo.Name is not the same as expression property.");
        }

        [TestMethod]
        public void FluentMapper_Initialize_Should_Add_EntityMap()
        {
            PreTest();
            FluentMapper.Initialize(c => c.AddMap(new MapWithOnePropertyMap()));
            Assert.IsTrue(FluentMapper.EntityMaps.Count == 1, "FluentMapper.EntityMaps contains less or more than 1 mapping.");

            var entityMap = FluentMapper.EntityMaps.Single();
            Assert.IsTrue(entityMap.Key == typeof (TestEntity), "EntityMap entity type is not correct.");
            Assert.IsTrue(entityMap.Value is MapWithOnePropertyMap, "EntityMap type is not correct.");
        }

        [TestMethod]
        public void FluentMapper_Initialize_Should_Add_Dapper_TypeMap()
        {
            PreTest();
            FluentMapper.Initialize(c => c.AddMap(new MapWithOnePropertyMap()));
            SqlMapper.ITypeMap typeMap = SqlMapper.GetTypeMap(typeof (TestEntity));
            Assert.IsNotNull(typeMap, "TypeMap is null.");
        }

        [TestMethod]
        public void PropertyMap_ShouldMapInheritedProperies()
        {
            PreTest();
            var map = new DerivedMap();
            var idMap = map.PropertyMaps.First();
            Assert.IsTrue(idMap.PropertyInfo.ReflectedType == typeof (DerivedTestEntity));

            var nameName = map.PropertyMaps.Skip(1).First();
            Assert.IsTrue(nameName.PropertyInfo.ReflectedType == typeof(DerivedTestEntity));
        }

        private static void PreTest()
        {
            FluentMapper.EntityMaps.Clear();
            FluentMapper.TypeConventions.Clear();
        }

        private class IgnoreMap : EntityMap<TestEntity>
        {
            public IgnoreMap()
            {
                Map(p => p.Id).Ignore();
            }
        }

        private class CaseInsensitveMap : EntityMap<TestEntity>
        {
            public CaseInsensitveMap()
            {
                Map(p => p.Id).ToColumn("test", caseSensitive: false);
            }
        }

        private class MapWithOnePropertyMap : EntityMap<TestEntity>
        {
            public MapWithOnePropertyMap()
            {
                Map(p => p.Id).ToColumn("test");
            }
        }

        private class DerivedMap : EntityMap<DerivedTestEntity>
        {
            public DerivedMap()
            {
                Map(p => p.Id).ToColumn("intId");
                Map(p => p.Name).ToColumn("strName");
            }
        }

        private class MapWithDuplicateMapping : EntityMap<TestEntity>
        {
            public MapWithDuplicateMapping()
            {
                Map(p => p.Id).ToColumn("id");
                Map(p => p.Id).ToColumn("id2");
            }
        }

        private class EmptyMap : EntityMap<TestEntity>
        {
        }
    }
}
