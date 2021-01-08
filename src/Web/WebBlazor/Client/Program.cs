using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebBlazor.Client.Extensions;
using WebBlazor.Client.Services;
using WebBlazor.Shared;

namespace WebBlazor.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            using var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
            var appSettings = await http.GetFromJsonAsync<AppSettings>("home/config");
            builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
            {
                ["PurchaseUrl"] = appSettings.PurchaseUrl,
                ["MarketingUrl"] = appSettings.MarketingUrl,
                ["ActivateCampaignDetailFunction"] = appSettings.ActivateCampaignDetailFunction
            });

            builder.Services.AddOidcAuthentication(options =>
            {
                options.ProviderOptions.ClientId = "blazor";
                options.ProviderOptions.Authority = appSettings.IdentityUrl;
                options.ProviderOptions.PostLogoutRedirectUri = appSettings.CallBackUrl;
                options.ProviderOptions.ResponseType = "id_token token";
                options.ProviderOptions.DefaultScopes.Add("orders");
                options.ProviderOptions.DefaultScopes.Add("basket");
                options.ProviderOptions.DefaultScopes.Add("locations");
                options.ProviderOptions.DefaultScopes.Add("marketing");
                options.ProviderOptions.DefaultScopes.Add("webshoppingagg");
                options.ProviderOptions.DefaultScopes.Add("orders.signalrhub");
            });

            builder.Services.AddHttpClientServices();

            builder.Services.AddScoped<EventService>();

            await builder.Build().RunAsync();
        }
    }
}
