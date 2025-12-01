namespace UmaDesignli.Application.Interfaces.Repositories
{
    /// <summary>
    /// Generic Repository for classes
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Get all generic.
        /// </summary>
        /// <returns>Return all information</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Get by Id.
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Entity by Id</returns>
        Task<TEntity?> GetByIdAsync(int id);

        /// <summary>
        /// Create an entity.
        /// </summary>
        /// <param name="entity">The entity values</param>
        /// <returns>enitity created</returns>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="entity">entity values</param>
        /// <returns></returns>
        Task<TEntity?> UpdateAsync(int id, TEntity entity);

        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>deleted or not</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Return the counts of fields
        /// </summary>
        /// <returns></returns>
        Task<int> CountAsync();

        /// <summary>
        /// Create multiples entities.
        /// </summary>
        /// <param name="entities">entities values</param>
        /// <returns>enities created</returns>
        Task AddRangeAsync(IEnumerable<TEntity> entities);
    }
}
