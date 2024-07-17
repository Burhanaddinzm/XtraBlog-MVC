using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
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
    private readonly IUserService _userService;

    public BlogManager(
        AppDbContext context,
        IWebHostEnvironment env,
        IUserService userService)
    {
        _context = context;
        _env = env;
        _userService = userService;
    }

    public async Task<List<Blog>> GetAllBlogsAsync(string? createdBy = null)
    {
        var query = _context.Blogs.Include("BlogTags.Tag")
                                  .Include("Comments");
        return createdBy != null
            ? await query.Where(x => x.CreatedBy == createdBy).ToListAsync()
            : await query.ToListAsync();
    }

    public async Task<Blog?> GetBlogAsync(int id)
    {
        return await _context.Blogs.FindAsync(id);
    }

    public async Task CreateBlogAsync(CreateBlogVM blogVM, AppUser user)
    {
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

    public async Task UpdateBlogAsync(UpdateBlogVM blogVM, AppUser user)
    {
        var existingBlog = await _context.Blogs.FindAsync(blogVM.Id);

        if (existingBlog == null)
        {
            throw new Exception("Blog doesn't exist!");
        }

        if (existingBlog.User != user)
        {
            throw new UnauthorizedAccessException("Only author of the blog can update it!");
        }

        existingBlog.Title = blogVM.Title;
        existingBlog.Content = blogVM.Content;
        if (blogVM.Image != null)
        {
            var fileName = await blogVM.Image.SaveFileAsync(_env.WebRootPath, "img");
            existingBlog.ImageUrl = fileName;
        }

        _context.Blogs.Update(existingBlog);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CheckDuplicateAsync(string blogTitle, int? blogId = null)
    {
        Blog? existingBlog;

        if (blogId != null)
        {
            existingBlog = await _context.Blogs.FirstOrDefaultAsync(
                x => x.Title.Trim().ToLower() == blogTitle.Trim().ToLower() &&
                x.Id != blogId
                );
        }
        else
        {
            existingBlog = await _context.Blogs.FirstOrDefaultAsync(
                x => x.Title.Trim().ToLower() == blogTitle.Trim().ToLower()
                );
        }

        return existingBlog != null;
    }
}
