using System.ComponentModel.DataAnnotations;

namespace XtraBlog.ViewModels.Tag;

public class UpdateTagVM
{
    public int Id { get; set; }
    [MaxLength(20)]
    public string Name { get; set; } = null!;
}
