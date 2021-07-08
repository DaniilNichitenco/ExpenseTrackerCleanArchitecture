using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ExpenseTracker.Core.Application.Commands;
using ExpenseTracker.Core.Application.Interfaces;
using ExpenseTracker.Core.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Core.Application.CommandHandlers
{
    public class CreateExpenseCommandHandler : BaseExpenseCommandHandler, IRequestHandler<CreateExpenseCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Expense> _expenseRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateExpenseCommandHandler(IMediator mediator, IMapper mapper,
            IGenericRepository<Expense> expenseRepository,
            IHttpContextAccessor httpContextAccessor) : base(mediator)
        {
            _mapper = mapper;
            _expenseRepository = expenseRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Guid> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            var expense = _mapper.Map<Expense>(request);

            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;

            expense.OwnerId = new Guid(userId);

            await _expenseRepository.AddAsync(expense, cancellationToken);
            await _expenseRepository.SaveChangesAsync();
            
            await PostHandle();

            return expense.Id;
        }
    }
}