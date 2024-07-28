using Microsoft.AspNetCore.Mvc;
using XtraBlog.Models;
using XtraBlog.Services.Interfaces;
using XtraBlog.ViewModels.Tag;

namespace XtraBlog.Controllers;

public class TagController : Controller
{
    private readonly ITagService _tagService;
    private readonly IUserService _userService;

    public TagController(ITagService tagService, IUserService userService)
    {
        _tagService = tagService;
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(UpdateTagVM tagVM)
    {
        var user = await _userService.FindCurrentUserAsync();
        if (!ModelState.IsValid)
        {
            return View(tagVM);
        }

        if (await _tagService.CheckDuplicateAsync(tagVM.Name, user.Id))
        {
            ModelState.AddModelError("Name", "Tag already exists!");
            return View(tagVM);
        }

        await _tagService.CreateTagAsync(tagVM, user);

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Update(int? id)
    {
        if (id == null || id <= 0)
        {
            return BadRequest("Invalid id!");
        }

        var tag = await _tagService.GetTagAsync(id.Value);

        if (tag == null)
        {
            return NotFound("Tag not found!");
        }

        var currentUser = await _userService.FindCurrentUserAsync();

        if (currentUser != tag.User && currentUser.UserName != "Admin")
        {
            return Unauthorized();
        }

        return View(new UpdateTagVM { Id = tag.Id, Name = tag.Name });
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateTagVM tagVM)
    {
        if (!ModelState.IsValid)
        {
            return View(tagVM);
        }

        var user = await _userService.FindCurrentUserAsync();

        if (await _tagService.CheckDuplicateAsync(tagVM.Name, user.Id, tagVM.Id))
        {
            ModelState.AddModelError("Name", "Tag already exists!");
            return View(tagVM);
        }

        await _tagService.UpdateTagAsync(tagVM);

        return RedirectToAction(nameof(Create));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || id <= 0)
        {
            return BadRequest("Invalid id!");
        }

        var tag = await _tagService.GetTagAsync(id.Value);

        if (tag == null)
        {
            return NotFound("Tag not found!");
        }

        var currentUser = await _userService.FindCurrentUserAsync();

        if (currentUser != tag.User && currentUser.UserName != "Admin")
        {
            return Unauthorized();
        }

        await _tagService.DeleteTagAsync(tag);

        var referrerUrl = Request.Headers["Referer"].ToString();
        if (!string.IsNullOrEmpty(referrerUrl))
        {
            return Redirect(referrerUrl);
        }

        return RedirectToAction(nameof(Create));
    }
}
