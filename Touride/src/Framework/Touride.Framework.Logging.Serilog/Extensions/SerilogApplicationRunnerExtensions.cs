using Elasticsearch.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Elasticsearch;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.Graylog;
using Serilog.Sinks.Graylog.Core.Transport;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.RabbitMQ;
using System.Diagnostics;
using Touride.Framework.Abstractions.Logging;
using Touride.Framework.Abstractions.Secrets;
using ILogger = Serilog.ILogger;
using LogLevel = Touride.Framework.Abstractions.Logging.LogLevel;
using Ser = Serilog;

namespace Touride.Framework.Logging.Serilog.Extensions
{
    public static class SerilogApplicationRunnerExtensions
    {
        /// <summary>
        /// Hosting ile ilgili hataları yakalamak için kullanılır.
        /// Touride.Framework.Api-> Program.cs
        /// </summary>
        /// <param name="builderFunc"></param>
        /// <param name="args"></param>
        /// <param name="loggingOptions"></param>
        public static void RunHost(Func<string[],
            IHostBuilder> builderFunc,
            string[] args,
            LoggingOptions loggingOptions,
            IConfiguration configuration)
        {
            Ser.Log.Logger = CreateLogger(loggingOptions, configuration);
            try
            {
                Ser.Log.Information("Application starting.");
                var hostBuilder = builderFunc.Invoke(args).UseSerilog();
                if (loggingOptions.ClearDefaultLogProviders)
                {
                    hostBuilder.ConfigureLogging((hostingContext, config) => { config.ClearProviders(); });
                }
                hostBuilder.Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        /// <summary>
        /// Uygulamadaki hataları yakalamak için, tanımlı konfigürasyonu kullanarak başlatır.
        /// </summary>
        /// <param name="loggingOptions"></param>
        /// <returns></returns>
        private static ILogger CreateLogger(LoggingOptions loggingOptions, IConfiguration configuration)
        {

            if (loggingOptions.EnableSelfLog)
            {
                Ser.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
                Ser.Debugging.SelfLog.Enable(Console.Error);
            }

            var loggerConfiguration = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", loggingOptions.ApplicationName);

            if (loggingOptions.OverrideMicrosoftLevels)
            {
                loggerConfiguration.MinimumLevel
                .Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning);
            }

            if (loggingOptions.EnableWriteToConsole)
            {
                loggerConfiguration.WriteTo.Console();
            }

            if (loggingOptions.EnableWriteToDebug)
            {
                loggerConfiguration.WriteTo.Debug();
            }

            if (loggingOptions.EnableWriteToFile)
            {
                if (loggingOptions.LogToFileOptions == null)
                {
                    throw new Exception("Log to file options are missing for Write to file");
                }
                loggerConfiguration.WriteTo.File(
                    formatter: new CompactJsonFormatter(),
                    path: loggingOptions.LogToFileOptions.Path,
                    restrictedToMinimumLevel: LogLevelToLogEventLevel(loggingOptions.LogToFileOptions.MinimumLevel),
                    fileSizeLimitBytes: loggingOptions.LogToFileOptions.FileSizeLimitBytes,
                    rollingInterval: ToSerilogInterval(loggingOptions.LogToFileOptions.RollingInterval)
                    );
            }

            if (loggingOptions.EnableWriteToElasticSearch)
            {
                if (loggingOptions.LogToElasticSearchOptions == null)
                {
                    throw new Exception("Log to elasticsearch options are missing for Write to ElasticSearch");
                }

                loggerConfiguration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(loggingOptions.LogToElasticSearchOptions.Uri))
                {
                    MinimumLogEventLevel = LogLevelToLogEventLevel(loggingOptions.LogToElasticSearchOptions.MinimumLevel),
                    AutoRegisterTemplate = loggingOptions.LogToElasticSearchOptions.AutoRegisterTemplate,
                    IndexFormat = loggingOptions.LogToElasticSearchOptions.IndexFormat,
                    TemplateName = loggingOptions.LogToElasticSearchOptions.TemplateName,
                    CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
                    ModifyConnectionSettings = x => x.BasicAuthentication(loggingOptions.LogToElasticSearchOptions.UserName, loggingOptions.LogToElasticSearchOptions.Password)
                }); ;
            }

