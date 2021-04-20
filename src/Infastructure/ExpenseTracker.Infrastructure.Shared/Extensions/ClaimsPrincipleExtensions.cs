using System.Linq;
using System.Security.Claims;

namespace ExpenseTracker.Infrastructure.Shared.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static Claim GetClaim(this ClaimsPrincipal claimsPrincipal, string name) => claimsPrincipal.Claims.FirstOrDefault(x => x.Type == name);
    }
}