using Microsoft.EntityFrameworkCore;

namespace Touride.Framework.DataAudit.SqlServer
{
    internal class AuditContext : DbContext
    {
        public DbSet<InternalEntity> DataAuditEntities { get; set; }
        private readonly string _connectionString;
        public AuditContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
