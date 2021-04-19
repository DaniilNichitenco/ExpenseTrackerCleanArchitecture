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

namespace ExpenseTracker.Core.Application.QueryHandlers
{
    public class GetUserExpensesQueryHandler : IRequestHandler<GetUserExpensesQuery, IEnumerable<ExpenseDto>>
    {
        private readonly IEFRepository<Expense> _expenseRepository;
        private readonly IMapper _mapper;

        public GetUserExpensesQueryHandler(IEFRepository<Expense> expenseRepository, 
            IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<ExpenseDto>> Handle(GetUserExpensesQuery request,
            CancellationToken cancellationToken)
        {
            var idClaim = request.User.Claims.FirstOrDefault(x => x.Type == "id");
            if (idClaim == null)
            {
                return null;
            }
            
            var id = long.Parse(idClaim.Value);
            var expenses = await _expenseRepository.Read().Where(x => x.OwnerId == id).ToListAsync(cancellationToken: cancellationToken);
            var result = _mapper.Map<List<ExpenseDto>>(expenses);
        
            return result;
        }
    }
}