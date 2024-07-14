using Microsoft.AspNetCore.Identity;
using XtraBlog.Enums;
using XtraBlog.Models;
using XtraBlog.Services.Interfaces;
using XtraBlog.ViewModels.Auth;

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

    public async Task<IdentityResult> Create(RegisterVM registerVM)
    {
        AppUser newUser = new AppUser
        {
            UserName = registerVM.UserName,
            Email = registerVM.Email
        };

        var result = await _userManager.CreateAsync(newUser, registerVM.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(newUser, Roles.Bloger.ToString());
        }
        return result;
    }

    public async Task<bool> CheckDuplicate(string email, string username)
    {
        return await _userManager.FindByEmailAsync(email) != null
               || await _userManager.FindByNameAsync(username) != null;
    }

    public bool CheckLoggedIn()
    {
        return _accessor.HttpContext!.User != null && _accessor.HttpContext.User.Identity!.IsAuthenticated;
    }

    public async Task<AppUser> FindCurrentUserAsync()
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

    public async Task<(AppUser?, ICollection<string>?)> CheckExistance(string email)
    {
        ICollection<string> roles = new List<string>();

        var user = await _userManager.FindByEmailAsync(email);
        if (user != null) roles = await _userManager.GetRolesAsync(user);

        return (user, roles);
    }
}
