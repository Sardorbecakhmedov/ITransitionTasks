using Task_4_Core.Entities;
using Task_4_Core.ViewModels;

namespace Task_4_Core.Contracts;

public interface IUserManager
{
    Task<User> RegisterAsync(RegisterViewModel model, string? userRole = null);
    Task<User?> LoginAsync(LoginViewModel model);
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(Guid userId);
    Task BlockUsers(List<Guid> selectedUsers);
    Task UnBlockUsers(List<Guid> selectedUsers);
    Task DeleteUsers(List<Guid> selectedUsers);

    Task<bool> HasAdmin();
}