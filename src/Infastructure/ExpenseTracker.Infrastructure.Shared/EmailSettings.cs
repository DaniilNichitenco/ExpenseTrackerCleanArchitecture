using ExpenseTracker.Core.Application.Interfaces;

namespace ExpenseTracker.Infrastructure.Shared
{
    public class EmailSettings : IEmailSettings
    {
        public string Server { get; set; }
        public string Port { get; set; }

    }
}
