using Basket.Application.IntegrationEvents.EventHandling;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Touride.Framework.Api.Extensions;
using Touride.Framework.Api.Helpers;

namespace Basket.API.Helpers
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddDefactoApi(
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
            builder.AddMediatRService(options);
            builder.AddCustomAuthentication(configuration);
            builder.AddCustomHttpClient(configuration);
            builder.AddCustomProblemDetails();
            builder.AddFluentValidation(options);
            builder.AddUserContextProvider();
            builder.AddAuditLogStore(configuration, options);
            builder.AddTransantionInfoProvider();
            builder.AddCustomApplicationServices();

            return builder;
        }

        public static void AddCustomApplicationServices(this IServiceCollection builder)
        {
            builder.AddScoped<OrderStatusChangedToSubmittedIntegrationEventHandler>();
        }

        public static IApplicationBuilder UseDefactoApi(
            this IApplicationBuilder app,
            ApiOptions options,
            IConfiguration configuration,
            IWebHostEnvironment env)
        {

            app.UseSecurityHeaders();
            app.UseCustomApiExceptionHandler();
            app.UseCustomRouting();
            app.UseRequestResponseMiddleware();
            app.UseSetLogging(configuration);
            app.UseCustomSwagger(null, options, configuration, env);
            app.UseCustomProblemDetails();
            app.UseUserContextProviderMiddleware();
            app.UseCustomAuthentication();
            app.UseElasticApm(configuration, env);
            app.UseCustomRoute(options, configuration);

            return app;
        }

    }
}
