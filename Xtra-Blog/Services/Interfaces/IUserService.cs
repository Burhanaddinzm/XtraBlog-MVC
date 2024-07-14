using Microsoft.AspNetCore.Identity;
using XtraBlog.Models;
using XtraBlog.ViewModels.Auth;

namespace XtraBlog.Services.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Checks if user is logged in or not.
    /// </summary>
    /// <returns>True if the user is logged in, otherwise false.</returns>
    bool CheckLoggedIn();

    /// <summary>
    /// Finds current <see cref="AppUser"/> of this session.
    /// </summary>
    /// <returns><see cref="AppUser"/> of current session.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="Exception"></exception>
    Task<AppUser> FindCurrentUserAsync();

    Task<IdentityResult> Create(RegisterVM registerVM);

    /// <summary>
    /// Checks if user with this username or email already exists.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="username"></param>
    /// <returns>True if user with given params already exists, otherwise false.</returns>
    Task<bool> CheckDuplicate(string email, string username);

    Task<(AppUser?, ICollection<string>?)> CheckExistance(string email);
}
