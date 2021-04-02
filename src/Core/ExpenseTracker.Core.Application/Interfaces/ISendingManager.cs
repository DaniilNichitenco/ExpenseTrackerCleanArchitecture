using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Application.Interfaces
{
    public interface ISendingManager
    {
        bool SendMessage(string subject, string body, List<MailAddress> addresses);
    }
}
