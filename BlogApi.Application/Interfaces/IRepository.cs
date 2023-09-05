using BlogApi.Domain.Entities;

namespace BlogApi.Application.Interfaces
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        public Task AddAsync(TEntity entity);
        public Task<IEnumerable<TEntity>> GetAllAsync();
        public Task<TEntity> GetByIdAsync(int id);
        public Task UpdateAsync(TEntity entity);

    }
}
