using System;
using ExpenseTracker.Core.Domain.Entities;

namespace ExpenseTracker.Core.Domain.Dtos.Expenses
{
    public class ExpenseDto
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public Guid? TopicId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public double Money { get; set; }
    }
}