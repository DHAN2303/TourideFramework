using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using Touride.Framework.Abstractions.Client;
using Touride.Framework.Abstractions.Data.AuditLog;
using Touride.Framework.Data.Abstractions;
using Touride.Framework.Data.AuditProperties;
using Touride.Framework.Data.Configuration;
using Touride.Framework.Data.Repository;
using Touride.Framework.DataAudit.Common;

namespace Touride.Framework.Data
{
    public abstract class UnitOfWork : DbContext, IUnitOfWork
    {
        private readonly UnitOfWorkOptions _unitOfWorkOptions;
        private Dictionary<Type, object> repositories;
        private readonly IAuditPropertyInterceptorManager _auditPropertyInterceptorManager;
        private readonly IUserContextProvider _clientInfoProvider;
        private readonly IAuditEventCreator _auditEventCreator;
        private readonly IAuditLogStore _auditLogStore;
        private readonly IOptions<UnitOfWorkOptions> _options;

        private static readonly ILoggerFactory DebuggerLoggerFactory
            = LoggerFactory.Create(builder =>
            {
                builder.AddFilter((catagory, level) =>
                catagory == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                .AddDebug();
            });

        /// <summary>F
        /// Migration ve EF Power Tools tarafından kullanılmak üzere oluşturulmuştur.
        /// </summary>
        public UnitOfWork() : base()
        {
            _unitOfWorkOptions = new InternalUnitOfWorkOptions();
            _auditPropertyInterceptorManager = new InternalAuditPropertyInterceptorManager(_unitOfWorkOptions);
        }

        public UnitOfWork(
            IOptions<UnitOfWorkOptions> options,
            IAuditPropertyInterceptorManager auditPropertyInterceptorManager,
            IUserContextProvider clientInfoProvider,
            IAuditEventCreator auditEventCreator,
            IAuditLogStore auditLogStore)
            : base()
        {
            _unitOfWorkOptions = options.Value;
            _options = options;
            _auditPropertyInterceptorManager = auditPropertyInterceptorManager;
            _clientInfoProvider = clientInfoProvider;
            _auditEventCreator = auditEventCreator;
            _auditLogStore = auditLogStore;
            ConfigureContext();
        }

        public UnitOfWork(
            IOptions<UnitOfWorkOptions> options,
            [NotNull] DbContextOptions dbContextOptions,
            IAuditPropertyInterceptorManager auditPropertyInterceptorManager,
            IUserContextProvider clientInfoProvider,
            IAuditEventCreator auditEventCreator,
            IAuditLogStore auditLogStore)
            : base(dbContextOptions)
        {
            _unitOfWorkOptions = options.Value;
            _options = options;
            _auditPropertyInterceptorManager = auditPropertyInterceptorManager;
            _clientInfoProvider = clientInfoProvider;
            _auditEventCreator = auditEventCreator;
            _auditLogStore = auditLogStore;
            ConfigureContext();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_unitOfWorkOptions != null && _unitOfWorkOptions.EnableDebuggerLogger)
            {
                optionsBuilder.UseLoggerFactory(DebuggerLoggerFactory).EnableSensitiveDataLogging();
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                SetPropertyInterceptors(modelBuilder, entityType);
                SetDefaultDeleteBehavior(entityType);
            }

            base.OnModelCreating(modelBuilder);
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                if (_unitOfWorkOptions == null || _unitOfWorkOptions.RunAsDisconnected)
                {
                    repositories[type] = new DisconnectedRepository<TEntity>(this, _options);
                }
                else
                {
                    repositories[type] = new ConnectedRepository<TEntity>(this, _options);
                }
            }

            return (IRepository<TEntity>)repositories[type];
        }

        public int SaveChanges(AuditBehaviour auditBehaviour = AuditBehaviour.Enabled)
        {
            var timestamp = DateTime.Now;
            BeforeSave(timestamp);
            if (_unitOfWorkOptions.EnableDataAudit && auditBehaviour == AuditBehaviour.Enabled)
            {
                var auditEvents = GetAuditEvents(timestamp);
                return _auditLogStore.StoreAuditEvents(auditEvents, () => base.SaveChanges());
            }
            else
            {
                return base.SaveChanges();
            }
        }

