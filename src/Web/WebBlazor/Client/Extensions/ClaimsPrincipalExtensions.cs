using System.Security.Claims;

namespace WebBlazor.Client.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        // Id
        public static string GetSub(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.FindFirst(x => x.Type == "sub")?.Value;

        public static string GetPreferredUsername(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.FindFirst(x => x.Type == "preferred_username")?.Value;
    }
}
