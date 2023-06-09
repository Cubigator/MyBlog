using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace MyBlog.Services;

public class EncryptorService
{
    public string HashPassword(string password, string salt)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2
                                        (
                                        password, 
                                        Encoding.Unicode.GetBytes(salt),
                                        KeyDerivationPrf.HMACSHA512,
                                        5000,
                                        64
                                        ));
    }
}
