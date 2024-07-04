using XtraBlog.Models.Common;

namespace XtraBlog.Models;

public class Comment : BaseAuditableEntity
{
    public string Content { get; set; } = null!;
    public int BlogId { get; set; }
    public Blog Blog { get; set; } = null!;
    public Guid UserId { get; set; }
    public int? ParentId { get; set; }
    public Comment? Parent { get; set; } 
    public ICollection<Comment> Children { get; set; } = new List<Comment>();
}
