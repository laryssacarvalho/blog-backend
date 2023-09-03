namespace BlogApi.Domain.Entities
{
    public class User : Entity
    {
        public string Email { get; private set; }
        public string Password { get; private set; }
        
        public User(string email, string password)
        {
            Email = email;
            Password = password;
        }

    }
}
