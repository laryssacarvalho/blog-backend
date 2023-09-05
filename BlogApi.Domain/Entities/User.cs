namespace BlogApi.Domain.Entities
{
    public class User : Entity
    {
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string PasswordSalt { get; private set; }
        public List<Role> Roles { get; set; }

        public User(string email, string passwordHash, string passwordSalt)
        {
            Email = email;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            Roles = new();
        }

        public void AddRole(Role role) => Roles.Add(role);
    }
}
