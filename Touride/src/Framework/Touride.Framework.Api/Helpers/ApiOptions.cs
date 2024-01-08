using System.Reflection;
using Touride.Framework.DataAudit.Common;

namespace Touride.Framework.Api.Helpers
{
    public class ApiOptions
    {
        /// <summary>
        /// FluentValidation deklerasyonlarinin aranacagi assembly'ler
        /// </summary>
        public IEnumerable<Assembly> RegistrationAssemblies { get; set; }
        public string ApiName { get; set; }
        public IEnumerable<Type> HubList { get; set; }
        public IEnumerable<Type> ApiClientList { get; set; }
        public string ConnectionString { get; set; }
        public string MigrationsAssemblyName { get; set; }

        public AuditLogStoreType AuditLogStoreType { get; set; } = AuditLogStoreType.SqlServer;

    }
}
