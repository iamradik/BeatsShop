using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

namespace BeatsShop.Models.Account;

public class RegisterViewModel : BaseModel
{
    [Required]
    [Display(Name = "UserName")]
    public string? UserName { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string? Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string? ConfirmPassword { get; set; }

    [DataType(DataType.PhoneNumber)]
    [Display(Name = "PhoneNumber")]
    public string? PhoneNumber { get; set; }

    public string? ReturnUrl { get; set; }

    [Display(Name = "Remember login?")]
    public bool RememberLogin { get; set; }

    public IEnumerable<AuthenticationScheme>? ExternalLogins { get; set; }

}