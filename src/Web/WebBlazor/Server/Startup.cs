using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using WebBlazor.Server.Infrastructure;
using WebBlazor.Shared;

namespace WebBlazor.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public Startup()
        {
            var localPath = new Uri(Configuration["ASPNETCORE_URLS"])?.LocalPath ?? "/";
            Configuration["BaseUrl"] = localPath;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            RegisterAppInsights(services);

            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddUrlGroup(new Uri(Configuration["IdentityUrlHC"]), name: "identityapi-check", tags: new[] { "identityapi" });

            services.Configure<AppSettings>(Configuration);

            if (Configuration.GetValue<string>("IsClusterEnv") == bool.TrueString)
            {
                services.AddDataProtection(opts =>
                    opts.ApplicationDiscriminator = "eshop.webblazor")
                .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(Configuration["DPConnectionString"]), "DataProtection-Keys");
            }

            services.AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true);
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            //Seed Data
            WebContextSeed.Seed(app, env, loggerFactory);

            var pathBase = Configuration["PATH_BASE"];

            if (!string.IsNullOrEmpty(pathBase))
            {
                loggerFactory.CreateLogger<Startup>().LogDebug("Using PATH BASE '{pathBase}'", pathBase);
                app.UsePathBase(pathBase);
            }

            app.UseBlazorFrameworkFiles();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }

        private void RegisterAppInsights(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddApplicationInsightsKubernetesEnricher();
        }
    }
}
