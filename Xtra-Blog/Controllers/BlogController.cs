using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XtraBlog.Extensions.File;
using XtraBlog.Services.Interfaces;
using XtraBlog.ViewModels.Blog;

namespace XtraBlog.Controllers;
[Authorize]
public class BlogController : Controller
{
    private readonly IBlogService _blogService;
    private readonly IUserService _userService;
    private readonly ITagService _tagService;

    public BlogController(
        IBlogService blogService,
        IUserService userService,
        ITagService tagService)
    {
        _blogService = blogService;
        _userService = userService;
        _tagService = tagService;
    }
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (_userService.CheckLoggedIn())
        {
            var user = await _userService.FindCurrentUserAsync();
            ViewBag.UserId = user.Id;
            ViewBag.UserName = user.UserName;
        }

        var blogs = await _blogService.GetAllBlogsAsync();

        return View(blogs);
    }

    [HttpGet]
    public async Task<IActionResult> MyBlogs()
    {
        var user = await _userService.FindCurrentUserAsync();
        var blogs = await _blogService.GetAllBlogsAsync(user.UserName);

        return View(blogs);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Detail(int? id)
    {
        if (id == null || id <= 0)
        {
            return BadRequest("Invalid id!");
        }

        var blog = await _blogService.GetBlogAsync(id.Value);

        if (blog == null)
        {
            return NotFound("Blog not found!");
        }

        return View(blog);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var user = await _userService.FindCurrentUserAsync();
        ViewBag.Tags = await _tagService.GetAllTagsAsync(user);
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBlogVM blogVM)
    {
        var user = await _userService.FindCurrentUserAsync();
        ViewBag.Tags = await _tagService.GetAllTagsAsync(user);

        if (!ModelState.IsValid)
        {
            return View(blogVM);
        }

        if (await _blogService.CheckDuplicateAsync(blogVM.Title))
        {
            ModelState.AddModelError("Title", "This blog title already exists!");
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

        await _blogService.CreateBlogAsync(blogVM, user);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Update(int? id)
    {
        if (id == null || id <= 0)
        {
            return BadRequest("Invalid id!");
        }

        var blog = await _blogService.GetBlogAsync(id.Value);

        if (blog == null)
        {
            return NotFound("Blog not found!");
        }

        var currentUser = await _userService.FindCurrentUserAsync();

        if (currentUser != blog.User && currentUser.UserName != "Admin")
        {
            return Unauthorized();
        }

        var blogVm = new UpdateBlogVM
        {
            Id = blog.Id,
            Title = blog.Title,
            Content = blog.Content,
            ImageUrl = blog.ImageUrl,
            SelectedTags = blog.BlogTags.Select(bt => bt.TagId).ToList()
        };

        var user = await _userService.FindCurrentUserAsync();
        ViewBag.Tags = await _tagService.GetAllTagsAsync(user);

        return View(blogVm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateBlogVM blogVM)
    {
        var user = await _userService.FindCurrentUserAsync();
        ViewBag.Tags = await _tagService.GetAllTagsAsync(user);

        if (!ModelState.IsValid)
        {
            return View(blogVM);
        }

        if (await _blogService.CheckDuplicateAsync(blogVM.Title, blogVM.Id))
        {
            ModelState.AddModelError("Title", "This blog title already exists!");
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

        await _blogService.UpdateBlogAsync(blogVM, user);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || id <= 0)
        {
            return BadRequest("Invalid id!");
        }

        var blog = await _blogService.GetBlogAsync(id.Value);

        if (blog == null)
        {
            return NotFound("Blog not found!");
        }

        var currentUser = await _userService.FindCurrentUserAsync();

        if (currentUser != blog.User && currentUser.UserName != "Admin")
        {
            return Unauthorized();
        }

        await _blogService.DeleteBlogAsync(blog);

        var referrerUrl = Request.Headers["Referer"].ToString();
        if (!string.IsNullOrEmpty(referrerUrl))
        {
            return Redirect(referrerUrl);
        }

        return RedirectToAction("Index");
    }
}
