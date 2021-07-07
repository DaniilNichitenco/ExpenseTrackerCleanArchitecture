using ExpenseTracker.Core.Domain.Enums;

namespace ExpenseTracker.Core.Application.Queries.ExpenseQueries
{
    public class GetExpensesForPeriodQuery : BaseExpensesForPeriodQuery
    {
        public ExpensesForPeriod ExpensesForPeriod { get; set; }
    }
}