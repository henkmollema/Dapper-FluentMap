using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Dapper.FluentMap.TypeMaps
{
    /// <summary>
    /// Represents a Dapper type mapping strategy which consists of multiple strategies.
    /// </summary>
    internal abstract class MultiTypeMap : SqlMapper.ITypeMap
    {
        private readonly IEnumerable<SqlMapper.ITypeMap> _mappers;
        private static readonly ConcurrentDictionary<string, PropertyInfo> _typePropertyMapCache = new ConcurrentDictionary<string, PropertyInfo>();

        /// <summary>
        /// Initializes an instance of the <see cref="T:Dapper.FluentMap.TypeMaps.MultiTypeMap"/> 
        /// class with the specified Dapper type mappers.
        /// </summary>
        /// <param name="mappers">The type mapping strategies to be used when mapping.</param>
        protected MultiTypeMap(params SqlMapper.ITypeMap[] mappers)
        {
            _mappers = mappers;
        }

        public ConstructorInfo FindConstructor(string[] names, Type[] types)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    var result = mapper.FindConstructor(names, types);

                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                    // Ignore NotImplementedException's thrown by the CustomPropertyTypeMap
                    // and continue to the next mapping strategy.
                }
            }

            return null;
        }

        public ConstructorInfo FindExplicitConstructor()
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    var result = mapper.FindExplicitConstructor();
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                    // Ignore NotImplementedException's thrown by the CustomPropertyTypeMap
                    // and continue to the next mapping strategy.
                }
            }

            return null;
        }

        public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    var result = mapper.GetConstructorParameter(constructor, columnName);

                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                    // Ignore NotImplementedException's thrown by the CustomPropertyTypeMap
                    // and continue to the next mapping strategy.
                }
            }

            return null;
        }

        public SqlMapper.IMemberMap GetMember(string columnName)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    var result = mapper.GetMember(columnName);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                    // Ignore NotImplementedException's thrown by the CustomPropertyTypeMap
                    // and continue to the next mapping strategy.
                }
            }

            return null;
        }

        protected static ConcurrentDictionary<string, PropertyInfo> TypePropertyMapCache
        {
            get
            {
                return _typePropertyMapCache;
            }
        }
    }
}
