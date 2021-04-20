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
    public class GetExpensesSumForDayQueryHandler : IRequestHandler<GetExpensesSumForDayQuery, IEnumerable<ExpensesSumDto>>
    {
        private readonly IEFRepository<Expense> _expenseRepository;

        public GetExpensesSumForDayQueryHandler(IEFRepository<Expense> expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<IEnumerable<ExpensesSumDto>> Handle(GetExpensesSumForDayQuery request,
            CancellationToken cancellationToken)
        {
            var expenses = await _expenseRepository.Read()
                .Include(x => x.Wallet)
                .Where(x => x.OwnerId == request.UserId
                            && x.Date.Date == request.Date.Date
                )
                .GroupBy(x => x.WalletId)
                .Select(x => new ExpensesSumDto
                {
                    CurrencyCode = x.Select(e => e.Wallet.CurrencyCode).FirstOrDefault(),
                    Sum = x.Sum(e => e.Money)
                }).ToListAsync(cancellationToken: cancellationToken);
            
            return expenses;
        }
    }
}