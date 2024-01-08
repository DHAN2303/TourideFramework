using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;
using Touride.Framework.Abstractions.Client;
using Touride.Framework.Abstractions.Data.AuditLog;
using Touride.Framework.Data;
using Touride.Framework.Data.AuditProperties;
using Touride.Framework.Data.Configuration;
using Touride.Framework.DataAudit.Common;

namespace Order.Infrastructure.DbContexts
{
    public class OrderDbContext : UnitOfWork
    {
        public OrderDbContext(
            IOptions<UnitOfWorkOptions> options,
            IAuditPropertyInterceptorManager auditPropertyInterceptorManager,
            IUserContextProvider clientInfoProvider,
            IAuditEventCreator auditEventCreator,
            IAuditLogStore auditLogStore,
            DbContextOptions<OrderDbContext> dbContextOptions)
            : base(options, dbContextOptions, auditPropertyInterceptorManager, clientInfoProvider, auditEventCreator, auditLogStore)
        { }

        public DbSet<Domain.AggregatesModel.OrderAggregate.Order> Order => Set<Domain.AggregatesModel.OrderAggregate.Order>();
        public DbSet<Domain.AggregatesModel.OrderItemAggregate.OrderItem> OrderItem => Set<Domain.AggregatesModel.OrderItemAggregate.OrderItem>();
        public DbSet<Domain.AggregatesModel.AddressAggregate.Address> Address => Set<Domain.AggregatesModel.AddressAggregate.Address>();

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
