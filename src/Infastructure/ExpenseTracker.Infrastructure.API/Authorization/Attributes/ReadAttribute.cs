using Microsoft.AspNetCore.Authorization;

namespace ExpenseTracker.Infrastructure.API.Authorization.Attributes
{
    public class ReadAttribute : AuthorizeAttribute
    {

        public ReadAttribute() : base("read") { }
    }
}
