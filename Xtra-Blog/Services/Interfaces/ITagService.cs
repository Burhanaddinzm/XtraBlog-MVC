using XtraBlog.Models;
using XtraBlog.ViewModels.Tag;

namespace XtraBlog.Services.Interfaces;

public interface ITagService
{
    Task<List<Tag>> GetAllTagsAsync(AppUser user);
    Task CreateTagAsync(UpdateTagVM tagVM, AppUser user);
    Task<bool> CheckDuplicateAsync(string tagName, string userId, int? tagId = null);
    Task<Tag?> GetTagAsync(int id);
    Task UpdateTagAsync(UpdateTagVM tagVM);
    Task DeleteTagAsync(Tag tag);
}
