using System;
using System.Reflection;

namespace Dapper.FluentMap.TypeMaps
{
    /// <summary>
    /// Represents a Dapper type mapping strategy which consists of multiple strategies.
    /// </summary>
    public abstract class MultiTypeMap : SqlMapper.ITypeMap
    {
        private readonly SqlMapper.ITypeMap[] _mappers;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiTypeMap"/>
        /// class with the specified Dapper type mappers.
        /// </summary>
        /// <param name="mappers">The type mapping strategies to be used when mapping.</param>
        protected MultiTypeMap(params SqlMapper.ITypeMap[] mappers)
        {
            _mappers = mappers;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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
    }
}
