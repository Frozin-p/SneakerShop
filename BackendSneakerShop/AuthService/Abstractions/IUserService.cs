using AuthService.Contracts;

namespace AuthService.Abstractions;

public interface IUserService
{
    Task<User?> AuthenticateUser(string username, string password); // Аутентификация пользователя
    Task RegisterUser(User user); // Регистрация нового пользователя
}
