using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Domain.Entities
{
    public class Wallet : BaseEntity
    {
        public double Bill { get; set; }
        public string CurrencyCode { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
