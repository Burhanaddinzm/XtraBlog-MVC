using System.ComponentModel.DataAnnotations;

namespace XtraBlog.ViewModels.Tag;

public class CreateTagVM
{
    [MaxLength(20)]
    public string Name { get; set; } = null!;
}
