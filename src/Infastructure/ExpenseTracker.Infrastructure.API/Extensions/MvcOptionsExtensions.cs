using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.Repository.API.Extensions
{
    public static class MvcOptionsExtensions
    {
        public static void AddAuthorizationFilters(this MvcOptions options)
        {
            var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .RequireScope("app.expensetracker.api.full",
                                    "app.expensetracker.api.write",
                                    "app.expensetracker.api.read")
                                .Build();

            options.Filters.Add(new AuthorizeFilter(policy));
        }
    }
}
