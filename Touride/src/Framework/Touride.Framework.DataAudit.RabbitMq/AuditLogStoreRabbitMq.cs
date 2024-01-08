using Microsoft.Extensions.Options;
using System.Transactions;
using Touride.Framework.Abstractions.Data.AuditLog;
using Touride.Framework.DataAudit.RabbitMq.Configuration;

namespace Touride.Framework.DataAudit.RabbitMq
{
    public class AuditLogStoreRabbitMq : IAuditLogStore
    {
        private readonly DataAuditRabbitMqOptions _dataAuditRabbitMqOptions;
        private readonly RabbitMqPublisher<string, string> _publisher;
        public AuditLogStoreRabbitMq(
            IOptions<DataAuditRabbitMqOptions> dataAuditRabbitMqOptions,
            RabbitMqPublisher<string, string> publisher)
        {
            _dataAuditRabbitMqOptions = dataAuditRabbitMqOptions.Value;
            _publisher = publisher;
        }
        public int StoreAuditEvents(Func<IEnumerable<AuditEvent>> auditEventsFucn, Func<int> saveChanges)
        {
            int result = 0;
            if (_dataAuditRabbitMqOptions.RunAsTransactional)
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
            if (_dataAuditRabbitMqOptions.RunAsTransactional)
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
                _publisher.Publish(auditEvents,
                                    _dataAuditRabbitMqOptions.QueueName,
                                    _dataAuditRabbitMqOptions.RouteKey);
            }

            return result;
        }

        public async Task<int> StoreAuditEventsInternalAsync(Func<IEnumerable<AuditEvent>> auditEventsFucn, Func<Task<int>> saveChanges)
        {
            int result = await saveChanges.Invoke();
            if (result > 0)
            {
                var auditEvents = auditEventsFucn.Invoke();

                _publisher.Publish(auditEvents,
                                    _dataAuditRabbitMqOptions.QueueName,
                                    _dataAuditRabbitMqOptions.RouteKey);

            }
            return result;
        }
    }
}
