using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_4_Authentication_Authorization.Helper;
using Task_4_Core.Contracts;
using Task_4_Core.Entities;

namespace Task_4_Authentication_Authorization.Controllers;

[Authorize]
public class UsersController : Controller
{
    private readonly IUserManager _userManager;
    private readonly UserProvider _userProvider;

    public UsersController(IUserManager userManager, UserProvider userProvider)
    {
        _userManager = userManager;
        _userProvider = userProvider;
    }

    public async Task<IActionResult> Profile()
    {
        var userId = _userProvider.UserId;
        var user = await _userManager.GetUserByIdAsync(userId);

        if (user == null)
        {
            _userProvider.UserLogOut();
            return RedirectToAction("Register", "Account");
        }

        if (user.UserRole != UserRole.Admin && user.IsBlocked)
        {
            _userProvider.UserLogOut();
            return RedirectToAction("AccessDenied", "Account", new { userName = user.UserName});
        }

        return View(user);
    }

    public async Task<IActionResult> GetAllUsers()
    {
        var userId = _userProvider.UserId;
        var user = await _userManager.GetUserByIdAsync(userId);

        if (user is not null && user.UserRole != UserRole.Admin && user.IsBlocked)
        {
            _userProvider.UserLogOut();
            return RedirectToAction("AccessDenied", "Account", new{userName = user.UserName});
        }

        var allUsers = await _userManager.GetAllUsersAsync();

        return View(allUsers);
    }

    public async Task<IActionResult> BlockUsers(List<Guid> selectedUsers)
    {
        await _userManager.BlockUsers(selectedUsers);
        return RedirectToAction(nameof(GetAllUsers));
    }

    public async Task<IActionResult> UnblockUsers(List<Guid> selectedUsers)
    {
        await _userManager.UnBlockUsers(selectedUsers);

        return RedirectToAction(nameof(GetAllUsers));
    }

    public async Task<IActionResult> DeleteUsers(List<Guid> selectedUsers)
    {
        await _userManager.DeleteUsers(selectedUsers);
        return RedirectToAction(nameof(GetAllUsers));
    }
}


