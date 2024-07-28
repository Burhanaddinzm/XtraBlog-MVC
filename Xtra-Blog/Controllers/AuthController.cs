using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using XtraBlog.Models;
using XtraBlog.Services.Interfaces;
using XtraBlog.ViewModels.Auth;

namespace XtraBlog.Controllers;
public class AuthController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserService _userService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IEmailSender _emailSender;

    public AuthController(
        IUserService userService,
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        IEmailSender emailSender)
    {
        _userService = userService;
        _signInManager = signInManager;
        _userManager = userManager;
        _emailSender = emailSender;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM loginVM)
    {
        if (!ModelState.IsValid)
        {
            return View(loginVM);
        }

        var (user, roles) = await _userService.CheckExistance(loginVM.Email);

        if (user == null)
        {
            ModelState.AddModelError("", "Invalid Credentials");
            return View(loginVM);
        }

        var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);

        if (!result.Succeeded)
        {
            if (!result.IsLockedOut && !result.IsNotAllowed) ModelState.AddModelError("", "Invalid Credentials!");
            if (result.IsNotAllowed) ModelState.AddModelError("", "Email is not confirmed!");
            if (result.IsLockedOut) ModelState.AddModelError("", "You have been locked out, try again in 5 minutes!");
            return View(loginVM);
        }

        //if (roles!.Contains("Admin"))
        //{
        //    return RedirectToAction("Index", "Dashboard", new { Area = "Admin" });
        //}
        return RedirectToAction("Index", "Blog");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        if (!ModelState.IsValid)
        {
            return View(registerVM);
        }

        if (await _userService.CheckDuplicate(registerVM.Email, registerVM.UserName))
        {
            ModelState.AddModelError("", "User with this email or username already exists!");
            return View(registerVM);
        }

        var result = await _userService.Create(registerVM);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(registerVM);
        }

        // Send email confirmation
        var user = await _userManager.FindByEmailAsync(registerVM.Email);
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user!);
        var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { userId = user!.Id, token });
        Console.WriteLine(confirmationLink);
        await _emailSender.SendEmailAsync(registerVM.Email, "Confirm your email", confirmationLink!);

        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Blog");
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string? userId, string? token)
    {
        if (userId == null || token == null)
        {
            return BadRequest("Invalid email confirmation request!");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found!");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
        {
            return BadRequest("Email confirmation failed!");
        }

        return RedirectToAction("Index", "Blog");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return Forbid();
    }
}
