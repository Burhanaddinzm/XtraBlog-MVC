using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using XtraBlog.Data;
using XtraBlog.Extensions.File;
using XtraBlog.Models;
using XtraBlog.Services.Interfaces;
using XtraBlog.ViewModels.Blog;

namespace XtraBlog.Services.Implementations;

public class BlogManager : IBlogService
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly IHttpContextAccessor _accessor;
    private readonly UserManager<AppUser> _userManager;

    public BlogManager(
        AppDbContext context,
        IWebHostEnvironment env,
        IHttpContextAccessor accessor,
        UserManager<AppUser> userManager)
    {
        _context = context;
        _env = env;
        _accessor = accessor;
        _userManager = userManager;
    }

    public async Task<List<Blog>> GetAllBlogsAsync()
    {
        return await _context.Blogs.Include("BlogTags.Tag")
                                   .Include("Comments")
                                   .ToListAsync();
    }

    public async Task CreateBlogAsync(CreateBlogVM blogVM)
    {
        if (_accessor?.HttpContext?.User?.Identity?.Name == null)
        {
            throw new Exception("User not found!");
        }

        var user = await _userManager.FindByNameAsync(_accessor.HttpContext.User.Identity.Name!);

        if (user == null)
        {
            throw new Exception("Error occured while searching for user!");
        }

        var blog = new Blog
        {
            Title = blogVM.Title,
            Content = blogVM.Content,
            UserId = user.Id,
        };

        if (blogVM.Image != null)
        {
            var fileName = await blogVM.Image.SaveFileAsync(_env.WebRootPath, "img");
            blog.ImageUrl = fileName;
        }

        await _context.Blogs.AddAsync(blog);
        await _context.SaveChangesAsync();
    }
}
