using Microsoft.AspNetCore.Mvc;
using XtraBlog.Models;
using XtraBlog.Services.Interfaces;
using XtraBlog.ViewModels.Tag;

namespace XtraBlog.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTagVM tagVM)
        {
            if (!ModelState.IsValid)
            {
                return View(tagVM);
            }

            if (await _tagService.CheckDuplicateAsync(tagVM.Name))
            {
                ModelState.AddModelError("Name", "Tag already exists!");
                return View(tagVM);
            }

            await _tagService.CreateTagAsync(tagVM);

            return RedirectToAction("Index", "Blog");
        }
    }
}
