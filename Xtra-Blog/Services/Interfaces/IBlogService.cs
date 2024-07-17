using XtraBlog.Models;
using XtraBlog.ViewModels.Blog;

namespace XtraBlog.Services.Interfaces;

public interface IBlogService
{
    Task<List<Blog>> GetAllBlogsAsync(string? createdBy = null);
    Task<Blog?> GetBlogAsync(int id);
    Task CreateBlogAsync(CreateBlogVM blogVM, AppUser user);
    Task<bool> CheckDuplicateAsync(string blogTitle, int? blogId = null);

    /// <summary>
    /// Updates <see cref="Blog"/> with the id of <see cref="UpdateBlogVM.Id"/>. Replaces old values with the values provided by the blogVM.
    /// </summary>
    /// <param name="blogVM"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="Exception"></exception>
    Task UpdateBlogAsync(UpdateBlogVM blogVM, AppUser user);
}
