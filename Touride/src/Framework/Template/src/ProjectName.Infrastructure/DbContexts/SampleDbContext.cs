using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProjectName.Domain.AggregatesModel.AddressAggregate;
using ProjectName.Domain.AggregatesModel.UserAggregate;
using System.Reflection;
using Touride.Framework.Abstractions.Client;
using Touride.Framework.Abstractions.Data.AuditLog;
using Touride.Framework.Data;
using Touride.Framework.Data.AuditProperties;
using Touride.Framework.Data.Configuration;
using Touride.Framework.DataAudit.Common;

namespace ProjectName.Infrastructure.DbContexts
{
    public class SampleDbContext : UnitOfWork
    {
        public SampleDbContext(
            IOptions<UnitOfWorkOptions> options,
            IAuditPropertyInterceptorManager auditPropertyInterceptorManager,
            IUserContextProvider clientInfoProvider,
            IAuditEventCreator auditEventCreator,
            IAuditLogStore auditLogStore,
            DbContextOptions<SampleDbContext> dbContextOptions)
            : base(options, dbContextOptions, auditPropertyInterceptorManager, clientInfoProvider, auditEventCreator, auditLogStore)
        { }

        public DbSet<User> User => Set<User>();
        public DbSet<Address> Address => Set<Address>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
