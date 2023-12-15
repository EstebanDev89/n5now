using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repository
{
    public class RepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext context;

        public RepositoryAsync(ApplicationDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity must not be null");
            }

            try
            {
                await context.AddAsync(entity);
                await context.SaveChangesAsync();

                return entity;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Error saving entity: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity must not be null");
            }

            try
            {
                context.Update(entity);
                await context.SaveChangesAsync();

                return entity;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Error updating entity: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public virtual async Task DeleteAsync<TId>(TId id)
        {
            var entity = await context.FindAsync<TEntity>(id);
            
            if (entity == null)
            {
                throw new Exception($"Permission {id} not found");
            }

            context.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
