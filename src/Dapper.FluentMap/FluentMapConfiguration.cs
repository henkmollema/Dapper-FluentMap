namespace Dapper.FluentMap
{
    /// <summary>
    /// Defines methods for configuring Dapper.FluentMap.
    /// </summary>
    public interface IFluentMapConfiguration
    {
        /// <summary>
        /// Adds the specified EntityMap to the configuration of Dapper.FluentMap.
        /// </summary>
        /// <typeparam name="TEntity">The type argument of the entity.</typeparam>
        /// <param name="mapper">An instance of the EntityMap classs containing the entity mapping configuration.</param>
        void AddEntityMap<TEntity>(EntityMap<TEntity> mapper) where TEntity : class;
    }

    public class FluentMapConfiguration : IFluentMapConfiguration
    {
        public void AddEntityMap<TEntity>(EntityMap<TEntity> mapper) where TEntity : class
        {
            FluentMapper.EntityMappers.Add(typeof (TEntity), mapper);
            FluentMapper.AddTypeMap<TEntity>();
        }
    }
}
