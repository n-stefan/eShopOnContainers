using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
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

            var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
            var appSettings = await http.GetFromJsonAsync<AppSettings>("home/config");
            builder.Configuration.AddInMemoryCollection(new Dictionary<string, string> { ["PurchaseUrl"] = appSettings.PurchaseUrl });

            builder.Services.AddHttpClientServices(builder.HostEnvironment.BaseAddress);

            await builder.Build().RunAsync();
        }
    }

    static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpClientServices(this IServiceCollection services, string baseAddress)
        {
            services.AddHttpClient<ICatalogService, CatalogService>();

            //services.AddHttpClient("Catalog", c => c.BaseAddress = new Uri(baseAddress));

            return services;
        }
    }
}
