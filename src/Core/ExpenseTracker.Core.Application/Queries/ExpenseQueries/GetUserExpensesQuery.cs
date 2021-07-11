using System;
using System.Collections.Generic;
using System.Security.Claims;
using ExpenseTracker.Core.Domain.Dtos.Expenses;
using ExpenseTracker.Core.Domain.Entities;
using MediatR;

namespace ExpenseTracker.Core.Application.Queries.ExpenseQueries
{
    public class GetUserExpensesQuery : IRequest<IEnumerable<ExpenseDto>>
    {
        public Guid UserId { get; set; }
    }
}