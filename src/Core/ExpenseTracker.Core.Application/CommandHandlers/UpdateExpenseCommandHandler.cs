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
    public class UpdateExpenseCommandHandler : BaseExpenseCommandHandler, IRequestHandler<UpdateExpenseCommand, Guid>
    {
        private readonly IGenericRepository<Expense> _expenseRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateExpenseCommandHandler(IMediator mediator, IGenericRepository<Expense> expenseRepository,
            IMapper mapper, IHttpContextAccessor httpContextAccessor) :
            base(mediator)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Guid> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            var expense = await _expenseRepository.GetByIdAsync(request.Id, cancellationToken);

            if (expense == null || userId == null || expense.OwnerId != new Guid(userId))
            {
                throw new InvalidOperationException($"Cannot update expense with id: {request.Id}");
            }
            
            expense = _mapper.Map<Expense>(request);
            
            _expenseRepository.Update(expense);
            await _expenseRepository.SaveChangesAsync();
            
            await PostHandle();

            return expense.Id;
        }
    }
}