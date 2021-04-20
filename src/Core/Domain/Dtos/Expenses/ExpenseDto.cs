using System;
using ExpenseTracker.Core.Domain.Entities;

namespace ExpenseTracker.Core.Domain.Dtos.Expenses
{
    public class ExpenseDto
    {
        public long Id { get; set; }
        public long WalletId { get; set; }
        public long? TopicId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public double Money { get; set; }
    }
}