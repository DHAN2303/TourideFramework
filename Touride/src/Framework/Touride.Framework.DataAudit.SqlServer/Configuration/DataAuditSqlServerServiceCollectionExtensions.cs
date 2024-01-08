using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Touride.Framework.Abstractions.Data.AuditLog;
using Touride.Framework.DataAudit.Common;

namespace Touride.Framework.DataAudit.SqlServer.Configuration
{
    public static class DataAuditSqlServerServiceCollectionExtensions
    {
        /// <summary>
        /// UnitOfWorkOptions konfigurasyonunun kaydı için kullanılır.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureDataAuditSqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DataAuditSqlServerOptions>(configuration.GetSection(DataAuditSqlServerOptions.DataAuditSqlServerOptionsSection));
            services.AddScoped<IAuditLogStoreSqlServer, AuditLogStoreSqlServer>();
            services.AddScoped<IAuditEventCreator, AuditEventCreator>();
            return services;
        }

        /// <summary>
        /// UnitOfWorkOptions konfigurasyonunun kaydı için kullanılır.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureDataAuditSqlServer(this IServiceCollection services, Action<DataAuditSqlServerOptions> action)
        {
            services.Configure<DataAuditSqlServerOptions>(action);
            services.AddScoped<IAuditLogStoreSqlServer, AuditLogStoreSqlServer>();
            services.AddScoped<IAuditEventCreator, AuditEventCreator>();
            return services;
        }
    }
}
