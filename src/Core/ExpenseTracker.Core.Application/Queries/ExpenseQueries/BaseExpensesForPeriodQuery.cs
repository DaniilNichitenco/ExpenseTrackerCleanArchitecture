using System;
using System.Collections.Generic;
using ExpenseTracker.Core.Domain.Dtos.Expenses;
using MediatR;

namespace ExpenseTracker.Core.Application.Queries.ExpenseQueries
{
    public abstract class BaseExpensesForPeriodQuery : IRequest<IEnumerable<ExpensesSumDto>>
    {
        public Guid UserId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}