using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace BlogApi.Application.Helpers;

public static class PasswordHelper
{
    public static string HashPassword(string password, string salt)
    {
        var byteSalt = Encoding.UTF8.GetBytes(salt);

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password!,
            salt: byteSalt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        return hashed;
    }

    public static string GenerateSalt()
    {
        using var rng = RandomNumberGenerator.Create();
        
        var byteSalt = new byte[16];
        
        rng.GetBytes(byteSalt);

        var salt = Convert.ToBase64String(byteSalt);

        return salt;
    }
}

