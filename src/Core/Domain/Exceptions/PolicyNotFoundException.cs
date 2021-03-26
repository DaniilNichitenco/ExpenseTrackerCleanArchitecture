using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Domain.Exceptions
{
    public class PolicyNotFoundException : ApplicationException
    {
        public PolicyNotFoundException(string policy) : base($"Permission {policy} not found") { }
    }
}
