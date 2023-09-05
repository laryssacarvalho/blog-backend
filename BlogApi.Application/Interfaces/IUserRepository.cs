using BlogApi.Domain.Entities;

namespace BlogApi.Application.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<User> GetUserByEmail(string email);
    }
}
