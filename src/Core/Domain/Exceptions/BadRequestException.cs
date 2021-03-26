using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Domain.Exceptions
{
    public class BadRequestException :ApplicationException
    {
        public BadRequestException() { }
        public BadRequestException(string message) : base(message) { }
    }
}
