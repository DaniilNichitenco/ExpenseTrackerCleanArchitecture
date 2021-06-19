using Microsoft.AspNetCore.Authorization;

namespace ExpenseTracker.Infrastructure.Repository.API.Authorization.Attributes
{
    public class WriteAttribute : AuthorizeAttribute
    {
        public WriteAttribute() : base("write") { }
    }
}
