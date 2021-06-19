using System;
using System.Linq;
using System.Security.Claims;

namespace ExpenseTracker.Infrastructure.Repository.Shared.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static Claim GetClaim(this ClaimsPrincipal claimsPrincipal, string name)
        {
            try
            {
                return claimsPrincipal.Claims.FirstOrDefault(x => x.Type == name);
            }
            catch
            {
                return null;
            }
        }
    }
}