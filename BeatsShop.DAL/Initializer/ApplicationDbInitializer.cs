using BeatsShop.DAL.Contexts;
using BeatsShop.DAL.DomainModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BeatsShop.DAL.Initializer;

public class ApplicationDbInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public ApplicationDbInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var provider = scope.ServiceProvider;
        await using var context = provider.GetRequiredService<ApplicationContext>();
        await context.Database.MigrateAsync(cancellationToken);
        if (!context.Roles.Any())
        {
            var roleManager = provider.GetRequiredService<RoleManager<Role>>();
            await roleManager.CreateAsync(new Role { Name = "Admin", NormalizedName = "ADMIN" });
            await roleManager.CreateAsync(new Role { Name = "User", NormalizedName = "USER" });
            await roleManager.CreateAsync(new Role { Name = "Moderator", NormalizedName = "MODERATOR" });
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}