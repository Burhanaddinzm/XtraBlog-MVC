using Microsoft.AspNetCore.Mvc;
using XtraBlog.Extensions.File;
using XtraBlog.Services.Interfaces;
using XtraBlog.ViewModels.Blog;

namespace XtraBlog.Controllers;

public class BlogController : Controller
{
    private readonly IBlogService _blogService;
    private readonly IUserService _userService;

    public BlogController(IBlogService blogService, IUserService userService)
    {
        _blogService = blogService;
        _userService = userService;
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

        try
        {
            var user = await _userService.FindCurrentUserAsync();
            await _blogService.CreateBlogAsync(blogVM, user);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

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

        try
        {
            var currentUser = await _userService.FindCurrentUserAsync();

            if (currentUser != blog.User)
            {
                return Unauthorized();
            }
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

        var blogVm = new UpdateBlogVM
        {
            Id = blog.Id,
            Title = blog.Title,
            Content = blog.Content,
            ImageUrl = blog.ImageUrl,
        };

        return View(blogVm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateBlogVM blogVM)
    {
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

        try
        {
            var user = await _userService.FindCurrentUserAsync();
            await _blogService.UpdateBlogAsync(blogVM, user);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int? id)
    {
        return RedirectToAction(nameof(Index));
    }
}
