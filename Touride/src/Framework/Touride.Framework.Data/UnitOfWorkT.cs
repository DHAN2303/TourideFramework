using Touride.Framework.Abstractions.Data.AuditLog;
using Touride.Framework.Data.Abstractions;

namespace Touride.Framework.Data
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : IUnitOfWork
    {
        private readonly TContext _context;

        public UnitOfWork(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AutoMigration()
        {
            _context.AutoMigration();
        }

        public bool CreateDatabase()
        {
            return _context.CreateDatabase();
        }

        public async Task<bool> CreateDatabaseAsync()
        {
            return await _context.CreateDatabaseAsync();
        }

        public bool DeleteDatabase()
        {
            return _context.DeleteDatabase();
        }

        public async Task<bool> DeleteDatabaseAsync()
        {
            return await _context.DeleteDatabaseAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return _context.GetRepository<TEntity>();
        }

        public void OpenConnection()
        {
            _context.OpenConnection();
        }

        public async Task OpenConnectionAsync()
        {
            await _context.OpenConnectionAsync();
        }

        public int SaveChanges(AuditBehaviour auditBehaviour = AuditBehaviour.Enabled)
        {
            return _context.SaveChanges(auditBehaviour);
        }

        public async Task<int> SaveChangesAsync(AuditBehaviour auditBehaviour = AuditBehaviour.Enabled, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _context.SaveChangesAsync(auditBehaviour);
        }
    }
}
