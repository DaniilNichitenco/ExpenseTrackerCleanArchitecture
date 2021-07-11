using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Application.Interfaces
{
    public interface IEmailSettings
    {
        string Port { get; set; }
        string Server { get; set; }
    }
}
