using System;
using System.Collections.Generic;
using ExpenseTracker.Core.Domain.Dtos.Expenses;
using MediatR;

namespace ExpenseTracker.Core.Application.Queries.ExpenseQueries
{
    public class GetExpensesSumPerDayForMonth : IRequest<IEnumerable<ExpensesSumPerDayDto>>
    {
        public long UserId { get; set; }
        public DateTime Date { get; set; }
    }
}