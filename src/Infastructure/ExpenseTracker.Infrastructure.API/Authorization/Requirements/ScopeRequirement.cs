using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.API.Authorization.Requirements
{
    public class ScopeRequirement : IAuthorizationRequirement
    {
        public List<string> Scopes { get; set; }
        public bool RequireAll { get; set; }

        public ScopeRequirement(List<string> scopes, bool requireAll)
        {
            Scopes = scopes;
            RequireAll = requireAll;
        }

        public ScopeRequirement(List<string> scopes)
        {
            Scopes = scopes;
            RequireAll = true;
        }
    }
}
