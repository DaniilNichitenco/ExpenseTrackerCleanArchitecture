using Microsoft.AspNetCore.Authorization;

namespace ExpenseTracker.Infrastructure.Repository.API.Authorization.Attributes
{
    public class ReadAttribute : AuthorizeAttribute
    {

        public ReadAttribute() : base("read") { }
    }
}
