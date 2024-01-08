using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Transactions;
using Touride.Framework.Abstractions.Data.AuditLog;
using Touride.Framework.DataAudit.Postgresql.Configuration;

namespace Touride.Framework.DataAudit.Postgresql
{
    public class AuditLogStorePostgresql : IAuditLogStorePostgresql
    {
        private readonly DataAuditPostgresqlOptions _dataAuditPostgresqlOptions;
        private readonly string _insertCommandColumnNames;
        private readonly string _insertCommandValues;
        private readonly string _insertCommand;
        public AuditLogStorePostgresql(IOptions<DataAuditPostgresqlOptions> dataAuditSqlServerOptions)
        {
            _dataAuditPostgresqlOptions = dataAuditSqlServerOptions.Value;
            _insertCommandColumnNames = CreateInsertColumnNames();
            _insertCommandValues = CreateInsertCommandValues();
            _insertCommand = CreateInsertCommand();
        }
        public int StoreAuditEvents(Func<IEnumerable<AuditEvent>> auditEventsFucn, Func<int> saveChanges)
        {
            int result = 0;
            if (_dataAuditPostgresqlOptions.RunAsTransactional)
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
            if (_dataAuditPostgresqlOptions.RunAsTransactional)
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
                _dataAuditPostgresqlOptions.AuditTableColumnNameForEventTime,
                _dataAuditPostgresqlOptions.AuditTableColumnNameForEventType,
                _dataAuditPostgresqlOptions.AuditTableColumnNameForPk1,
                _dataAuditPostgresqlOptions.AuditTableColumnNameForPk2,
                _dataAuditPostgresqlOptions.AuditTableColumnNameForPropertyValues,
                _dataAuditPostgresqlOptions.AuditTableColumnNameForDatabaseName,
                _dataAuditPostgresqlOptions.AuditTableColumnNameForSchemaName,
                _dataAuditPostgresqlOptions.AuditTableColumnNameForTableName,
                _dataAuditPostgresqlOptions.AuditTableColumnNameForUser
                };
            return string.Join(", ", columnNames.Select(c => $"{c}"));
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
            var sql = $"INSERT INTO {_dataAuditPostgresqlOptions.SchemaName}.{_dataAuditPostgresqlOptions.TableName} ({_insertCommandColumnNames}) VALUES ({_insertCommandValues})";
            return sql;
        }

        private AuditContext CreateAuditContext()
        {
            return new AuditContext(_dataAuditPostgresqlOptions.ConnectionString);
        }

        private NpgsqlParameter[] GetSqlParametersForInsert(AuditEvent auditEvent)
        {
            var parameters = new List<NpgsqlParameter>();
            NpgsqlParameter EventTime = new NpgsqlParameter(ParameterNames.EventTime, NpgsqlDbType.Date);
            NpgsqlParameter EventType = new NpgsqlParameter(ParameterNames.EventType, NpgsqlDbType.Text);
            NpgsqlParameter PkValue1 = new NpgsqlParameter(ParameterNames.PkValue1, NpgsqlDbType.Bigint);
            NpgsqlParameter PkValue2 = new NpgsqlParameter(ParameterNames.PkValue2, NpgsqlDbType.Bigint);
            NpgsqlParameter PropertyValues = new NpgsqlParameter(ParameterNames.PropertyValues, NpgsqlDbType.Json);
            NpgsqlParameter DatabaseName = new NpgsqlParameter(ParameterNames.DatabaseName, NpgsqlDbType.Text);
            NpgsqlParameter SchemaName = new NpgsqlParameter(ParameterNames.SchemaName, NpgsqlDbType.Text);
            NpgsqlParameter TableName = new NpgsqlParameter(ParameterNames.TableName, NpgsqlDbType.Text);
            NpgsqlParameter User = new NpgsqlParameter(ParameterNames.User, NpgsqlDbType.Text);

            EventTime.Value = auditEvent.EventTime;
            EventType.Value = auditEvent.EventType;
            PkValue1.Value = auditEvent.PkValue1.HasValue ? auditEvent.PkValue1.Value : DBNull.Value;
            PkValue2.Value = auditEvent.PkValue2.HasValue ? auditEvent.PkValue2.Value : DBNull.Value;
            PropertyValues.Value = ToJson(auditEvent.PropertyValues);
            DatabaseName.Value = auditEvent.DatabaseName;
            SchemaName.Value = string.IsNullOrWhiteSpace(auditEvent.SchemaName) ? "dbo" : auditEvent.SchemaName;
            TableName.Value = auditEvent.TableName;
            User.Value = string.IsNullOrWhiteSpace(auditEvent.User) ? DBNull.Value : auditEvent.User;

            parameters.Add(EventTime);
            parameters.Add(EventType);
            parameters.Add(PkValue1);
            parameters.Add(PkValue2);
            parameters.Add(PropertyValues);
            parameters.Add(DatabaseName);
            parameters.Add(SchemaName);
            parameters.Add(TableName);
            parameters.Add(User);

            return parameters.ToArray();
        }

        private string ToJson(Dictionary<string, object> propertyValues)
        {
            return JsonConvert.SerializeObject(propertyValues);
        }
    }
}
