using Hangfire;
using Hangfire.MAMQSqlExtension;
using Hangfire.PostgreSql;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Touride.Framework.Abstractions.TaskScheduling;

namespace Touride.Framework.TaskScheduling.Hangfire.Configuration
{
    public static class TaskSchedulerServiceCollectionExtensions
    {
        /// <summary>
        /// TaskSchedulingOptions konfigurasyonunun kaydı için kullanılır.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureTaskScheduler(this IServiceCollection services, IConfiguration configuration)
        {
            return services.Configure<TaskSchedulingOptions>(configuration.GetSection(TaskSchedulingOptions.TaskSchedulingkOptionsSection));
        }

        /// <summary>
        /// ITaskSchedulingEngine servisi ve bileşenlerini eklemek için kullanılır.
        /// </summary>
        public static void AddTaskScheduler(this IServiceCollection services)
        {
            services.AddSingleton<ITaskSchedulingEngine, HangfireTaskSchedulingEngine>();
        }
        public static void AddTaskScheduler(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetSection(TaskSchedulingOptions.TaskSchedulingkOptionsSection)
                .Get<TaskSchedulingOptions>();

            if (options.DataBaseName.PostgreSql)
            {
                services.AddHangfire(x =>
                {
                    x.UseColouredConsoleLogProvider()
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UseResultsInContinuations()
                        .UsePostgreSqlStorage(options.ConnectionString);
                });
            }

            if (options.DataBaseName.SqlServer)
            {
                services.AddHangfire(conf => conf
                                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                                .UseSimpleAssemblyNameTypeSerializer()
                                .UseRecommendedSerializerSettings()
                                .UseSqlServerStorage(options.ConnectionString,
                                    new SqlServerStorageOptions
                                    {
                                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                                        QueuePollInterval = TimeSpan.Zero,
                                        UseRecommendedIsolationLevel = true,
                                        DisableGlobalLocks = true
                                    })
                            );

                //services.AddHangfire(x =>
                //{
                //    x.UseColouredConsoleLogProvider()
                //        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                //        .UseSimpleAssemblyNameTypeSerializer()
                //        .UseRecommendedSerializerSettings()
                //        .UseResultsInContinuations()
                //        .UseMAMQSqlServerStorage(options.ConnectionString, new SqlServerStorageOptions
                //        {
                //            UsePageLocksOnDequeue = true,
                //            DisableGlobalLocks = true,
                //        }, options.Queues);
                //});
            }

            if (options.Enabled)
            {
                services.AddHangfireServer(x =>
                {
                    x.Queues = options.Queues;
                });
            }

            services.AddSingleton<ITaskSchedulingEngine, HangfireTaskSchedulingEngine>();
        }

    }
}
