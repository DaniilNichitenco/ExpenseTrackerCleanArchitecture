using AutoMapper;
using ExpenseTracker.Core.Domain.Dtos.Expenses;
using ExpenseTracker.Core.Domain.Entities;
using ExpenseTracker.Core.Domain.ViewModels;

namespace ExpenseTracker.Core.Domain.AutoMapperProfiles
{
    public class ExpenseProfile : Profile
    {
        public ExpenseProfile()
        {
            CreateMap<Expense, ExpenseDto>();
            CreateMap<ExpenseDto, ExpenseViewModel>();
            
            CreateMap<ExpensesSumDto, ExpensesSumViewModel>();

            CreateMap<ExpensesSumForDayDto, ExpensesSumForDayViewModel>();
            CreateMap<ExpensesSumPerDayDto, ExpensesSumPerDayViewModel>();
        }
    }
}