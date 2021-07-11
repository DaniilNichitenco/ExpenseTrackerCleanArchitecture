using Microsoft.AspNetCore.Authorization;

namespace ExpenseTracker.Infrastructure.Repository.API.Authorization.Attributes
{
    public class FullAttribute : AuthorizeAttribute
    {
        public FullAttribute() : base("full") { }
    }
}
