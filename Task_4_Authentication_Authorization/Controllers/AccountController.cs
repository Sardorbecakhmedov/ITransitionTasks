using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_4_Core.Contracts;
using Task_4_Core.Entities;
using Task_4_Core.ViewModels;

namespace Task_4_Authentication_Authorization.Controllers;

public class AccountController : Controller
{
    private readonly IUserManager _userManager;

    public AccountController(IUserManager userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
    {
        var user = await _userManager.RegisterAsync(model);

        await AddClaimsToCookie(user);

        return RedirectToAction("Profile", "Users");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromForm] LoginViewModel model)
    {
        var user = await _userManager.LoginAsync(model);

        if (user == null)
            return RedirectToAction("Register");

        if (user.UserRole != UserRole.Admin &&  user.IsBlocked)
        {
            return RedirectToAction(nameof(AccessDenied), new { userName = user.UserName });
        }

        await AddClaimsToCookie(user);
        return RedirectToAction("Profile", "Users");
    }


    [Authorize]
    public IActionResult Logout()
    {
        HttpContext.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }

    public IActionResult AccessDenied(string userName)
    {
        ViewBag.Username = userName;
        return View();
    }

    private async Task AddClaimsToCookie(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var claimPrincipal = new ClaimsPrincipal(claimIdentity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal);
    }
}