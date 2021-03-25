using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Core.Domain.Auth
{
    public class User : IdentityUser<long>
    {
        public bool IsActive { get; set; }
    }
}
