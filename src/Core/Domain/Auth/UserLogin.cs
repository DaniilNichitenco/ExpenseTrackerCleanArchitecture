using System;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Core.Domain.Auth
{
    public class UserLogin : IdentityUserLogin<Guid>
    {
    }
}
