using BeatsShop.DAL.DomainModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeatsShop.DAL.Contexts;

public sealed class ApplicationContext : IdentityDbContext<User, Role, int>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }
}