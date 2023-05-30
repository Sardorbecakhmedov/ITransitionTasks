using Microsoft.EntityFrameworkCore;
using Task_4_Core.ApplicationDbContext;
using Task_4_Core.Contracts;
using Task_4_Core.Entities;
using Task_4_Core.ViewModels;

namespace Task_4_Core.Repositories;

public class UserManager : IUserManager
{
    private readonly AppDbContext _appDbContext;

    public UserManager(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<User> RegisterAsync(RegisterViewModel model, string? userRole = null)
    {
        var user = new User
        {
            UserName = model.UserName,
            Email = model.Email,
            Password = model.Password,
            IsBlocked = false,
            UserRole = UserRole.User,
            RegistrationDate = DateTime.UtcNow,
            LastLoginDate = DateTime.UtcNow
        };

        if (userRole is not null)
        {
            user.UserRole = userRole;
        }

        await _appDbContext.Users.AddAsync(user);
        await _appDbContext.SaveChangesAsync();

        return user;
    }

    public async Task<User?> LoginAsync(LoginViewModel model)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(user => user.UserName == model.UserName);

        if (user == null || user.Password != model.Password)
        {
            return null;
        }

        user.LastLoginDate = DateTime.UtcNow;
        await _appDbContext.SaveChangesAsync();

        return user;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        var users = await _appDbContext.Users.ToListAsync();
        return users;
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

        return user ?? null;
    }

    public async Task BlockUsers(List<Guid> selectedUsers)
    {
        foreach (var userId in selectedUsers)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);

            if (user is not null && user.UserRole != UserRole.Admin)
            {
                user.IsBlocked = true;
            }
        }

        await _appDbContext.SaveChangesAsync();
    }

    public async Task UnBlockUsers(List<Guid> selectedUsers)
    {
        foreach (var userId in selectedUsers)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);

            if (user is not null)
            {
                user.IsBlocked = false;
            }
        }

        await _appDbContext.SaveChangesAsync();
    }

    public async Task DeleteUsers(List<Guid> selectedUsers)
    {
        foreach (var userId in selectedUsers)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);

            if (user is not null && user.UserRole != UserRole.Admin)
            {
                _appDbContext.Remove(user);
            }
        }
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<bool> HasAdmin()
    {
        return await _appDbContext.Users.AnyAsync(user => user.UserRole == UserRole.Admin);
    }
}