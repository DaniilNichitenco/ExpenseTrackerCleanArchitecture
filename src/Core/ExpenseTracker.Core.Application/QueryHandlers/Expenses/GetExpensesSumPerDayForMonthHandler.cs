using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExpenseTracker.Core.Application.Interfaces;
using ExpenseTracker.Core.Application.Queries.ExpenseQueries;
using ExpenseTracker.Core.Domain.Dtos.Expenses;
using ExpenseTracker.Core.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Core.Application.QueryHandlers.Expenses
{
    public class GetExpensesSumPerDayForMonthHandler : IRequestHandler<GetExpensesSumPerDayForMonth, IEnumerable<ExpensesSumPerDayDto>>
    {
        private readonly IEFRepository<Expense> _expenseRepository;

        public GetExpensesSumPerDayForMonthHandler(IEFRepository<Expense> expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }
        public async Task<IEnumerable<ExpensesSumPerDayDto>> Handle(GetExpensesSumPerDayForMonth request, CancellationToken cancellationToken)
        {
            var countDays = DateTime.DaysInMonth(request.Date.Year, request.Date.Month);
            
            var expenses = await _expenseRepository.Read()
                .Include(x => x.Wallet)
                .Where(x => x.OwnerId == request.UserId
                            && x.Date.Month == request.Date.Month
                            && x.Date.Year == request.Date.Year
                )
                .GroupBy(x => x.WalletId)
                .ToDictionaryAsync(g => g.FirstOrDefault().Wallet.CurrencyCode, 
                    g => g.GroupBy(e => e.Date.Day)
                    .Select(e => new ExpensesSumForDayDto() { Day = e.Key, Sum = e.Sum(exp => exp.Money)}).ToList(),
                    cancellationToken);

            foreach (var g in expenses)
            {
                for (var i = 1; i <= countDays; i++)
                {
                    if (g.Value.All(e => e.Day != i))
                    {
                        g.Value.Add(new ExpensesSumForDayDto() {Day = i, Sum = 0});
                    }
                }

                g.Value.Sort((f, s) => f.Day.CompareTo(s.Day));
            }

            var result = expenses.Select(x => new ExpensesSumPerDayDto
            {
                CurrencyCode = x.Key,
                Expenses = x.Value
            });

            return result;
        }
    }
}