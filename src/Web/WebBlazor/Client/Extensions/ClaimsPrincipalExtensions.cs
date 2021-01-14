using System.Linq;
using System.Security.Claims;

namespace WebBlazor.Client.Extensions
{
    static class ClaimsPrincipalExtensions
    {
        // Id
        public static string GetSub(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "sub")?.Value ?? string.Empty;

        public static string GetPreferredUsername(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value ?? string.Empty;

        public static string GetCity(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "address_city")?.Value ?? string.Empty;

        public static string GetStreet(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "address_street")?.Value ?? string.Empty;

        public static string GetState(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "address_state")?.Value ?? string.Empty;

        public static string GetCountry(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "address_country")?.Value ?? string.Empty;

        public static string GetZipCode(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "address_zip_code")?.Value ?? string.Empty;

        public static string GetCardNumber(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "card_number")?.Value ?? string.Empty;

        public static string GetCardHolderName(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "card_holder")?.Value ?? string.Empty;

        public static string GetExpiration(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "card_expiration")?.Value ?? string.Empty;

        public static string GetSecurityNumber(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "card_security_number")?.Value ?? string.Empty;

        public static string GetEmail(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "email")?.Value ?? string.Empty;

        public static string GetLastName(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "last_name")?.Value ?? string.Empty;

        public static string GetName(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "name")?.Value ?? string.Empty;

        public static string GetPhoneNumber(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "phone_number")?.Value ?? string.Empty;

        public static int GetCardType(this ClaimsPrincipal claimsPrincipal) =>
            int.Parse(claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "missing")?.Value ?? "0");
    }
}
