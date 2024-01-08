using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Touride.Framework.Abstractions.Data.AuditLog;
using Touride.Framework.Data.Abstractions;
using Touride.Framework.Data.AuditLogging;
using Touride.Framework.Data.AuditProperties;
using Touride.Framework.DataAudit.Common;

namespace Touride.Framework.Data.Configuration
{
    public static class DataServiceCollectionExtensions
    {
        /// <summary>
        /// UnitOfWorkOptions konfigurasyonunun kaydı için kullanılır.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureUnitOfWork(this IServiceCollection services, IConfiguration configuration)
        {
            return services.Configure<UnitOfWorkOptions>(configuration.GetSection(UnitOfWorkOptions.UnitOfWorkOptionsSection));
        }

        /// <summary>
        /// UnitOfWorkOptions konfigurasyonunun kaydı için kullanılır.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureUnitOfWork(this IServiceCollection services, Action<UnitOfWorkOptions> action)
        {
            return services.Configure<UnitOfWorkOptions>(action);
        }

        /// <summary>
        /// UnitOfWork servisi ve bileşenlerini eklemek için kullanılır.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null) where TContext : UnitOfWork
        {
            services.AddInternalUnitOfWorkServices();
            if (optionsAction != null)
            {
                services.AddDbContextOptions<TContext>((p, b) => optionsAction(b));
            }
            services.AddScoped<IUnitOfWork, TContext>();
            return services;
        }

        /// <summary>
        /// UnitOfWork servisi ve bileşenlerini eklemek için kullanılır.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAdditionalUnitOfWork<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null) where TContext : UnitOfWork
        {
            if (optionsAction != null)
            {
                services.AddDbContextOptions<TContext>((p, b) => optionsAction(b));
            }
            services.AddScoped<TContext, TContext>();
            services.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();
            return services;
        }

        /// <summary>
        /// UnitOfWork servisi ve bileşenlerini eklemek için kullanılır.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        //public static IServiceCollection AddUnitOfWork(this IServiceCollection services, Type unitOfworkType)
        //{
        //    if (!typeof(UnitOfWork).IsAssignableFrom(unitOfworkType))
        //    {
        //        throw new Exception("Invalid unitOfWorkType");
        //    }
        //    services.AddInternalUnitOfWorkServices();
        //    services.AddScoped(unitOfworkType);
        //    return services;
        //}

        private static IServiceCollection AddInternalUnitOfWorkServices(this IServiceCollection services)
        {
            services.AddSingleton<IAuditPropertyInterceptor, HasCreateDateInterceptor>();
            services.AddSingleton<IAuditPropertyInterceptor, HasUpdateDateInterceptor>();
            services.AddSingleton<IAuditPropertyInterceptor, HasCreatedByInterceptor>();
            services.AddSingleton<IAuditPropertyInterceptor, HasUpdatedByInterceptor>();
            services.AddSingleton<IAuditPropertyInterceptor, HasCreatedAtInterceptor>();
            services.AddSingleton<IAuditPropertyInterceptor, HasUpdatedAtInterceptor>();
            services.AddSingleton<IAuditPropertyInterceptor, HasCreatedByUserCodeInterceptor>();
            services.AddSingleton<IAuditPropertyInterceptor, HasUpdatedByUserCodeInterceptor>();
            services.AddScoped<IAuditLogStore, NullAuditLogStore>();
            services.AddScoped<IAuditEventCreator, NullAuditEventCreator>();
            services.AddSingleton<IAuditPropertyInterceptorManager, AuditPropertyInterceptorManager>();
            return services;
        }

        private static void AddDbContextOptions<TContext>(this IServiceCollection services, Action<IServiceProvider, DbContextOptionsBuilder> optionsAction) where TContext : UnitOfWork
        {
            services.TryAdd(
                new ServiceDescriptor(
                    typeof(DbContextOptions<TContext>),
                    p => CreateDbContextOptions<TContext>(p, optionsAction),
                    ServiceLifetime.Scoped));

            services.Add(
                new ServiceDescriptor(
                    typeof(DbContextOptions),
                    p => p.GetRequiredService<DbContextOptions<TContext>>(),
                    ServiceLifetime.Scoped));
        }

        private static void AddAdditionalDbContextOptions<TContext>(this IServiceCollection services, Action<IServiceProvider, DbContextOptionsBuilder> optionsAction) where TContext : UnitOfWork
        {
            services.Add(
                new ServiceDescriptor(
                    typeof(DbContextOptions),
                    p => p.GetRequiredService<DbContextOptions<TContext>>(),
                    ServiceLifetime.Scoped));
        }

        private static DbContextOptions<TContext> CreateDbContextOptions<TContext>(
            IServiceProvider applicationServiceProvider,
            Action<IServiceProvider, DbContextOptionsBuilder> optionsAction)
            where TContext : UnitOfWork
        {
            var builder = new DbContextOptionsBuilder<TContext>(
                new DbContextOptions<TContext>(new Dictionary<Type, IDbContextOptionsExtension>()));

            builder.UseApplicationServiceProvider(applicationServiceProvider);

            optionsAction?.Invoke(applicationServiceProvider, builder);

            return builder.Options;
        }
    }
}
