using BlogApi.Application.Interfaces;
using BlogApi.Domain.Entities;
using BlogApi.Infrastructure.Database;

namespace BlogApi.Infrastructure
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(BlogDbContext dbContext) : base(dbContext)
        {

        }
        public async Task<Role> GetRoleByName(string name)
        {
            return await FirstOrDefaultAsync(r => r.Name == name);
        }
    }
}
