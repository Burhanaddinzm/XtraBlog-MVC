using XtraBlog.Models;
using XtraBlog.ViewModels.Tag;

namespace XtraBlog.Services.Interfaces;

public interface ITagService
{
    Task<List<Tag>> GetAllTagsAsync(AppUser user);
    Task CreateTagAsync(CreateTagVM tagVM, AppUser user);
    Task<bool> CheckDuplicateAsync(string tagName, string userId, int? tagId = null);
}
