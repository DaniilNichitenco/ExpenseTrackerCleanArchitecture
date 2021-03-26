using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Domain.Exceptions
{
    public class DeleteEntityException : ApplicationException
    {
        public DeleteEntityException(string message) : base(message) { }
    }
}
