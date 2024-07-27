using Microsoft.AspNetCore.Mvc;
using XtraBlog.Services.Interfaces;
using XtraBlog.ViewModels.Tag;

namespace XtraBlog.Controllers
{
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
        public async Task<IActionResult> Create(CreateTagVM tagVM)
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

            return RedirectToAction("Create", "Tag");
        }
    }
}
