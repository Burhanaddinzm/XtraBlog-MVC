using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace XtraBlog.ViewModels.Auth;

public class RegisterVM
{
    [DisplayName("User Name")]
    public string UserName { get; set; } = null!;
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    [DisplayName("Confirm Password")]
    [DataType(DataType.Password), Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = null!;
}
