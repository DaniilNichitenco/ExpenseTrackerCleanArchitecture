using System;

namespace ExpenseTracker.Core.Application.Commands
{
    public class UpdateExpenseCommand : BaseCreateUpdateExpenseCommand
    {
        public Guid Id { get; set; }
    }
}