using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Task_4_Authentication_Authorization.Helper;

public class UserProvider
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UserProvider(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    private HttpContext Context => _contextAccessor.HttpContext!;

    public Guid UserId => Guid.Parse(Context.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public bool IsLogin()
    {
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return userId is not null;
    }

    public void UserLogOut()
    {
        Context.SignOutAsync();
    }

}