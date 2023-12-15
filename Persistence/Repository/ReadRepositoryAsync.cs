using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repository
{
    public class ReadRepositoryAsync<TEntity> : IReadRepositoryAsync<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext context;

        public ReadRepositoryAsync(ApplicationDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                return await context.Set<TEntity>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public virtual async Task<TEntity> GetByIdAsync<TId>(TId id)
        {
            try
            {
                return await context.FindAsync<TEntity>(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve the entity with id {id}: {ex.Message}");
            }
        }

    }
}
