using XtraBlog.Models;
using XtraBlog.ViewModels.Blog;

namespace XtraBlog.Services.Interfaces;

public interface IBlogService
{
    Task<List<Blog>> GetAllBlogsAsync();
    Task CreateBlogAsync(CreateBlogVM blogVM);
}
