using Microsoft.AspNetCore.Identity;

namespace BeatsShop.DAL.DomainModels;

public class Role : IdentityRole<int>
{
    public ICollection<User> Users { get; set; }
}