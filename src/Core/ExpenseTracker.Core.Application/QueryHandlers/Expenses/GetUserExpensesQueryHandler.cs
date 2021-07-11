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
    public class GetUserExpensesQueryHandler : IRequestHandler<GetUserExpensesQuery, IEnumerable<ExpenseDto>>
    {
        private readonly IGenericRepository<Expense> _expenseRepository;
        private readonly IMapper _mapper;

        public GetUserExpensesQueryHandler(IGenericRepository<Expense> expenseRepository, 
            IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<ExpenseDto>> Handle(GetUserExpensesQuery request,
            CancellationToken cancellationToken)
        {
            var expenses = await _expenseRepository.Read().Where(x => x.OwnerId == request.UserId)
                .ToListAsync(cancellationToken: cancellationToken);
            var result = _mapper.Map<List<ExpenseDto>>(expenses);
        
            return result;
        }
    }
}