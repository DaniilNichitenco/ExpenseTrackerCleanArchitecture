using ExpenseTracker.Core.Domain.Entities;
using ExpenseTracker.Infrastructure.API.EFConfiguration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.API
{
    public class ExpenseTrackerDbContext : DbContext
    {
        public ExpenseTrackerDbContext(DbContextOptions options) : base(options)
        {
        }

        public ExpenseTrackerDbContext()
        {
        }

        DbSet<Topic> Topics { get; set; }
        DbSet<Expense> Expenses { get; set; }
        DbSet<Wallet> Wallets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var assembly = typeof(ExpenseConfiguration).Assembly;

            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }
    }
}
