using XtraBlog.Models;
using XtraBlog.ViewModels.Blog;

namespace XtraBlog.Services.Interfaces;

public interface IBlogService
{
    Task<List<Blog>> GetAllBlogsAsync(string? createdBy = null);
    Task<Blog?> GetBlogAsync(int id);
    Task CreateBlogAsync(CreateBlogVM blogVM, AppUser user);
    Task<bool> CheckDuplicateAsync(string blogTitle, int? blogId = null);
    Task UpdateBlogAsync(UpdateBlogVM blogVM, AppUser user);
    Task DeleteBlogAsync(Blog blog);
}
