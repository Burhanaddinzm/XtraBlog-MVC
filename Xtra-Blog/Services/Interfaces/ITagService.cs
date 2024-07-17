using XtraBlog.Models;

namespace XtraBlog.Services.Interfaces;

public interface ITagService
{
    Task<List<Tag>> GetAllTagsAsync();
}
