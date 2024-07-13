using XtraBlog.Models;

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
    Task<AppUser> FindUserAsync();
}
