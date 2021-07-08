using AutoMapper;
using ExpenseTracker.Core.Application.Commands;
using ExpenseTracker.Core.Domain.Entities;

namespace ExpenseTracker.Infrastructure.API.AutoMapperProfiles
{
        public class ExpenseProfile : Profile
        {
            public ExpenseProfile()
            {
                CreateMap<CreateExpenseCommand, Expense>();
                CreateMap<UpdateExpenseCommand, Expense>();
            }
        }
}