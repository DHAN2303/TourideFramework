using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Touride.Framework.Abstractions.Data.AuditLog;
using Touride.Framework.DataAudit.Common;
using Touride.Framework.Secret.Extensions;

namespace Touride.Framework.DataAudit.RabbitMq.Configuration
{
    public static class DataAuditRabbitMqServiceCollectionExtensions
    {
        /// <summary>
        /// Data Audit Log ElasticSearch konfigurasyonunun kaydı için kullanılır.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureDataAuditRabbitMq(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<DataAuditRabbitMqOptions>(configuration.GetSection(DataAuditRabbitMqOptions.DataAuditRabbitMqOptionsSection));
            services.AddScoped<IAuditLogStore, AuditLogStoreRabbitMq>();
            services.AddScoped<IAuditEventCreator, AuditEventCreator>();
            services.AddSingleton(typeof(RabbitMqPublisher<,>));
            services.AddSingleton(typeof(RabbitMqService<,>));

            var rabbitmqconstring = configuration["Touride.Framework:DataAudit:RabbitMq:Hostname"];
            var rabbitmqusername = configuration["Touride.Framework:DataAudit:RabbitMq:Username"];
            var rabbitmqpassword = configuration["Touride.Framework:DataAudit:RabbitMq:Password"];

            services.AddSingleton(s => new ConnectionFactory()
            {
                Uri = new Uri
                    (rabbitmqconstring),
                UserName = rabbitmqusername,
                Password = rabbitmqpassword
            });
            return services;
        }
    }
}
