using MediatR;

namespace ExpenseTracker.Core.Application.Events
{
    public class ExpenseUpdatedEvent : INotification
    {
        public string UserId { get; set; }
    }
}