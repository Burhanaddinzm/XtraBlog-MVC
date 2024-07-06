using Microsoft.AspNetCore.Mvc;
using XtraBlog.Extensions.File;
using XtraBlog.Services.Interfaces;
using XtraBlog.ViewModels.Blog;

namespace XtraBlog.Controllers;

public class BlogController : Controller
{
    private readonly IBlogService _blogService;

    public BlogController(IBlogService blogService)
    {
        _blogService = blogService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var blogs = await _blogService.GetAllBlogsAsync();

        //if (blogs == null || blogs.Count == 0)
        //{
        //    return NotFound();
        //}

        return View(blogs);
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int? id)
    {
        return View();
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBlogVM blogVM)
    {
        if (!ModelState.IsValid)
        {
            return View(blogVM);
        }

        if (blogVM.Image != null)
        {
            var validationResult = blogVM.Image.ValidateFile();

            if (!validationResult.IsValid)
            {
                ModelState.AddModelError("Image", validationResult.ErrorMessage);
                return View(blogVM);
            }
        }

        try
        {
            await _blogService.CreateBlogAsync(blogVM);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int? id)
    {
        return View();
    }
}
