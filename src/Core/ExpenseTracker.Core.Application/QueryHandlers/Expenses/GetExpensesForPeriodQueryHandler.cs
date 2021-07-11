using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExpenseTracker.Core.Application.Queries.ExpenseQueries;
using ExpenseTracker.Core.Domain.Dtos.Expenses;
using ExpenseTracker.Core.Domain.Enums;
using MediatR;

namespace ExpenseTracker.Core.Application.QueryHandlers.Expenses
{
    public class GetExpensesForPeriodQueryHandler : IRequestHandler<GetExpensesForPeriodQuery, IEnumerable<ExpensesSumDto>>
    {
        private readonly
            Dictionary<ExpensesForPeriod, Func<Guid, DateTime, CancellationToken, Task<IEnumerable<ExpensesSumDto>>>>
            _expensesSumBuilderFunctions;
        private readonly IMediator _mediator;
        
        public GetExpensesForPeriodQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
            _expensesSumBuilderFunctions = new Dictionary<ExpensesForPeriod, Func<Guid, DateTime, CancellationToken, Task<IEnumerable<ExpensesSumDto>>>>
            {
                {ExpensesForPeriod.Day, GetExpensesSumForDay},
                {ExpensesForPeriod.Month, GetExpensesSumForMonth},
                {ExpensesForPeriod.Year, GetExpensesSumForYear}
            };
        }

        public async Task<IEnumerable<ExpensesSumDto>> Handle(GetExpensesForPeriodQuery request, CancellationToken cancellationToken)
        {
            if (!_expensesSumBuilderFunctions.TryGetValue(request.ExpensesForPeriod, out var builderFunc))
            {
                throw new ArgumentException($"There is not function for type ${request.ExpensesForPeriod}");
            }

            return await builderFunc.Invoke(request.UserId, request.Date, cancellationToken);
        }

        private async Task<IEnumerable<ExpensesSumDto>> GetExpensesSumForDay(Guid userId, DateTime date, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetExpensesSumForDayQuery
            {
                UserId = userId,
                Date = date
            }, cancellationToken);
        }
        
        private async Task<IEnumerable<ExpensesSumDto>> GetExpensesSumForMonth(Guid userId, DateTime date, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetExpensesSumForMonthQuery
            {
                UserId = userId,
                Date = date
            }, cancellationToken);
        }
        
        private async Task<IEnumerable<ExpensesSumDto>> GetExpensesSumForYear(Guid userId, DateTime date, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetExpensesSumForYearQuery
            {
                UserId = userId,
                Date = date
            }, cancellationToken);
        }
    }
}