using BlogApi.Application.Interfaces;
using BlogApi.Domain.Entities;
using BlogApi.Infrastructure.Database;

namespace BlogApi.Infrastructure.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(BlogDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<Role> GetRoleByName(string name) => await FirstOrDefaultAsync(r => r.Name == name);
    }
}
