using System.Collections.Generic;
using System.Security.Claims;
using ExpenseTracker.Core.Domain.Dtos.Expenses;
using ExpenseTracker.Core.Domain.Entities;
using MediatR;

namespace ExpenseTracker.Core.Application.Queries.ExpenseQueries
{
    public class GetUserExpensesQuery : IRequest<IEnumerable<ExpenseDto>>
    {
        public ClaimsPrincipal User { get; }

        public GetUserExpensesQuery(ClaimsPrincipal user) => User = user;
    }
}