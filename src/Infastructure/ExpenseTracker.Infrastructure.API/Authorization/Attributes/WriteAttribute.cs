using Microsoft.AspNetCore.Authorization;

namespace ExpenseTracker.Infrastructure.API.Authorization.Attributes
{
    public class WriteAttribute : AuthorizeAttribute
    {
        public WriteAttribute() : base("write") { }
    }
}
