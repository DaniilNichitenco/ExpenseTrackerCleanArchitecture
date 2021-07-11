using System;

namespace ExpenseTracker.Core.Domain.ViewModels
{
    public class ExpenseViewModel
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public Guid? TopicId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public double Money { get; set; }
    }
}