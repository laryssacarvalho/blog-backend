namespace BlogApi.Application.Interfaces
{
    public interface IAuthService
    {
        public Task<string> Login(string email, string password);
        public Task<int> AddNewPublicUser(string email, string password);
    }
}
