using System.Collections.Generic;
using ExpenseTracker.Core.Domain.Dtos.Expenses;

namespace ExpenseTracker.Core.Domain.ViewModels
{
    public class ExpensesSumPerDayViewModel
    {
        public string CurrencyCode { get; set; }
        public  IEnumerable<ExpensesSumForDayDto> Expenses { get; set; }
    }
}