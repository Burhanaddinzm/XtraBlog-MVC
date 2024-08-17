using Microsoft.AspNetCore.Identity;
using XtraBlog.Models;
using XtraBlog.ViewModels.Auth;

namespace XtraBlog.Services.Interfaces;

public interface IUserService
{
    bool CheckLoggedIn();
    Task<AppUser> FindCurrentUserAsync();
    Task<IdentityResult> Create(RegisterVM registerVM);
    Task<bool> CheckDuplicate(string email, string username);
    Task<(AppUser?, ICollection<string>?)> CheckExistance(string email);
}
