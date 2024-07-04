using XtraBlog.Models.Common;

namespace XtraBlog.Models;

public class Tag : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public ICollection<BlogTag> BlogTags { get; set; } = new List<BlogTag>();
}
