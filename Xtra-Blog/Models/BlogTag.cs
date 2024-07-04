namespace XtraBlog.Models;

public class BlogTag
{
    public int BlogId { get; set; }
    public Blog Blog { get; set; } = null!;
    public int TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}
