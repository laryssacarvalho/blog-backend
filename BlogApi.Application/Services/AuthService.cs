using BlogApi.Application.Helpers;
using BlogApi.Application.Interfaces;
using BlogApi.Application.Settings;
using BlogApi.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogApi.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<AuthService> _logger;        
        private readonly AppSettings _settings;

        public AuthService(IUserRepository userRepository, ILogger<AuthService> logger, IOptions<AppSettings> settings, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _logger = logger;
            _settings = settings.Value;
            _roleRepository = roleRepository;
        }

        public async Task<string> Login(string email, string password)
        {            
            var user = await _userRepository.GetUserByEmail(email);

            if (user is null)
                throw new Exception("Invalid email or password");

            var passwordHash = PasswordHelper.HashPassword(password, user.PasswordSalt);

            if (user.PasswordHash != passwordHash)
                throw new Exception("Invalid email or password");

            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Email, user.Email),
               new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in user.Roles)            
                authClaims.Add(new Claim(ClaimTypes.Role, userRole.Name));
            
            return GenerateToken(authClaims);
        }

        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _settings.ValidIssuer,
                Audience = _settings.ValidAudience,
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<int> AddNewPublicUser(string email, string password)
        {
            var salt = PasswordHelper.GenerateSalt();            

            var newUser = new User(email, PasswordHelper.HashPassword(password, salt), salt);
            
            var role = await _roleRepository.GetRoleByName("Public");
            newUser.AddRole(role);

            await _userRepository.AddAsync(newUser);
            
            return newUser.Id;
        }
    }

}
