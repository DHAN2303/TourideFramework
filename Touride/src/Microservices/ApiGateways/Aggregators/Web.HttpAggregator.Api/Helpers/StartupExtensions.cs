using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Touride.Framework.Caching.Redis;
using Touride.Framework.Api.Extensions;
using Touride.Framework.Api.Helpers;
using Touride.Framework.Caching.InMemory.Configuration;
using Web.HttpAggregator.Abstraction.Services;
using Web.HttpAggregator.Application.Services;

namespace Web.HttpAggregator.Api.Helpers
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddTourideApi(
            this IServiceCollection builder,
            IConfiguration configuration,
            ApiOptions options)
        {
            builder.AddCustomMvc();
            builder.AddForwardedHeaders();
            builder.AddConfigureLogging(configuration);
            builder.AddCustomAutoMapper(options);
            builder.AddCustomSwagger();
            builder.AddDaprEventBus(configuration);
            builder.AddDaprStateStore();
            //builder.AddMediatRService(options);
            builder.AddCustomAuthentication(configuration);
            builder.AddCustomHttpClient(configuration);
            builder.AddCustomProblemDetails();
            builder.AddFluentValidation(options);
            builder.AddUserContextProvider();
            builder.AddAuditLogStore(configuration, options);
            builder.AddTransantionInfoProvider();
            builder.AddInMemoryCache(configuration);
            builder.AddCustomHangfire(configuration);
            //builder.AddRedisCache(configuration);
            return builder;
        }

        public static void AddCustomApplicationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IBasketService, BasketService>();
        }

        public static IApplicationBuilder UseTourideApi(
            this IApplicationBuilder app,
            ApiOptions options,
            IConfiguration configuration,
            IApiVersionDescriptionProvider provider,
            IWebHostEnvironment env)
        {

            app.UseSecurityHeaders();
            app.UseCustomApiExceptionHandler();
            app.UseCustomRouting();
            app.UseRequestResponseMiddleware();
            app.UseSetLogging(configuration);
            app.UseCustomSwagger(provider, options, configuration, env);
            app.UseCustomProblemDetails();
            app.UseCustomHangfireDashboard(configuration);
            app.UseUserContextProviderMiddleware();
            app.UseCustomAuthentication();
            app.UseElasticApm(configuration, env);
            app.UseCustomRoute(options, configuration);
            app.UseCustomAutofacActivator();
            return app;
        }

    }
}
