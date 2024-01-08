using Order.Infrastructure.DbContexts;
using Touride.Framework.Api.Extensions;
using Touride.Framework.Api.Helpers;

namespace Order.API.Helpers
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
            builder.AddSqlServerUnitOfWork<OrderDbContext>(options, configuration);
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

            return builder;
        }

        public static void AddCustomApplicationServices(this IServiceCollection builder)
        {

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
