using System;
using MediatR;

namespace ExpenseTracker.Core.Application.Commands
{
    public class BaseCreateUpdateExpenseCommand : IRequest<Guid>
    {
        public Guid WalletId { get; set; }
        public Guid TopicId { get; set; }
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public double Money { get; set; }
    }
}