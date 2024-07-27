using Microsoft.AspNetCore.Mvc;
using XtraBlog.Services.Interfaces;

namespace XtraBlog.ViewComponents;

public class TagViewComponent : ViewComponent
{
    private readonly ITagService _tagService;
    private readonly IUserService _userService;

    public TagViewComponent(ITagService tagService, IUserService userService)
    {
        _tagService = tagService;
        _userService = userService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await _userService.FindCurrentUserAsync();
        var tags = await _tagService.GetAllTagsAsync(user);
        return View(tags);
    }
}
