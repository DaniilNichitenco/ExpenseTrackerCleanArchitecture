using System.Threading.Tasks;
using ExpenseTracker.Core.Application.Events;
using MediatR;

namespace ExpenseTracker.Core.Application.CommandHandlers
{
    public class BaseExpenseCommandHandler
    {
        private readonly IMediator _mediator;

        public BaseExpenseCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected async Task PostHandle()
        {
            await _mediator.Publish(new ExpenseUpdatedEvent());
        }
    }
}