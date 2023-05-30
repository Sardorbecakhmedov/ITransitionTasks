using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Task_4_Core.Contracts;
using Task_4_Core.Entities;
using Task_4_Core.ViewModels;

namespace Task_4_Authentication_Authorization.Controllers;

public class HomeController : Controller
{
    private readonly IUserManager _userManager;

    public HomeController(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        if (!await _userManager.HasAdmin())
        {
            var userModel = new RegisterViewModel
            {
                UserName = "Admin",
                Email = "Admin@gmailcom",
                Password = "Admin",
            };

            await _userManager.RegisterAsync(userModel, UserRole.Admin);
        }

        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