        //TODO: Buradaki zaman kişinin lokasyon bilgisine göre şekillinedirilerek atılacak.
        public async Task<int> SaveChangesAsync(AuditBehaviour auditBehaviour = AuditBehaviour.Enabled, CancellationToken cancellationToken = new CancellationToken())
        {
            var timestamp = DateTime.Now;
            BeforeSave(timestamp);

            // await SaveIntegrationEventsAsync(cancellationToken);
            if (_unitOfWorkOptions.EnableDataAudit && auditBehaviour == AuditBehaviour.Enabled)
            {
                var auditEvents = GetAuditEvents(timestamp);
                return await _auditLogStore.StoreAuditEventsAsync(auditEvents, () => base.SaveChangesAsync());
            }
            else
            {
                return await base.SaveChangesAsync();
            }
        }

        private void ConfigureContext()
        {
            if (_unitOfWorkOptions == null || _unitOfWorkOptions.RunAsDisconnected)
            {
                ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            }
        }

        private void BeforeSave(DateTime timestamp)
        {
            ChangeTracker.DetectChanges();
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted))
            {
                if (_auditPropertyInterceptorManager != null && _clientInfoProvider != null)
                {
                    _auditPropertyInterceptorManager.OnSave(_clientInfoProvider, timestamp, entry);
                }
            }
        }

        #region[ENTITY_MODEL_CREATION]

        private void SetPropertyInterceptors(ModelBuilder modelBuilder, IMutableEntityType entityType)
        {
            if (_auditPropertyInterceptorManager != null)
            {
                _auditPropertyInterceptorManager.OnModelCreating(modelBuilder.Entity(entityType.Name));
            }
        }

        private void SetDefaultDeleteBehavior(IMutableEntityType entityType)
        {
            if (_unitOfWorkOptions != null)
            {
                var foreignKeys = entityType.GetForeignKeys();
                if (foreignKeys.Any())
                {
                    foreach (var fk in foreignKeys)
                    {
                        fk.DeleteBehavior = _unitOfWorkOptions.DefaultDeleteBehavior;
                    }
                }
            }
        }

        #endregion[END_ENTITY_MODEL_CREATION]

        #region[DATABASE_OPERATIONS]

        public bool CreateDatabase()
        {
            if (_unitOfWorkOptions != null && _unitOfWorkOptions.EnableDatabaseCreation)
            {
                return base.Database.EnsureCreated();
            }
            return false;
        }

        public bool DeleteDatabase()
        {
            if (_unitOfWorkOptions != null && _unitOfWorkOptions.EnableDatabaseDeletion)
            {
                return base.Database.EnsureDeleted();
            }
            return false;
        }

        public void OpenConnection()
        {
            base.Database.OpenConnection();
        }

        public async Task<bool> CreateDatabaseAsync()
        {
            if (_unitOfWorkOptions != null && _unitOfWorkOptions.EnableDatabaseCreation)
            {
                return await base.Database.EnsureCreatedAsync();
            }
            return false;
        }

        public async Task<bool> DeleteDatabaseAsync()
        {
            if (_unitOfWorkOptions != null && _unitOfWorkOptions.EnableDatabaseDeletion)
            {
                return await base.Database.EnsureDeletedAsync();
            }
            return false;
        }

        public async Task OpenConnectionAsync()
        {

            await base.Database.OpenConnectionAsync();
        }

        #endregion[END_DATABASE_OPERATIONS]

        #region[DATA_AUDIT]

        private Func<IEnumerable<AuditEvent>> GetAuditEvents(DateTime timestamp)
        {
            return _auditEventCreator.CreateAuditEvents(this, _clientInfoProvider, timestamp);
        }

        public void AutoMigration()
        {
            base.Database.Migrate();
        }

        #endregion[END_DATA_AUDIT]
    }
}
