using ExpenseTracker.Core.Application.Interfaces;
using ExpenseTracker.Core.Domain.Entities;
using ExpenseTracker.Infrastructure.API.API;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.API.Extensions
{
    public static class HostExtensions
    {
        public static async Task SeedData(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var topicRepository = services.GetRequiredService<IEFRepository<Topic>>();
                    var walletRepository = services.GetRequiredService<IEFRepository<Wallet>>();
                    var expenseRepository = services.GetRequiredService<IEFRepository<Expense>>();

                    var context = services.GetRequiredService<ExpenseTrackerDbContext>();

                    context.Database.EnsureCreated();

                    await Seed.SeedTopics(topicRepository);
                    await Seed.SeedWallets(walletRepository);
                    await Seed.SeedExpenses(expenseRepository);
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
