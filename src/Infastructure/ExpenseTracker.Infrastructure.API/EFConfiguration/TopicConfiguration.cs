using ExpenseTracker.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.Repository.API.EFConfiguration
{
    public class TopicConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(40);

            builder.HasMany(e => e.Expenses)
                .WithOne(e => e.Topic)
                .HasForeignKey(e => e.TopicId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
