using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Core.Domain.Entities
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public long OwnerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
