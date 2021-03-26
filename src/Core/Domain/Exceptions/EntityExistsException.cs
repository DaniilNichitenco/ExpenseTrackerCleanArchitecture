using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Domain.Exceptions
{
    public class EntityExistsException : ApplicationException
    {
        public EntityExistsException() { }

        public EntityExistsException(string message) : base(message) { }
    }
}
