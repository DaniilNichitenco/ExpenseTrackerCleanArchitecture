using System;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Core.Domain.Auth
{
    public class User : IdentityUser<Guid>
    {
        public bool IsActive { get; set; }
    }
}
