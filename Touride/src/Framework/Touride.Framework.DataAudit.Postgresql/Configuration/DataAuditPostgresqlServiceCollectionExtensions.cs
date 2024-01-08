using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Touride.Framework.Abstractions.Data.AuditLog;
using Touride.Framework.DataAudit.Common;

namespace Touride.Framework.DataAudit.Postgresql.Configuration
{
    public static class DataAuditPostgresqlServiceCollectionExtensions
    {
        /// <summary>
        /// UnitOfWorkOptions konfigurasyonunun kaydı için kullanılır.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureDataAuditPostgresql(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DataAuditPostgresqlOptions>(configuration.GetSection(DataAuditPostgresqlOptions.DataAuditPostgresqlOptionsSection));
            services.AddScoped<IAuditLogStorePostgresql, AuditLogStorePostgresql>();
            services.AddScoped<IAuditEventCreator, AuditEventCreator>();
            return services;
        }

        /// <summary>
        /// UnitOfWorkOptions konfigurasyonunun kaydı için kullanılır.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureDataAuditPostgresql(this IServiceCollection services, Action<DataAuditPostgresqlOptions> action)
        {
            services.Configure<DataAuditPostgresqlOptions>(action);
            services.AddScoped<IAuditLogStorePostgresql, AuditLogStorePostgresql>();
            services.AddScoped<IAuditEventCreator, AuditEventCreator>();
            return services;
        }
    }
}
