using System;
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
    public class GetExpensesSumPerDayForMonthHandler : IRequestHandler<GetExpensesSumPerDayForMonth, IEnumerable<ExpensesSumPerDayDto>>
    {
        private readonly IGenericRepository<Expense> _expenseRepository;
        private readonly ExpensesBuilder _expensesBuilder;

        public GetExpensesSumPerDayForMonthHandler(IGenericRepository<Expense> expenseRepository,
            ExpensesBuilder expensesBuilder)
        {
            _expenseRepository = expenseRepository;
            _expensesBuilder = expensesBuilder;
        }

        public async Task<IEnumerable<ExpensesSumPerDayDto>> Handle(GetExpensesSumPerDayForMonth request,
            CancellationToken cancellationToken)
        {
            var countDays = DateTime.DaysInMonth(request.Date.Year, request.Date.Month);

            var expenses = _expenseRepository.Read()
                .Include(x => x.Wallet)
                .Where(x => x.OwnerId == request.UserId
                            && x.Date.Month == request.Date.Month
                            && x.Date.Year == request.Date.Year
                );

            var result = await _expensesBuilder
                .SetExpenses(expenses)
                .BuildExpensesSumPerDayAsync(countDays, cancellationToken);

            return result;
        }
    }
}