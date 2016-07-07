using System;
using System.Linq;
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
        /// <param name="mapper">
        /// An instance of the <see cref="T:Dapper.FluentMap.Mapping.IEntityMap"/> interface containing the
        /// entity mapping configuration.
        /// </param>
        public void AddMap<TEntity>(IEntityMap<TEntity> mapper) where TEntity : class
        {
            if (FluentMapper.EntityMaps.TryAdd(typeof(TEntity), mapper))
            {
                FluentMapper.AddTypeMap<TEntity>();
            }
            else
            {
                throw new InvalidOperationException($"Adding entity map for type '{typeof(TEntity)}' failed. The type already exists. Current entity maps: " + string.Join(", ", FluentMapper.EntityMaps.Select(e => e.Key.ToString())));
            }
        }

        /// <summary>
        /// Adds the specified <see cref="T:Dapper.FluentMap.Conventions.Convention"/> to the configuration of Dapper.FluentMap.
        /// </summary>
        /// <typeparam name="TConvention">The type of the convention.</typeparam>
        /// <returns>
        /// An instance of <see cref="T:Dapper.FluentMap.Configuration.FluentConventionConfiguration"/>
        /// which allows configuration of the convention.
        /// </returns>
        public FluentConventionConfiguration AddConvention<TConvention>() where TConvention : Convention, new()
        {
            return new FluentConventionConfiguration(new TConvention());
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
