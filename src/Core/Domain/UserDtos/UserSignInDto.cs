using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Core.Domain.UserDtos
{
    public class UserSignInDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
