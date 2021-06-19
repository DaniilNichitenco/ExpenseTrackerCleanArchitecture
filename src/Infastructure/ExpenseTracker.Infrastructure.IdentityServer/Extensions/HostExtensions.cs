using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ExpenseTracker.Core.Domain.Auth;

namespace ExpenseTracker.Infrastructure.Repository.IdentityServer.Extensions
{
    public static class HostExtensions
    {
        public static async Task SeedData(this IHost host)
        {
            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var configurationContext = services.GetRequiredService<ConfigurationDbContext>();
                    var persistedGrantContext = services.GetRequiredService<PersistedGrantDbContext>();
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var roleManager = services.GetRequiredService<RoleManager<Role>>();

                    await Seed.SeedIdentityRoles(roleManager);
                    await Seed.SeedIdentityUsers(userManager);
                    await Seed.SeedIdentityServer(configurationContext, persistedGrantContext);
                }
                catch (Exception exception)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(exception, "An error occured during migration");
                }
            }
        }
    }
}
