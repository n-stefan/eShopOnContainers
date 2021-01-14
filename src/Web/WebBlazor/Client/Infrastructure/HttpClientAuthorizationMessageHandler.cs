using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;

namespace WebBlazor.Client.Infrastructure
{
    public class HttpClientAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public HttpClientAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigationManager, IConfiguration configuration)
            : base(provider, navigationManager)
        {
            ConfigureHandler(
                authorizedUrls: new[] { configuration["PurchaseUrl"], configuration["MarketingUrl"] },
                scopes: new[] { "basket", "marketing", "locations", "orders" });
        }
    }
}
