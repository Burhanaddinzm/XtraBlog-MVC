using XtraBlog.Models;
using XtraBlog.ViewModels.Tag;

namespace XtraBlog.Services.Interfaces;

public interface ITagService
{
    Task<List<Tag>> GetAllTagsAsync();
    Task CreateTagAsync(CreateTagVM tagVM);
    Task<bool> CheckDuplicateAsync(string tagName, int? tagId = null);
}
