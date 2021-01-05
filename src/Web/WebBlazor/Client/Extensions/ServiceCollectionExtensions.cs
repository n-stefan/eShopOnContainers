﻿using Microsoft.Extensions.DependencyInjection;
using WebBlazor.Client.Infrastructure;
using WebBlazor.Client.Services;

namespace WebBlazor.Client.Extensions
{
    static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpClientServices(this IServiceCollection services)
        {
            services.AddScoped<HttpClientAuthorizationMessageHandler>();

            services.AddHttpClient<ICatalogService, CatalogService>();

            services.AddHttpClient<IBasketService, BasketService>()
                .AddHttpMessageHandler<HttpClientAuthorizationMessageHandler>();

            return services;
        }
    }
}