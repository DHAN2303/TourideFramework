using AutoMapper;
using FluentValidation.AspNetCore;
using Grpc.Net.Client;
using Hangfire;
using Hangfire.PostgreSql;
using Hangfire.SqlServer;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Polly;
using Polly.Extensions.Http;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Net;
using Touride.Framework.Abstractions.Auth;
using Touride.Framework.Abstractions.Client;
using Touride.Framework.Api.Helpers;
using Touride.Framework.Api.OpenApi;
using Touride.Framework.Api.Providers;
using Touride.Framework.Client.Abstractions;
using Touride.Framework.Client.Models.Service;
using Touride.Framework.Client.Providers;
using Touride.Framework.Dapr.Extensions;
using Touride.Framework.Data;
using Touride.Framework.Data.Configuration;
using Touride.Framework.DataAudit.Common;
using Touride.Framework.DataAudit.Elasticsearch.Configuration;
using Touride.Framework.DataAudit.Postgresql.Configuration;
using Touride.Framework.DataAudit.RabbitMq.Configuration;
using Touride.Framework.Logging.Extensions;
using Touride.Framework.Monitoring;
using Touride.Framework.Secret.Extensions;
using Touride.Framework.TaskScheduling.Hangfire.Configuration;
using Touride.Framework.Validation.Exceptions;

