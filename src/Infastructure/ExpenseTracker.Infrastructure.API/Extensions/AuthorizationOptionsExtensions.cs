﻿using ExpenseTracker.Infrastructure.API.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace ExpenseTracker.Infrastructure.IdentityServer.Extensions
{
    public static class AuthorizationOptionsExtensions
    {
        public static void AddAuthorizationPolicies(this AuthorizationOptions options)
        {
            options.AddPolicy("read", policy =>
            {
                policy.AddRequirements(new ScopeRequirement(new List<string> { "app.expensetracker.api.read", "app.expensetracker.api.full" }, false));
            });

            options.AddPolicy("write", policy =>
            {
                policy.AddRequirements(new ScopeRequirement(new List<string> { "app.expensetracker.api.write", "app.expensetracker.api.full" }, false));
            });

            options.AddPolicy("full", policy =>
            {
                policy.AddRequirements(new ScopeRequirement(new List<string> { "app.expensetracker.api.full" }));
            });
        }
    }
}
