using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dapper.FluentMap.Conventions;

namespace Dapper.FluentMap.TypeMaps
{
    /// <summary>
    /// Represents a Dapper type mapping strategy which first tries to map the type using a <see cref="T:Dapper.CustomPropertyTypeMap"/>
    /// with the configured conventions. <see cref="T:Dapper.DefaultTypeMap"/> is used as fallback mapping strategy.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    internal class FluentConventionTypeMap<TEntity> : MultiTypeMap
    {
        /// <summary>
        /// Intializes a new instance of the <see cref="T:Dapper.FluentMap.FluentMapTypeMapper"/> class 
        /// which uses the <see cref="T:Dapper.CustomPropertyTypeMap"/> and <see cref="T:Dapper.DefaultTypeMap"/>
        /// as mapping strategies.
        /// </summary>
        public FluentConventionTypeMap()
            : base(new CustomPropertyTypeMap(typeof (TEntity), GetPropertyInfo), new DefaultTypeMap(typeof (TEntity)))
        {
        }

        private static PropertyInfo GetPropertyInfo(Type type, string columnName)
        {
            string cacheKey = string.Format("{0};{1}", type.FullName, columnName);

            PropertyInfo info;
            if (TypePropertyMapCache.TryGetValue(cacheKey, out info))
            {
                return info;
            }

            IList<Convention> conventions;
            if (FluentMapper.TypeConventions.TryGetValue(type, out conventions))
            {
                foreach (var convention in conventions)
                {
                    // Find property map for current type and column name.
                    var maps = convention.PropertyMaps
                                         .Where(c => c.PropertyInfo.DeclaringType == type &&
                                                     c.ColumnName == columnName)
                                         .ToList();

                    if (maps.Count > 1)
                    {
                        const string msg = "Finding mappings for column '{0}' yielded more than 1 PropertyMap. The conventions should be more specific. Type: '{1}'. Convention: '{2}'.";
                        throw new Exception(string.Format(msg, columnName, type, convention));
                    }

                    if (maps.Count == 0)
                    {
                        return null;
                    }

                    info = maps[0].PropertyInfo;
                    TypePropertyMapCache.Add(cacheKey, info);
                    return info;
                }
            }

            // If we get here, the property was not mapped.
            TypePropertyMapCache.Add(cacheKey, null);
            return null;
        }
    }
}
