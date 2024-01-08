using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Data;
using System.Transactions;
using Touride.Framework.Abstractions.Data.AuditLog;
using Touride.Framework.DataAudit.SqlServer.Configuration;

namespace Touride.Framework.DataAudit.SqlServer
{
    public class AuditLogStoreSqlServer : IAuditLogStoreSqlServer
    {
        private readonly DataAuditSqlServerOptions _dataAuditSqlServerOptions;
        private readonly string _insertCommandColumnNames;
        private readonly string _insertCommandValues;
        private readonly string _insertCommand;
        public AuditLogStoreSqlServer(IOptions<DataAuditSqlServerOptions> dataAuditSqlServerOptions)
        {
            _dataAuditSqlServerOptions = dataAuditSqlServerOptions.Value;
            _insertCommandColumnNames = CreateInsertColumnNames();
            _insertCommandValues = CreateInsertCommandValues();
            _insertCommand = CreateInsertCommand();
        }
        public int StoreAuditEvents(Func<IEnumerable<AuditEvent>> auditEventsFucn, Func<int> saveChanges)
        {
            int result = 0;
            if (_dataAuditSqlServerOptions.RunAsTransactional)
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
            if (_dataAuditSqlServerOptions.RunAsTransactional)
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
                using (var context = CreateAuditContext())
                {
                    foreach (var auditEvent in auditEvents)
                    {
                        var sqlParameters = GetSqlParametersForInsert(auditEvent);
                        context.Database.ExecuteSqlRaw(_insertCommand, sqlParameters);
                    }
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
                using (var context = CreateAuditContext())
                {
                    foreach (var auditEvent in auditEvents)
                    {
                        var sqlParameters = GetSqlParametersForInsert(auditEvent);
                        context.Database.ExecuteSqlRaw(_insertCommand, sqlParameters);
                    }
                }
            }
            return result;
        }

        private string CreateInsertColumnNames()
        {
            List<string> columnNames = new List<string>() {
                _dataAuditSqlServerOptions.AuditTableColumnNameForEventTime,
                _dataAuditSqlServerOptions.AuditTableColumnNameForEventType,
                _dataAuditSqlServerOptions.AuditTableColumnNameForPk1,
                _dataAuditSqlServerOptions.AuditTableColumnNameForPk2,
                _dataAuditSqlServerOptions.AuditTableColumnNameForPropertyValues,
                _dataAuditSqlServerOptions.AuditTableColumnNameForDatabaseName,
                _dataAuditSqlServerOptions.AuditTableColumnNameForSchemaName,
                _dataAuditSqlServerOptions.AuditTableColumnNameForTableName,
                _dataAuditSqlServerOptions.AuditTableColumnNameForUser
                };
            return string.Join(", ", columnNames.Select(c => $"[{c}]"));
        }

        private string CreateInsertCommandValues()
        {
            List<string> values = new List<string>() {
               ParameterNames.EventTime,
               ParameterNames.EventType,
               ParameterNames.PkValue1,
               ParameterNames.PkValue2,
               ParameterNames.PropertyValues,
               ParameterNames.DatabaseName,
               ParameterNames.SchemaName,
               ParameterNames.TableName,
               ParameterNames.User
                };
            return string.Join(", ", values.Select(c => c));
        }

        private string CreateInsertCommand()
        {
            return $"INSERT INTO [{_dataAuditSqlServerOptions.SchemaName}].[{_dataAuditSqlServerOptions.TableName}] ({_insertCommandColumnNames}) VALUES ({_insertCommandValues})";
        }

        private AuditContext CreateAuditContext()
        {
            return new AuditContext(_dataAuditSqlServerOptions.ConnectionString);
        }

        private SqlParameter[] GetSqlParametersForInsert(AuditEvent auditEvent)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter(ParameterNames.EventTime, auditEvent.EventTime));
            parameters.Add(new SqlParameter(ParameterNames.EventType, auditEvent.EventType));
            parameters.Add(new SqlParameter(ParameterNames.PkValue1, auditEvent.PkValue1.HasValue ? auditEvent.PkValue1.Value : DBNull.Value));
            parameters.Add(new SqlParameter(ParameterNames.PkValue2, auditEvent.PkValue2.HasValue ? auditEvent.PkValue2.Value : DBNull.Value));
            parameters.Add(new SqlParameter(ParameterNames.PropertyValues, ToJson(auditEvent.PropertyValues)));
            parameters.Add(new SqlParameter(ParameterNames.DatabaseName, auditEvent.DatabaseName));
            parameters.Add(new SqlParameter(ParameterNames.SchemaName, string.IsNullOrWhiteSpace(auditEvent.SchemaName) ? "dbo" : auditEvent.SchemaName));
            parameters.Add(new SqlParameter(ParameterNames.TableName, auditEvent.TableName));
            parameters.Add(new SqlParameter(ParameterNames.User, string.IsNullOrWhiteSpace(auditEvent.User) ? DBNull.Value : auditEvent.User));
            return parameters.ToArray();
        }

        private string ToJson(Dictionary<string, object> propertyValues)
        {
            return JsonConvert.SerializeObject(propertyValues);
        }
    }
}
