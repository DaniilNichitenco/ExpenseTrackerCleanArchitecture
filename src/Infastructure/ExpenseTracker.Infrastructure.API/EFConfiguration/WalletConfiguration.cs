using ExpenseTracker.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.API.EFConfiguration
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.Property(p => p.Bill)
                .IsRequired();

            builder.Property(p => p.CurrencyCode)
                .IsRequired()
                .HasMaxLength(3)
                .HasColumnType("char(3)");

            builder.Property(p => p.RowVersion)
                .IsRowVersion();

            builder.HasMany(p => p.Expenses)
                .WithOne(e => e.Wallet)
                .HasForeignKey(e => e.WalletId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
