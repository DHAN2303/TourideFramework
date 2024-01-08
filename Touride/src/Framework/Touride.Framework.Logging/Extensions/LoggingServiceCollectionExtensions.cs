using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Touride.Framework.Abstractions.Logging;

namespace Touride.Framework.Logging.Extensions
{
    public static class LoggingServiceCollectionExtensions
    {
        /// <summary>
        /// Logging konfigurasyonunun kaydı için kullanılır.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureLogging(this IServiceCollection services, IConfiguration configuration)
        {
            return services.Configure<LoggingOptions>(configuration.GetSection(LoggingOptions.OptionsSection));
        }
    }
}
