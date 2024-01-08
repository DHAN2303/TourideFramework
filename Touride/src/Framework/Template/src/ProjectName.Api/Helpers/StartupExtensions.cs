using Dapr.Client;
using Dapr.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Touride.Framework.Caching.Redis;
using ProjectName.Abstraction.Services;
using ProjectName.Application.Services;
using ProjectName.Infrastructure.DbContexts;
using System.Configuration;
using Touride.Framework.Api.Extensions;
using Touride.Framework.Api.Helpers;
using Touride.Framework.Caching.InMemory.Configuration;
using Touride.Framework.Elasticsearch.Extensions;
using Touride.Framework.Notification.Email.SmtpClient;
using Touride.Framework.Statics.Exceptions;

namespace ProjectName.Api.Helpers
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
            builder.AddSqlServerUnitOfWork<SampleDbContext>(options, configuration);
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
            builder.AddCustomHangfire(configuration);
            builder.AddInMemoryCache(configuration);
            builder.AddElasticSearch(configuration);
            //builder.AddRedisCache(configuration);
            builder.AddCustomApplicationServices(configuration);
            return builder;
        }

        public static IConfigurationBuilder AddCustomConfiguration(this IConfigurationBuilder builder)
        {
            builder.AddDaprSecretStore(
                "secretstore",
                new DaprClientBuilder().Build());

            return builder;
        }
        public static void AddCustomApplicationServices(this IServiceCollection builder, IConfiguration configuration)
        {
            builder.AddFluentEmailNotification(configuration);

            //builder.AddSingleton<ITaskSchedularService, TaskSchedularService>();

            //using ServiceProvider serviceProvider = builder.BuildServiceProvider(validateScopes: true);

            //using (IServiceScope scope = serviceProvider.CreateScope())
            //{
            //    var taskSchedulerService = scope.ServiceProvider.GetService<ITaskSchedularService>();

            //    taskSchedulerService.Execute();
            //}

            //builder.AddSmtpClientEmailNotification(new SmtpClientOptions
            //{
            //    EnableSsl = bool.TryParse(configuration[$"MailNotificationSettings:EnableSsl"], out bool enableSsl) ? enableSsl : false,
            //    UserName = configuration[$"MailNotificationSettings:UserName"],
            //    Password = configuration[$"MailNotificationSettings:Password"],
            //    Host = configuration[$"MailNotificationSettings:Host"],
            //    Port = int.TryParse(configuration[$"MailNotificationSettings:Port"], out int port) ? port : 0
            //});
        }

        public static IServiceCollection AddCustomHealthCheck(
        this IServiceCollection services,
        IConfiguration configuration)
        {

            var hcBuilder = services.AddHealthChecks();

            hcBuilder
            .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddSqlServer(
                    configuration.GetConnectionString(""),
                    name: "check",
                    tags: new[] { "test" });
            return services;
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
