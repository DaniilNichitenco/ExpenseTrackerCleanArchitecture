using ExpenseTracker.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Infrastructure.Repository.API.EFConfiguration;

namespace ExpenseTracker.Infrastructure.Repository.API
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
