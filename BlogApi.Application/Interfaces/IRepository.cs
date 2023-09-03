using BlogApi.Domain.Entities;

namespace BlogApi.Application.Interfaces
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        public IEnumerable<TEntity> Find();
        public Task AddAsync(TEntity entity);
        public Task UpdateAsync(TEntity entity);
        public Task SaveAsync();

    }
}
