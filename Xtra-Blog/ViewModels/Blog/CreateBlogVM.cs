using System.ComponentModel;

namespace XtraBlog.ViewModels.Blog;

public class CreateBlogVM
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    [DisplayName("Blog Image")]
    public IFormFile? Image { get; set; }
}
