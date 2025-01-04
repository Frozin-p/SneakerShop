using AuthService.Contracts;

namespace AuthService.Abstractions
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username); // Найти пользователя по имени
        Task<User?> GetUserByEmailAsync(string email); // Найти пользователя по email
        Task AddUserAsync(User user); // Добавить нового пользователя
    }
}
