using Microsoft.Extensions.DependencyInjection;
using WebBlazor.Client.Infrastructure;
using WebBlazor.Client.Services;

namespace WebBlazor.Client.Extensions
{
    static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpClientServices(this IServiceCollection services)
        {
            services.AddScoped<HttpClientAuthorizationMessageHandler>();
            services.AddScoped<HttpClientRequestIdDelegatingHandler>();

            services.AddHttpClient<ICatalogService, CatalogService>();

            services.AddHttpClient<IBasketService, BasketService>()
                .AddHttpMessageHandler<HttpClientAuthorizationMessageHandler>();

            services.AddHttpClient<ICampaignService, CampaignService>()
                .AddHttpMessageHandler<HttpClientAuthorizationMessageHandler>();

            services.AddHttpClient<ILocationService, LocationService>()
                .AddHttpMessageHandler<HttpClientAuthorizationMessageHandler>();

            services.AddHttpClient<IOrderingService, OrderingService>()
                .AddHttpMessageHandler<HttpClientAuthorizationMessageHandler>()
                .AddHttpMessageHandler<HttpClientRequestIdDelegatingHandler>();

            return services;
        }
    }
}
