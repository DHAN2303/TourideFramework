using Elasticsearch.Net;
using Microsoft.Extensions.Options;
using Nest;
using System.Net.Security;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using Touride.Framework.DataAudit.Elasticsearch.Configuration;

namespace Touride.Framework.DataAudit.Elasticsearch
{
    internal class ElasticClientProvider : IElasticClientProvider
    {
        private readonly DataAuditElasticOptions dataAuditElasticOptions;

        public ElasticClientProvider(IOptions<DataAuditElasticOptions> dataAuditElasticOptions)
        {
            this.dataAuditElasticOptions = dataAuditElasticOptions.Value;
            Client = CreateClient();
        }
        public ElasticClient Client { get; }

        private ElasticClient CreateClient()
        {
            var connectionPool = new SingleNodeConnectionPool(new Uri(dataAuditElasticOptions.Uri));
            var connectionSettings = new ConnectionSettings(connectionPool)
                .ThrowExceptions(true)
                .RequestTimeout(TimeSpan.FromSeconds(dataAuditElasticOptions.Timeout))
            .DefaultMappingFor<Abstractions.Data.AuditLog.AuditEvent>(m => m.IndexName(dataAuditElasticOptions.IndexName + "-" + DateTime.Today.ToShortDateString()));

            if (!(string.IsNullOrEmpty(dataAuditElasticOptions.UserName) || string.IsNullOrEmpty(dataAuditElasticOptions.Password)))
            {
                connectionSettings.BasicAuthentication(dataAuditElasticOptions.UserName, dataAuditElasticOptions.Password);
            }

            return new ElasticClient(connectionSettings);
        }
    }
}
