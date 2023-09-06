using BlogApi.Domain.Entities;

namespace BlogApi.Application.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> GetUserByEmail(string email);
        public Task AddUserAsync(User user);
    }
}
