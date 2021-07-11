using ExpenseTracker.Core.Application.Interfaces;
using ExpenseTracker.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Infrastructure.Repository.API.API;

namespace ExpenseTracker.Infrastructure.Repository.API.Extensions
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
                    var topicRepository = services.GetRequiredService<IGenericRepository<Topic>>();
                    var walletRepository = services.GetRequiredService<IGenericRepository<Wallet>>();
                    var expenseRepository = services.GetRequiredService<IGenericRepository<Expense>>();

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
