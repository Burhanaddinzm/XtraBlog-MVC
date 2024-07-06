using XtraBlog.Models.Common;

namespace XtraBlog.Models;

public class BlogTag : BaseEntity
{
    public int BlogId { get; set; }
    public Blog Blog { get; set; } = null!;
    public int TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}
