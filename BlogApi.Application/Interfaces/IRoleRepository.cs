using BlogApi.Domain.Entities;

namespace BlogApi.Application.Interfaces
{
    public interface IRoleRepository
    {
        public Task<Role> GetRoleByName(string name);
    }
}
