using XtraBlog.Models.Common;

namespace XtraBlog.Models;

public class Blog : BaseAuditableEntity
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public AppUser User { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<BlogTag> BlogTags { get; set; } = new List<BlogTag>();
}
