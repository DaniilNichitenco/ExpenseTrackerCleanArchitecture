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
    public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(p => p.Date)
                .IsRequired();

            builder.Property(p => p.Money)
                .HasDefaultValue(0)
                .IsRequired();
        }
    }
}
