using Dapper.FluentMap.Conventions;

namespace Dapper.FluentMap.Mapping
{
    /// <summary>
    /// Defines methods for configuring Dapper.FluentMap.
    /// </summary>
    public interface IFluentMapConfiguration
    {
        /// <summary>
        /// Adds the specified <see cref="T:Dapper.FluentMap.Mapping.EntityMap"/> to the configuration of Dapper.FluentMap.
        /// </summary>
        /// <typeparam name="TEntity">The type argument of the entity.</typeparam>
        /// <param name="mapper">An instance of the EntityMap classs containing the entity mapping configuration.</param>
        void AddMap<TEntity>(EntityMap<TEntity> mapper) where TEntity : class;

        /// <summary>
        /// Adds the specified <see cref="T:Dapper.FluentMap.Conventions.Convention"/> to the configuration of Dapper.FluentMap.
        /// </summary>
        /// <param name="convention">An instance of the <see cref="T:Dapper.FluentMap.Conventions.Convention"/> class.</param>
        /// <returns>
        /// An instance of <see cref="T:Dapper.FluentMap.Conventions.FluentMapConventionConfiguration"/> 
        /// which allows configuration of the convention.
        /// </returns>
        FluentMapConventionConfiguration AddConvention(Convention convention);
    }

    internal class FluentMapConfiguration : IFluentMapConfiguration
    {
        public void AddMap<TEntity>(EntityMap<TEntity> mapper) where TEntity : class
        {
            FluentMapper.EntityMappers.Add(typeof (TEntity), mapper);
            FluentMapper.AddTypeMap<TEntity>();
        }

        public FluentMapConventionConfiguration AddConvention(Convention convention)
        {
            return new FluentMapConventionConfiguration(convention);
        }
    }
}
