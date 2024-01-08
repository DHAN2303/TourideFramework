using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Touride.Framework.Abstractions.Logging;
using Touride.Framework.Abstractions.Secrets;
using Touride.Framework.Data.Abstractions;
using Touride.Framework.Logging.Serilog.Extensions;

namespace Touride.Framework.Api.Extensions
{
    public static class ApiApplicationRunnerExtensions
    {
        private static LoggingOptions _loggingOptions;
        private static string[] arguments;
        private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        private static LoggingOptions LoggingOptions
        {
            get
            {
                if (_loggingOptions == null)
                {
                    var loggingOptions = new LoggingOptions();
                    Configuration.Bind(LoggingOptions.OptionsSection, loggingOptions);
                    _loggingOptions = loggingOptions;
                }

                return _loggingOptions;
            }
        }

        private static string EnvironmentName
        {
            get
            {
                var webHostBuilder = WebHost.CreateDefaultBuilder(arguments);
                var environment = webHostBuilder.GetSetting("environment");
                return environment;
            }
        }

        public static void RunHost(Func<string[], IHostBuilder> builderFunc, string[] args)
        {
            arguments = args;
            var loggingOptions = LoggingOptions;
            var host = builderFunc.Invoke(args).Build();
            if (Configuration.GetValue<bool>("Touride.Framework:DatabaseMigrationsConfiguration:ApplyDatabaseMigrations"))
            {
                using (var scope = host.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    db.AutoMigration();
                }
            }

            switch (loggingOptions.LoggerType)
            {
                case LoggerType.Microsoft:
                    {
                        WithMicorostLogging(builderFunc, args);
                        break;
                    }
                case LoggerType.Serilog:
                    {
                        var scope = host.Services.CreateScope();
                        SerilogApplicationRunnerExtensions.RunHost(builderFunc, args, loggingOptions, Configuration);
                        break;
                    }
                default:
                    {
                        throw new Exception("Not supported LoggerType");
                    }
            }
        }

        private static void WithMicorostLogging(Func<string[], IHostBuilder> builderFunc, string[] args)
        {
            builderFunc.Invoke(args).Build().Run();
        }
    }
}
