using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExpenseTracker.Core.Application.Commands;
using ExpenseTracker.Core.Application.Interfaces;
using ExpenseTracker.Core.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Core.Application.CommandHandlers
{
    public class DeleteExpenseCommandHandler : BaseExpenseCommandHandler, IRequestHandler<DeleteEntityCommand, bool>
    {
        private readonly IGenericRepository<Expense> _expenseRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteExpenseCommandHandler(IMediator mediator, IGenericRepository<Expense> expenseRepository,
            IHttpContextAccessor httpContextAccessor) : base(mediator)
        {
            _expenseRepository = expenseRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Handle(DeleteEntityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
                var expense = await _expenseRepository.GetByIdAsync(request.Id, cancellationToken);

                if (!string.IsNullOrWhiteSpace(userId) && expense.OwnerId == new Guid(userId))
                {
                    _expenseRepository.Delete(expense);
                }
                
                var affectedRows = await _expenseRepository.SaveChangesAsync();

                if (affectedRows > 0)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            
            await PostHandle();

            return true;
        }
    }
}