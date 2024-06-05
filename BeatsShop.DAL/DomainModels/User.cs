using Microsoft.AspNetCore.Identity;

namespace BeatsShop.DAL.DomainModels;

public class User : IdentityUser<int>
{
    public int? RoleId { get; set; }
    public Role Role { get; set; }
}