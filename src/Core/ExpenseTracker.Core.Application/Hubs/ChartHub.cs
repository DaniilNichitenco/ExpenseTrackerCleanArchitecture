using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Core.Domain.ViewModels;
using Microsoft.AspNetCore.SignalR;

namespace ExpenseTracker.Core.Application
{
    public class ChartHub : Hub
    {
        public async Task UpdateChartExpensesSumForDay(string userId, IEnumerable<ExpensesSumViewModel> expenses)
        {
            await Clients.User(userId).SendAsync("UpdateExpensesSumForDay", expenses);
        }
        
        public async Task UpdateChartExpensesSumForMonth(string userId, IEnumerable<ExpensesSumViewModel> expenses)
        {
            await Clients.User(userId).SendAsync("UpdateExpensesSumForMonth", expenses);
        }
        
        public async Task UpdateChartExpensesSumForYear(string userId, IEnumerable<ExpensesSumViewModel> expenses)
        {
            await Clients.User(userId).SendAsync("UpdateExpensesSumForYear", expenses);
        }
        
        public async Task UpdateChartExpensesPerDayForMonth(string userId, IEnumerable<ExpensesSumPerDayViewModel> expenses)
        {
            await Clients.User(userId).SendAsync("UpdateExpensesPerDayForMonth", expenses);
        }
    }
}