using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Infrastructure.Repository.API.Authorization.Requirements;

namespace ExpenseTracker.Infrastructure.Repository.API.Authorization.Handlers
{
    public class ScopeHandler : AuthorizationHandler<ScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeRequirement requirement)
        {

            var claims = context.User.Claims.Where(c => c.Type == "scope");
            if(claims.Count() == 0)
            {
                return Task.CompletedTask;
            }

            var scopes = new List<string>();
            foreach(var c in claims)
            {
                scopes.Add(c.Value);
            }

            if (requirement.RequireAll)
            {
                if(requirement.Scopes.All(s => scopes.Contains(s)))
                {
                    context.Succeed(requirement);
                }
            }
            else
            {
                if (requirement.Scopes.Any(s => scopes.Contains(s)))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