namespace Touride.Framework.Api.Extensions
{
    public static class ApiServiceCollectionExtensions
    {
        public static void AddMediatRService(this IServiceCollection builder, ApiOptions options)
        {
            builder.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(options.RegistrationAssemblies.ToArray());
            });
        }
        public static void AddTransantionInfoProvider(this IServiceCollection builder)
        {
            builder.AddScoped<List<TransantionInfoProvider>>();
        }
        public static void AddForwardedHeaders(this IServiceCollection builder)
        {
            builder.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }
        public static void AddConfigureLogging(this IServiceCollection builder, IConfiguration configuration)
        {
            builder.ConfigureLogging(configuration);
        }
        public static void AddCustomSwagger(this IServiceCollection builder)
        {
            builder.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenerationOptions>();

            builder.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.OperationFilter<AuthorizeCheckOperationFilter>();
                c.UseInlineDefinitionsForEnums();
            });
        }
        public static void AddCustomHttpClient(this IServiceCollection builder, IConfiguration configuration)
        {
            builder.AddHttpClient();

            var backends = configuration.GetSection("Touride.Framework:Service:Backends").Get<BackendModel[]>();

            if (backends != null)
            {
                foreach (var section in backends)
                {
                    builder.AddHttpClient(section.Name, c =>
                    {
                        c.BaseAddress = new Uri(section.Address);
                    }).
                    AddPolicyHandler(p => Policy
                      .Handle<Exception>(res => p.Method == HttpMethod.Get || p.Method == HttpMethod.Put)
                      .OrTransientHttpStatusCode()
                      .WaitAndRetryAsync(0, retryAttempt =>
                          TimeSpan.FromSeconds(Math.Pow(2, retryAttempt
                      )))).
                    AddPolicyHandler(p => HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .CircuitBreakerAsync(1, TimeSpan.FromSeconds(30)));
                }
                builder.AddTransient<ITokenProvider, HttpContextTokenProvider>();
                builder.AddTransient<IHttpClientProvider, HttpClientProvider>();
            }
        }
        public static void AddCustomMvc(this IServiceCollection builder)
        {
            builder.AddDaprClient();

            builder.AddControllers(opts =>
            {
                var authenticatedUserPolicy = new AuthorizationPolicyBuilder()
                      .RequireAuthenticatedUser()
                      .Build();
                opts.Filters.Add(new AuthorizeFilter(authenticatedUserPolicy));
                opts.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;

            }).AddDapr();
        }
        public static void AddDaprEventBus(this IServiceCollection builder, IConfiguration configuration)
        {
            builder.ConfigureEventBus(configuration);
        }
        public static void AddDaprStateStore(this IServiceCollection builder)
        {
            builder.ConfigureStateStore();
        }
        public static void AddFluentValidation(this IServiceCollection builder, ApiOptions options)
        {
            builder.AddFluentValidation(fv =>
            {
                foreach (var assembly in options.RegistrationAssemblies)
                {
                    fv.RegisterValidatorsFromAssembly(assembly);
                    fv.ValidatorOptions.PropertyNameResolver = (type, member, expression) =>
                    {
                        return char.ToLowerInvariant(member.Name[0]) + member.Name.Substring(1);
                    };
                }
            });
        }
        public static void AddCustomProblemDetails(this IServiceCollection builder)
        {
            builder.AddProblemDetails(x =>
            {
                x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
                x.Map<ValidationException>(ex => new ValidationExceptionProblemDetails(ex));
            });
        }
        public static void AddCustomAuthentication(this IServiceCollection builder, IConfiguration configuration)
        {
            IdentityModelEventSource.ShowPII = true;

            builder.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                configuration.Bind("Touride.Framework:Security:Jwt", options);
            });
        }
        public static void AddCustomClient(this IServiceCollection builder, IConfiguration configuration)
        {
            if (configuration["Touride.Framework:Security:Jwt:ClientEnable"] != null
                && configuration.GetValue<bool>("Touride.Framework:Security:Jwt:ClientEnable"))
            {
                builder.AddSingleton<IIdentityFlowProvider>(s => new ClientBearerTokenProvider(
                    url: configuration["Touride.Framework:Security:Jwt:Authority"],
                    clientId: configuration["Touride.Framework:Security:Jwt:ClientId"],
                    clientSecret: configuration["Touride.Framework:Security:Jwt:ClientSecret"],
                    audience: configuration["Touride.Framework:Security:Jwt:Audience"]
                ));
            }
        }
        public static void AddAuditLogStore(
            this IServiceCollection builder,
            IConfiguration configuration,
            ApiOptions options)
        {
            if (configuration.GetValue<bool>("Touride.Framework:Data:UnitOfWork:EnableDataAudit"))
            {
                switch (options.AuditLogStoreType)
                {
                    case AuditLogStoreType.Elastic:
                        {
                            builder.ConfigureDataAuditElastic(configuration);
                            break;
                        }
                    case AuditLogStoreType.RabbitMq:
                        {
                            builder.ConfigureDataAuditRabbitMq(configuration);
                            break;
                        }
                    case AuditLogStoreType.PostgreSql:
                        {
                            builder.ConfigureDataAuditPostgresql(configuration);
                            break;
                        }
                    case AuditLogStoreType.None:
                        {
                            break;
                        }
                    default:
                        {
                            builder.ConfigureDataAuditElastic(configuration);
                            break;
                        }
                }
            }

        }
        public static void AddConfigureAzureKeyVault(this IServiceCollection builder)
        {
            builder.ConfigureAzureKeyVault();
        }

        public static void AddCustomHangfire(this IServiceCollection builder, IConfiguration configuration)
        {

            if (configuration.GetValue<bool>("Touride.Framework:TaskScheduling:Hangfire:Enabled"))
            {
                builder.ConfigureTaskScheduler(configuration);
                builder.AddTaskScheduler(configuration);
            }

        }
        public static void AddSqlServerUnitOfWork<TContext>(
            this IServiceCollection builder,
            ApiOptions options,
            IConfiguration configuration) where TContext : UnitOfWork
        {
            builder.ConfigureUnitOfWork(configuration);

            builder.AddUnitOfWork<TContext>(p =>
            {
                p.UseSqlServer(options.ConnectionString,
                    m =>
                    {
                        m.MigrationsAssembly(options.MigrationsAssemblyName);
                    });
            });

        }

        public static void AddPostgresqlUnitOfWork<TContext>(
            this IServiceCollection builder,
            ApiOptions options,
            IConfiguration configuration) where TContext : UnitOfWork
        {
            builder.ConfigureUnitOfWork(configuration);

            builder.AddUnitOfWork<TContext>(p =>
            {
                p.UseNpgsql(options.ConnectionString,
                    m =>
                    {
                        m.MigrationsAssembly(options.MigrationsAssemblyName);
                    });
            });

        }
        public static void AddUserContextProvider(this IServiceCollection builder)
        {
            builder.AddScoped<IUserContextProvider, ClientInfoProvider>();
        }
        public static void AddGrpcService(this IServiceCollection builder, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("Touride.Framework:gRPC:Server:Enabled"))
            {
                builder.AddGrpc();
            }

            var isclient = configuration.GetSection("Touride.Framework:gRPC:Client:Enabled").Get<bool>();

            if (isclient)
            {
                var clientList = configuration.GetSection("Touride.Framework:gRPC:Client:ChannelUrl").Get<List<string>>();

                if (clientList != null && clientList.Any())
                {
                    foreach (var item in clientList)
                    {
                        builder.AddSingleton(services =>
                        {
                            var config = services.GetRequiredService<IConfiguration>();
                            var backendUrl = config[item];
                            return GrpcChannel.ForAddress(backendUrl);
                        });
                    }
                }
            }
        }
        public static void AddCustomSignalR(this IServiceCollection builder, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("Touride.Framework:SignalR:EnableScaling"))
                builder.AddSignalR().AddStackExchangeRedis(o =>
                {
                    o.ConnectionFactory = async writer =>
                    {
                        var config = new ConfigurationOptions
                        {
                            AbortOnConnectFail = false
                        };
                        config.EndPoints.Add(IPAddress.Parse(configuration.GetValue<string>("Touride.Framework:SignalR:StateStore:Address")), configuration.GetValue<int>("Touride.Framework:SignalR:StateStore:Port"));
                        config.SetDefaultPorts();
                        var connection = await ConnectionMultiplexer.ConnectAsync(config, writer);
                        connection.ConnectionFailed += (_, e) =>
                        {
                            Console.WriteLine("Connection to Redis failed.");
                        };

                        if (!connection.IsConnected)
                        {
                            Console.WriteLine("Did not connect to Redis.");
                        }

                        return connection;
                    };
                });
            else
                builder.AddSignalR();
        }
        public static void AddCustomAutoMapper(this IServiceCollection builder, ApiOptions options)
        {
            builder.AddAutoMapper(options.RegistrationAssemblies);
        }
        public static void AddMonitoringService(this IServiceCollection builder, IConfiguration configuration)
        {
            var monitoring = configuration.GetSection("Touride.Framework:Monitoring").Get<MonitoringOptions>();

            builder.AddMonitoringServices(monitoring);
        }
        public static void AddApiVersioningService(this IServiceCollection builder, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("Touride.Framework:ApiVersioning"))
            {
                builder.AddApiVersioning(
                    options =>
                    {
                        // "api-supported-versions" ve "api-deprecated-versions" header'larinin donmesini saglar
                        options.ReportApiVersions = true;
                        options.ApiVersionReader = new UrlSegmentApiVersionReader();
                        // Namespace'e gore versiyonlama saglar
                        options.Conventions.Add(new VersionByNamespaceConvention());
                    }
                );
                builder.AddVersionedApiExplorer(
                    options =>
                    {
                        // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                        // note: the specified format code will format the version as "'v'major[.minor][-status]"
                        options.GroupNameFormat = "'v'VVV";

                        // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                        // can also be used to control the format of the API version in route templates
                        options.SubstituteApiVersionInUrl = true;
                    }
                );
            }
        }


    }
}