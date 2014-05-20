using System;
using System.ComponentModel;
using Dapper.FluentMap.Conventions;
using Dapper.FluentMap.Mapping;

namespace Dapper.FluentMap.Configuration
{
    /// <summary>
    /// Defines methods for configuring Dapper.FluentMap.
    /// </summary>
    public class FluentMapConfiguration
    {
        /// <summary>
        /// Adds the specified <see cref="T:Dapper.FluentMap.Mapping.EntityMap"/> to the configuration of Dapper.FluentMap.
        /// </summary>
        /// <typeparam name="TEntity">The type argument of the entity.</typeparam>
        /// <param name="mapper">An instance of the EntityMap classs containing the entity mapping configuration.</param>
        public void AddMap<TEntity>(EntityMap<TEntity> mapper) where TEntity : class
        {
            FluentMapper.EntityMappers.Add(typeof (TEntity), mapper);
            FluentMapper.AddTypeMap<TEntity>();
        }

        /// <summary>
        /// Adds the specified <see cref="T:Dapper.FluentMap.Conventions.Convention"/> to the configuration of Dapper.FluentMap.
        /// </summary>
        /// <param name="convention">An instance of the <see cref="T:Dapper.FluentMap.Conventions.Convention"/> class.</param>
        /// <returns>
        /// An instance of <see cref="T:Dapper.FluentMap.Conventions.FluentMapConventionConfiguration"/> 
        /// which allows configuration of the convention.
        /// </returns>
        public FluentConventionConfiguration AddConvention(Convention convention)
        {
            return new FluentConventionConfiguration(convention);
        }

        #region EditorBrowsableStates
        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType()
        {
            return base.GetType();
        }
        #endregion
    }
}
