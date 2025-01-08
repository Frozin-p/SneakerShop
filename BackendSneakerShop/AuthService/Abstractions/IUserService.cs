using AuthService.Contracts;

namespace AuthService.Abstractions;

public interface IUserService
{
    Task<User?> AuthenticateUser(string username, string password);
    Task RegisterUser(User user);
}
