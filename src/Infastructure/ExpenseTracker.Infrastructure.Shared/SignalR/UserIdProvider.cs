using ExpenseTracker.Infrastructure.Repository.Shared.Extensions;
using Microsoft.AspNetCore.SignalR;

namespace ExpenseTracker.Infrastructure.Repository.Shared.SignalR
{
    public class UserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User.GetClaim("id")?.Value;
        }
    }
}