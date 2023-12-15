namespace Persistence.Repository
{
    public interface IRepositoryAsync<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync<TId>(TId id);
    }
}
