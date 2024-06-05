using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

namespace BeatsShop.Models.Account;

public class LoginViewModel : BaseModel
{
    [Required]
    [Display(Name = "UserName")]
    public string? UserName { get; set; }

    [Required]
    [Display(Name = "Password")]
    public string? Password { get; set; }

    [Display(Name = "Remember login?")]
    public bool RememberLogin { get; set; }

    public string? ReturnUrl { get; set; }

    public IEnumerable<AuthenticationScheme>? ExternalLogins { get; set; }
}