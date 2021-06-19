using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Domain.Entities
{
    public class Topic : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
    }
}
