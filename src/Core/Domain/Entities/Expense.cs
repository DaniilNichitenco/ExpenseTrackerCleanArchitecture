using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Domain.Entities
{
    public class Expense : BaseEntity
    {
        public long WalletId { get; set; }
        public virtual Wallet Wallet { get; set; }
        public long? TopicId { get; set; }
        public virtual Topic Topic { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public double Money { get; set; }
    }
}
