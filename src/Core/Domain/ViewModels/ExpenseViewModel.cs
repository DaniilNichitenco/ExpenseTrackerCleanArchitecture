using System;

namespace ExpenseTracker.Core.Domain.ViewModels
{
    public class ExpenseViewModel
    {
        public long Id { get; set; }
        public long WalletId { get; set; }
        public long? TopicId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public double Money { get; set; }
    }
}