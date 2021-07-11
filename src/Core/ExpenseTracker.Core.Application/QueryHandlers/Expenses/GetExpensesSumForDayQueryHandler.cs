using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExpenseTracker.Core.Application.Interfaces;
using ExpenseTracker.Core.Application.Queries.ExpenseQueries;
using ExpenseTracker.Core.Application.QueryableBuilders;
using ExpenseTracker.Core.Domain.Dtos.Expenses;
using ExpenseTracker.Core.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Core.Application.QueryHandlers.Expenses
{
    public class GetExpensesSumForDayQueryHandler : IRequestHandler<GetExpensesSumForDayQuery, IEnumerable<ExpensesSumDto>>
    {
        private readonly IGenericRepository<Expense> _expenseRepository;
        private readonly ExpensesBuilder _expensesBuilder;

        public GetExpensesSumForDayQueryHandler(IGenericRepository<Expense> expenseRepository,
            ExpensesBuilder expensesBuilder)
        {
            _expenseRepository = expenseRepository;
            _expensesBuilder = expensesBuilder;
        }

        public async Task<IEnumerable<ExpensesSumDto>> Handle(GetExpensesSumForDayQuery request,
            CancellationToken cancellationToken)
        {
            var expenses = _expenseRepository.Read()
                .Include(x => x.Wallet)
                .Where(x => x.OwnerId == request.UserId
                            && x.Date.Date == request.Date.Date
                );
            
            var result = await _expensesBuilder
                .SetExpenses(expenses)
                .BuildExpensesSum(cancellationToken);
            
            return result;
        }
    }
}