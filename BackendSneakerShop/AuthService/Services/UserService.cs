using AuthService.Abstractions;
using AuthService.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<User?> AuthenticateUser(string username, string password)
    {
        var user = await _repository.GetUserByUsernameAsync(username);
        if (user == null) return null;

        var hashedPassword = HashPassword(password);
        return user.PasswordHash == hashedPassword ? user : null;
    }

    public async Task RegisterUser(User user)
        {
            if (!PasswordValidator.IsValid(user.PasswordHash, out string errorMessage))
            {
                throw new InvalidOperationException(errorMessage);
            }

        user.PasswordHash = HashPassword(user.PasswordHash);
        await _repository.AddUserAsync(user);
        }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
