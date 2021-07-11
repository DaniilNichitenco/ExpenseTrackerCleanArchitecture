using System;
using MediatR;

namespace ExpenseTracker.Core.Application.Commands
{
    public class DeleteEntityCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}