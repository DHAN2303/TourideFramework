using Autofac.Extensions.DependencyInjection;
using Elastic.Apm.NetCoreAll;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Configuration;
using Touride.Framework.Abstractions.Logging;
using Touride.Framework.Abstractions.Secrets;
using Touride.Framework.Api.ExceptionHandling;
using Touride.Framework.Api.Helpers;
using Touride.Framework.Api.Middlewares;
using Touride.Framework.Logging.Serilog.Extensions;
using Touride.Framework.TaskScheduling.Hangfire.Autofac.Configuration;

namespace Touride.Framework.Api.Extensions
{
    /// <summary>
    /// Servislerin ayağa kaldırılması ve http pipeline konfigürasyonu işlemlerini framework düzeyinde toparlamak için kullanılır.
    /// </summary>
    public static class ApiApplicationBuilderExtensions
    {
        public static void UseCustomRouting(this IApplicationBuilder app)
        {
            app.UseRouting();
        }
        public static void UseCustomProblemDetails(this IApplicationBuilder app)
        {
            app.UseProblemDetails();
        }
        public static void UseCustomAutofacActivator(this IApplicationBuilder app)
        {
            var autofacRoot = app.ApplicationServices.GetAutofacRoot();
            GlobalConfiguration.Configuration.UseAutofacActivator(autofacRoot, true);

        }
        public static void UseUserContextProviderMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<UserContextProviderMiddleware>();
        }
        public static void UseSetLogging(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.SetLogging(configuration);
        }
        public static void UseCustomHangfireDashboard(this IApplicationBuilder app, IConfiguration configuration)
        {

            if (configuration.GetValue<bool>("Touride.Framework:TaskScheduling:Hangfire:Enabled") &&
                configuration.GetValue<bool>("Touride.Framework:TaskScheduling:Hangfire:Dashboard"))
            {
                app.UseHangfireDashboard("/hangfire", new DashboardOptions()
                {
                    Authorization = new[]
                    {
                        new HangfireCustomBasicAuthenticationFilter
                        {
                            User = configuration["Touride.Framework:TaskScheduling:Hangfire:UserName"],
                            Pass = configuration["Touride.Framework:TaskScheduling:Hangfire:Password"]
                        }
                    }
                });
            }

        }
        public static void UseRequestResponseMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestResponseMiddleware>();
        }
        public static void UseCustomSwagger(
            this IApplicationBuilder app,
            IApiVersionDescriptionProvider provider,
            ApiOptions options,
            IConfiguration configuration,
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger(o =>
                {
                    o.RouteTemplate = "swagger/{documentName}/" + ("swagger") + ".json";
                });

                app.UseCustomSwaggerUI(provider, options, configuration);
            }
            else
            {
                //string basePath = string.Empty;
                var basePath = configuration.GetValue<string>("Touride.Framework:OpenApi:URL");

                app.UseSwagger(c =>
                {
                    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                    {
                        swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{basePath}" } };
                    });
                });

                app.UseSwaggerUI(s =>
                {
                    s.SwaggerEndpoint("v1/swagger.json", configuration.GetValue<string>("Touride.Framework:OpenApi:Name"));
                });
            }
        }
        public static void UseCustomSwaggerUI(this IApplicationBuilder app,
            IApiVersionDescriptionProvider provider,
            ApiOptions options,
            IConfiguration configuration)
        {
            app.UseSwaggerUI(setup =>
            {
                // Her API versiyonu icin bir Swagger endpoint
                if (provider != null)
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        setup.SwaggerEndpoint($"{description.GroupName}/{options.ApiName ?? "swagger"}.json",
                            description.GroupName.ToUpperInvariant());
                    }
                }

                setup.DocumentTitle = options.ApiName;
                setup.OAuthClientId(configuration["Touride.Framework:Security:Jwt:ClientId"]);
                setup.OAuthAppName(configuration["Touride.Framework:Security:Jwt:ApiName"]);
                setup.OAuthUsePkce();
            });
        }
        public static void UseCustomApiExceptionHandler(this IApplicationBuilder app)
        {

            app.UseApiExceptionHandler(options =>
                options.AddResponseDetails = new DefaultApiExceptionOptions().AddResponseDetails);

        }
        public static void UseSecurityHeaders(this IApplicationBuilder app)
        {
            app.UseCustomSecurityHeaders((opt) =>
            {
                opt.HeadersToAdd.Add("X-Frame-Options", "DENY");
                opt.HeadersToAdd.Add("X-XSS-Protection", "1; mode=block");
                opt.HeadersToAdd.Add("X-Content-Type-Options", "nosniff");
                opt.HeadersToAdd.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");

                opt.HeadersToRemove.Add("X-Powered-By");
                opt.HeadersToRemove.Add("x-aspnet-version");
                opt.HeadersToRemove.Add("server");
                opt.HeadersToRemove.Add("Server");
            });
        }
        public static void UseElasticApm(this IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment env)
        {
            //if (env.IsProduction()) 
            app.UseAllElasticApm(configuration);
        }
        public static void UseCustomAuthentication(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
        public static void UseCustomRoute(this IApplicationBuilder app, ApiOptions options, IConfiguration configuration)
        {
            app.UseCloudEvents();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                if (configuration.GetValue<bool>("Touride.Framework:DaprEventBusSettings:ActorModel")) endpoints.MapActorsHandlers();
                endpoints.MapSubscribeHandler();
                if (options.HubList != null)
                    foreach (var item in options.HubList)
                    {
                        var method = typeof(HubEndpointRouteBuilderExtensions).GetMethod("MapHub",
                            new[] { typeof(IEndpointRouteBuilder), typeof(string) });
                        var generic = method.MakeGenericMethod(item);
                        generic.Invoke(null, new object[] { endpoints, $"/{item.Name}" });
                    }
            });

        }
        private static void SetLogging(this IApplicationBuilder app, IConfiguration configuration)
        {
            var loggingOptions = new LoggingOptions();
            configuration.Bind(LoggingOptions.OptionsSection, loggingOptions);
            if (loggingOptions.EnableRequestResponseLogging)
            {
                if (loggingOptions.LoggerType == LoggerType.Serilog)
                {
                    app.UseRequestResponseLogging();
                }
            }
        }
    }


}