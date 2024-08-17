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

    /// <summary>
    /// Checks if user with this username or email already exists.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="username"></param>
    /// <returns>True if user with given params already exists, otherwise false.</returns>
    public async Task<bool> CheckDuplicate(string email, string username)
    {
        return await _userManager.FindByEmailAsync(email) != null
               || await _userManager.FindByNameAsync(username) != null;
    }

    /// <summary>
    /// Checks if user is logged in or not.
    /// </summary>
    /// <returns>True if the user is logged in, otherwise false.</returns>
    public bool CheckLoggedIn()
    {
        return _accessor.HttpContext!.User != null && _accessor.HttpContext.User.Identity!.IsAuthenticated;
    }

    /// <summary>
    /// Finds current <see cref="AppUser"/> of this session.
    /// </summary>
    /// <returns><see cref="AppUser"/> of current session.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="Exception"></exception>
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
