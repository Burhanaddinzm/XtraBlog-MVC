using System.ComponentModel.DataAnnotations;
using XtraBlog.Models.Common;

namespace XtraBlog.Models;

public class Tag : BaseAuditableEntity
{
    [MaxLength(20)]
    public string Name { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public AppUser User { get; set; } = null!;
    public ICollection<BlogTag> BlogTags { get; set; } = new List<BlogTag>();
}
