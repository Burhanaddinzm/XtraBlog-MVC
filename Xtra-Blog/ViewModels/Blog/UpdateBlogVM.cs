using System.ComponentModel;

namespace XtraBlog.ViewModels.Blog;

public class UpdateBlogVM
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    [DisplayName("Blog Image")]
    public IFormFile? Image { get; set; }
    public string? ImageUrl { get; set; }
    public List<int> SelectedTags { get; set; } = new List<int>();
}
