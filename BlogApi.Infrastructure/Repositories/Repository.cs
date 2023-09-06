using BlogApi.Domain.Entities;
using BlogApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace BlogApi.Infrastructure.Repositories;

public abstract class Repository<TEntity> where TEntity : Entity
{
    private readonly BlogDbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    protected async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
        var query = _dbSet.AsQueryable();

        if (include is not null)
            query = include(query);

        return await query.FirstOrDefaultAsync(expression);
    }

    protected async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
        var query = _dbSet.AsQueryable();

        query = query.Where(expression);

        if (include is not null)
            query = include(query);

        return await query.ToListAsync();
    }

    protected async Task AddAsync(TEntity entity)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    protected async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();    
    
    protected async Task UpdateAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Update(entity);
        await _dbContext.SaveChangesAsync();
    }
}