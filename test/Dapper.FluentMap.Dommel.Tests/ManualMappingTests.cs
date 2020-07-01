using Dapper.FluentMap.Dommel.Mapping;
using System;
using System.Linq;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Dapper.FluentMap.Dommel.Tests
{
    public class ManualMappingTests
    {
        [Fact]
        public void EntityMapsCustomIdAsKey()
        {
            PreTest();

            FluentMapper.Initialize(c => c.AddMap(new MapWithCustomIdPropertyMap()));

            var type = typeof(CustomIdEntity);
            var keyResolver = new Dommel.Resolvers.DommelKeyPropertyResolver();
            var columnResolver = new Dommel.Resolvers.DommelColumnNameResolver();

            var keys = keyResolver.ResolveKeyProperties(type);
            var columnName = columnResolver.ResolveColumnName(keys.Single().Property);
            
            Assert.Single(keys);
            Assert.Equal("customid", columnName);
        }

        [Fact]
        public void EntityMapsToSingleCustomId()
        {
            PreTest();

            FluentMapper.Initialize(c => c.AddMap(new MapSingleCustomIdPropertyMap()));

            var type = typeof(DoubleIdEntity);
            var keyResolver = new Dommel.Resolvers.DommelKeyPropertyResolver();
            var columnResolver = new Dommel.Resolvers.DommelColumnNameResolver();
            FluentMapper.EntityMaps.TryGetValue(type, out var entityMap);

            var keys = keyResolver.ResolveKeyProperties(type);
            var columnName = columnResolver.ResolveColumnName(keys.Single().Property);

            var idName = columnResolver.ResolveColumnName(entityMap.PropertyMaps.Single(x => x.PropertyInfo.Name == nameof(DoubleIdEntity.Id)).PropertyInfo);

            Assert.Single(keys);
            Assert.Equal("id", columnName);
            Assert.Equal("Id", idName);
        }

        [Fact]
        public void EntityMapsToDefaultSingleKey()
        {
            PreTest();

            FluentMapper.Initialize(c => c.AddMap(new MapSingleCustomIdDefaultKey()));

            var type = typeof(DoubleIdEntity);
            var keyResolver = new Dommel.Resolvers.DommelKeyPropertyResolver();
            var keys = keyResolver.ResolveKeyProperties(type);
            Assert.Single(keys);
        }

        [Fact]
        public void KeyPropertyIsGenerated()
        {
            PreTest();

            FluentMapper.Initialize(c => c.AddMap(new MapSingleCustomIdPropertyMap()));

            var type = typeof(DoubleIdEntity);
            var keyResolver = new Dommel.Resolvers.DommelKeyPropertyResolver();
            var keys = keyResolver.ResolveKeyProperties(type);

            var key = keys.FirstOrDefault();
            Assert.True(key.IsGenerated);
        }

        [Fact]

        public void PropertyIsGenerated()
        {
            PreTest();

            FluentMapper.Initialize(c => c.AddMap(new MapSingleCustomIdPropertyMap()));

            var type = typeof(DoubleIdEntity);
            var propertyResolver = new Dommel.Resolvers.DommelPropertyResolver();
            var properties = propertyResolver.ResolveProperties(type);

            var property = properties.Where(x => x.IsGenerated);
            Assert.NotEmpty(property);

        }

        private static void PreTest()
        {
            FluentMapper.EntityMaps.Clear();
            FluentMapper.TypeConventions.Clear();
        }

        private class MapWithCustomIdPropertyMap : DommelEntityMap<CustomIdEntity>
        {
            public MapWithCustomIdPropertyMap()
            {
                Map(p => p.CustomId).ToColumn("customid").IsIdentity().IsKey();
            }
        }

        private class MapSingleCustomIdPropertyMap : DommelEntityMap<DoubleIdEntity>
        {
            public MapSingleCustomIdPropertyMap()
            {
                Map(p => p.Id);
                Map(p => p.CustomId).IsKey().ToColumn("id", false);
            }
        }

        private class MapSingleCustomIdDefaultKey : DommelEntityMap<DoubleIdEntity>
        {
            public MapSingleCustomIdDefaultKey()
            {
                Map(p => p.CustomId).ToColumn("customid", false);
            }
        }
    }
}
