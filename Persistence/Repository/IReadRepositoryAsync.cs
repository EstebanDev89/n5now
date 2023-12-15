namespace Persistence.Repository
{
    public interface IReadRepositoryAsync<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync<TId>(TId id);
    }
}