            if (loggingOptions.EnableWriteToMsSqlServer)
            {
                if (loggingOptions.LogToMsSqlServerOptions == null)
                {
                    throw new Exception("Log to MsSqlServer options are missing for Write to MSSQL");
                }

                loggerConfiguration.WriteTo.MSSqlServer(
                    loggingOptions.LogToMsSqlServerOptions.ConnectionString,
                    new MSSqlServerSinkOptions()
                    {
                        AutoCreateSqlTable = loggingOptions.LogToMsSqlServerOptions.AutoCreateSqlTable,
                        SchemaName = loggingOptions.LogToMsSqlServerOptions.SchemaName,
                        TableName = loggingOptions.LogToMsSqlServerOptions.TableName,
                        BatchPeriod = TimeSpan.FromSeconds(loggingOptions.LogToMsSqlServerOptions.BatchPeriod),
                        BatchPostingLimit = loggingOptions.LogToMsSqlServerOptions.BatchPostLimit,
                    }
                    );
            }

            if (loggingOptions.EnableWriteToRabbitMq)
            {
                if (loggingOptions.LogToRabbitMqOptions == null)
                {
                    throw new Exception("Log to RabbitMq options are missing for Write to Elasticsearch");
                }

                loggerConfiguration.WriteTo.RabbitMQ((clientConfiguration, sinkConfiguration) =>
                {
                    clientConfiguration.Username = loggingOptions.LogToRabbitMqOptions.Username;// rabbitmqusername;
                    clientConfiguration.Password = loggingOptions.LogToRabbitMqOptions.Password; //rabbitmqpassword;
                    clientConfiguration.Exchange = loggingOptions.LogToRabbitMqOptions.Exchange;
                    clientConfiguration.ExchangeType = loggingOptions.LogToRabbitMqOptions.ExchangeType;
                    clientConfiguration.DeliveryMode = RabbitMQDeliveryMode.NonDurable;
                    clientConfiguration.RouteKey = loggingOptions.LogToRabbitMqOptions.RouteKey;//Logs
                    clientConfiguration.Port = loggingOptions.LogToRabbitMqOptions.Port;//5672

                    foreach (string hostname in loggingOptions.LogToRabbitMqOptions.Hostnames)
                    {
                        clientConfiguration.Hostnames.Add(hostname);
                    }

                    sinkConfiguration.TextFormatter = new JsonFormatter();
                });

            }

            if (loggingOptions.EnableWriteGraylog)
            {
                if (loggingOptions.LogToGraylogOptions == null)
                {
                    throw new Exception("Log to GrayLog options are missing for Write to GrayLog");
                }

                loggerConfiguration.WriteTo.Graylog(new GraylogSinkOptions
                {
                    HostnameOrAddress = loggingOptions.LogToGraylogOptions.HostnameOrAddress,
                    Port = loggingOptions.LogToGraylogOptions.Port,
                    TransportType = TransportType.Udp
                });
            }

            return loggerConfiguration.CreateLogger();
        }

        private static LogEventLevel LogLevelToLogEventLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    {
                        return LogEventLevel.Debug;
                    }
                case LogLevel.Error:
                    {
                        return LogEventLevel.Error;
                    }
                case Abstractions.Logging.LogLevel.Fatal:
                    {
                        return LogEventLevel.Fatal;
                    }
                case LogLevel.Information:
                    {
                        return LogEventLevel.Information;
                    }
                case LogLevel.Verbose:
                    {
                        return LogEventLevel.Verbose;
                    }
                case LogLevel.Warning:
                    {
                        return LogEventLevel.Warning;
                    }
                default:
                    {
                        return LogEventLevel.Information;
                    }
            }
        }

        private static Ser.RollingInterval ToSerilogInterval(Abstractions.Logging.RollingInterval rollingInterval)
        {
            switch (rollingInterval)
            {
                case Abstractions.Logging.RollingInterval.None:
                    {
                        return Ser.RollingInterval.Infinite;
                    }
                case Abstractions.Logging.RollingInterval.Year:
                    {
                        return Ser.RollingInterval.Year;
                    }
                case Abstractions.Logging.RollingInterval.Month:
                    {
                        return Ser.RollingInterval.Month;
                    }
                case Abstractions.Logging.RollingInterval.Day:
                    {
                        return Ser.RollingInterval.Day;
                    }
                case Abstractions.Logging.RollingInterval.Hour:
                    {
                        return Ser.RollingInterval.Hour;
                    }
                case Abstractions.Logging.RollingInterval.Minute:
                    {
                        return Ser.RollingInterval.Minute;
                    }
                default:
                    {
                        return Ser.RollingInterval.Day;
                    }
            }
        }
    }
}
