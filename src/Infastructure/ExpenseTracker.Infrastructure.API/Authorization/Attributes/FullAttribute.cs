using Microsoft.AspNetCore.Authorization;

namespace ExpenseTracker.Infrastructure.API.Authorization.Attributes
{
    public class FullAttribute : AuthorizeAttribute
    {
        public FullAttribute() : base("full") { }
    }
}
