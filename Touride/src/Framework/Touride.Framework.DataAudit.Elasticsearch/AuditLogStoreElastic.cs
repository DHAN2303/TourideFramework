using Microsoft.Extensions.Options;
using Nest;
using System.Transactions;
using Touride.Framework.Abstractions.Data.AuditLog;
using Touride.Framework.DataAudit.Elasticsearch.Configuration;

namespace Touride.Framework.DataAudit.Elasticsearch
{
    public class AuditLogStoreElastic : IAuditLogStore
    {
        private readonly DataAuditElasticOptions _dataAuditElasticOptions;
        private readonly ElasticClient elasticClient;

        public AuditLogStoreElastic(IOptions<DataAuditElasticOptions> dataAuditElasticOptions, IElasticClientProvider elasticClientProvider)
        {
            _dataAuditElasticOptions = dataAuditElasticOptions.Value;
            elasticClient = elasticClientProvider.Client;
        }
        public int StoreAuditEvents(Func<IEnumerable<AuditEvent>> auditEventsFucn, Func<int> saveChanges)
        {
            int result = 0;
            if (_dataAuditElasticOptions.RunAsTransactional)
            {
                using (var transaction = new TransactionScope())
                {
                    result = StoreAuditEventsInternal(auditEventsFucn, saveChanges);
                    transaction.Complete();
                }
            }
            else
            {
                result = StoreAuditEventsInternal(auditEventsFucn, saveChanges);
            }

            return result;
        }

        public async Task<int> StoreAuditEventsAsync(Func<IEnumerable<AuditEvent>> auditEventsFucn, Func<Task<int>> saveChanges)
        {
            int result = 0;
            if (_dataAuditElasticOptions.RunAsTransactional)
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    result = await StoreAuditEventsInternalAsync(auditEventsFucn, saveChanges);
                    transaction.Complete();
                }
            }
            else
            {
                result = await StoreAuditEventsInternalAsync(auditEventsFucn, saveChanges);
            }

            return result;
        }

        private int StoreAuditEventsInternal(Func<IEnumerable<AuditEvent>> auditEventsFucn, Func<int> saveChanges)
        {
            int result = saveChanges.Invoke();
            if (result > 0)
            {
                var auditEvents = auditEventsFucn.Invoke();
                var response = elasticClient.IndexMany(auditEvents);
                if (!response.IsValid)
                {
                    throw new Exception($"AuditLog Elastic Error {response.ItemsWithErrors.FirstOrDefault()?.Error?.Reason}");
                }
            }

            return result;
        }

        public async Task<int> StoreAuditEventsInternalAsync(Func<IEnumerable<AuditEvent>> auditEventsFucn, Func<Task<int>> saveChanges)
        {
            int result = await saveChanges.Invoke();
            if (result > 0)
            {
                var auditEvents = auditEventsFucn.Invoke();
                var response = await elasticClient.IndexManyAsync(auditEvents);
                if (!response.IsValid)
                {
                    throw new Exception($"AuditLog Elastic Error {response.ItemsWithErrors.FirstOrDefault()?.Error?.Reason}");
                }

            }
            return result;
        }
    }
}
