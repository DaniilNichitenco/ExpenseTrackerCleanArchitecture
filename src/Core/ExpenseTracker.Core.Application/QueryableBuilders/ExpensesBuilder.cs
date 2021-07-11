using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExpenseTracker.Core.Domain.Dtos.Expenses;
using ExpenseTracker.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Core.Application.QueryableBuilders
{
    public class ExpensesBuilder
    {
        private IQueryable<Expense> _expenses;

        public ExpensesBuilder()
        {
        }

        public ExpensesBuilder(IQueryable<Expense> expenses)
        {
            _expenses = expenses;
        }

        public ExpensesBuilder SetExpenses(IQueryable<Expense> expenses)
        {
            _expenses = expenses;
            
            return this;
        }

        public async Task<IEnumerable<ExpensesSumDto>> BuildExpensesSum(CancellationToken cancellationToken = default)
        {
            return await _expenses.GroupBy(x => x.WalletId)
                .Select(x => new ExpensesSumDto
                {
                    CurrencyCode = x.Select(e => e.Wallet.CurrencyCode).FirstOrDefault(),
                    Sum = x.Sum(e => e.Money)
                })
                .ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<ExpensesSumPerDayDto>> BuildExpensesSumPerDayAsync(int countDays, CancellationToken cancellationToken = default)
        {
            var expensesDictionary= await _expenses.GroupBy(x => x.WalletId)
                .ToDictionaryAsync(g => g.FirstOrDefault().Wallet.CurrencyCode, 
                    g => g.GroupBy(e => e.Date.Day)
                        .Select(e => new ExpensesSumForDayDto() { Day = e.Key, Sum = e.Sum(exp => exp.Money)}).ToList(),
                    cancellationToken);

            var expenses = await BuildExpensesSumForEmptyDaysAsync(expensesDictionary, countDays);
            
            var result = expenses.Select(x => new ExpensesSumPerDayDto
            {
                CurrencyCode = x.Key,
                Expenses = x.Value
            });

            return result;
        }

        private static async Task<Dictionary<string, List<ExpensesSumForDayDto>>> BuildExpensesSumForEmptyDaysAsync(Dictionary<string, List<ExpensesSumForDayDto>> expenses, int countDays)
        {
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

            return expenses;
        }
    }
}