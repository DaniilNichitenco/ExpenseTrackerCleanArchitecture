using System.Collections.Generic;

namespace ExpenseTracker.Core.Domain.Dtos.Expenses
{
    public class ExpensesSumPerDayDto
    {
        public string CurrencyCode { get; set; }
        public  IEnumerable<ExpensesSumForDayDto> Expenses { get; set; }
    }
}