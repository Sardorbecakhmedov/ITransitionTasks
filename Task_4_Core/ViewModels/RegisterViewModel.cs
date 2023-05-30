using System.ComponentModel.DataAnnotations;

namespace Task_4_Core.ViewModels;

public class RegisterViewModel
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}