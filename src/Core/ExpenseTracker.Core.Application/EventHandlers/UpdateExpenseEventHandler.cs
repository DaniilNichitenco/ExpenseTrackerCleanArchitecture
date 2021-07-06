using System;
using System.Threading;
using System.Threading.Tasks;
using ExpenseTracker.Core.Application.Events;
using ExpenseTracker.Core.Application.Queries.ExpenseQueries;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ExpenseTracker.Core.Application.EventHandlers
{
    public class UpdateExpenseEventHandler : INotificationHandler<ExpenseUpdatedEvent>
    {
        private readonly IHubContext<ChartHub> _hubContext;
        private readonly IMediator _mediator;

        public UpdateExpenseEventHandler(IHubContext<ChartHub> hubContext, IMediator mediator)
        {
            _hubContext = hubContext;
            _mediator = mediator;
        }
        
        public async Task Handle(ExpenseUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var clientProxy = _hubContext.Clients.User(notification.UserId);

            var updateChart1 = clientProxy.SendAsync("UpdateExpensesSumForDay",
                await _mediator.Send(new GetExpensesSumForDayQuery() {UserId = new Guid(notification.UserId)},
                    cancellationToken), cancellationToken);
            var updateChart2 = clientProxy.SendAsync("UpdateExpensesSumForMonth",
                await _mediator.Send(new GetExpensesSumForMonthQuery() {UserId = new Guid(notification.UserId)},
                    cancellationToken), cancellationToken);
            var updateChart3 = clientProxy.SendAsync("UpdateExpensesSumForYear",
                await _mediator.Send(new GetExpensesSumForYearQuery() {UserId = new Guid(notification.UserId)},
                    cancellationToken), cancellationToken);
            var updateChart4 = clientProxy.SendAsync("UpdateExpensesPerDayForMonth",
                await _mediator.Send(new GetExpensesSumPerDayForMonth() {UserId = new Guid(notification.UserId)},
                    cancellationToken), cancellationToken);

            await Task.WhenAll(updateChart1, updateChart2, updateChart3, updateChart4);
        }
    }
}