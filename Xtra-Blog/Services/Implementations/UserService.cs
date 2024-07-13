using Microsoft.AspNetCore.Identity;
using XtraBlog.Models;
using XtraBlog.Services.Interfaces;

namespace XtraBlog.Services.Implementations;

public class UserService : IUserService
{
    IHttpContextAccessor _accessor;
    UserManager<AppUser> _userManager;

    public UserService(
        IHttpContextAccessor accessor,
        UserManager<AppUser> userManager)
    {
        _accessor = accessor;
        _userManager = userManager;
    }

    public bool CheckLoggedIn()
    {
        return _accessor.HttpContext!.User != null && _accessor.HttpContext.User.Identity!.IsAuthenticated;
    }

    public async Task<AppUser> FindUserAsync()
    {
        var userName = _accessor?.HttpContext?.User?.Identity?.Name;
        if (userName == null)
        {
            throw new ArgumentException("User not found!");
        }

        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            throw new Exception($"Can't find user named \"${userName}\"!");
        }

        return user;
    }
}
