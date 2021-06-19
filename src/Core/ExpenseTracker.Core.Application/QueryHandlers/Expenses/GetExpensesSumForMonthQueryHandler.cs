using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ExpenseTracker.Core.Application.Interfaces;
using ExpenseTracker.Core.Application.Queries.ExpenseQueries;
using ExpenseTracker.Core.Domain.Dtos.Expenses;
using ExpenseTracker.Core.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Core.Application.QueryHandlers.Expenses
{
    public class GetExpensesSumForMonthQueryHandler : IRequestHandler<GetExpensesSumForMonthQuery, IEnumerable<ExpensesSumDto>>
    {
        private readonly IGenericRepository<Expense> _expenseRepository;

        public GetExpensesSumForMonthQueryHandler(IGenericRepository<Expense> expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }
        
        public async Task<IEnumerable<ExpensesSumDto>> Handle(GetExpensesSumForMonthQuery request,
            CancellationToken cancellationToken)
        {
            var expenses = await _expenseRepository.Read()
                .Include(x => x.Wallet)
                .Where(x => x.OwnerId == request.UserId
                            && x.Date.Year == request.Date.Year
                            && x.Date.Month == request.Date.Month
                )
                .GroupBy(x => x.WalletId)
                .Select(x => new ExpensesSumDto
                {
                    CurrencyCode = x.Select(e => e.Wallet.CurrencyCode).FirstOrDefault(),
                    Sum = x.Sum(e => e.Money)
                })
                .ToListAsync(cancellationToken: cancellationToken);

            return expenses;
        }
    }
}