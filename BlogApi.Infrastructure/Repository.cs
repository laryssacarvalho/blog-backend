using BlogApi.Application.Interfaces;
using BlogApi.Domain.Entities;
using BlogApi.Infrastructure.Database;

namespace BlogApi.Infrastructure
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly BlogDbContext _dbContext;

        public Repository(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public IEnumerable<TEntity> Find()
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

    }
}
