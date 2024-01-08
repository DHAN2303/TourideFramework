using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Product.Domain.AggregatesModel.CatalogBrandAggregate;
using Product.Domain.AggregatesModel.CatalogItemAggregate;
using Product.Domain.AggregatesModel.CatalogTypeAggregate;
using System.Reflection;
using Touride.Framework.Abstractions.Client;
using Touride.Framework.Abstractions.Data.AuditLog;
using Touride.Framework.Data;
using Touride.Framework.Data.AuditProperties;
using Touride.Framework.Data.Configuration;
using Touride.Framework.DataAudit.Common;

namespace Product.Infrastructure.DbContexts
{
    public class ProductDbContext : UnitOfWork
    {
        public ProductDbContext(
            IOptions<UnitOfWorkOptions> options,
            IAuditPropertyInterceptorManager auditPropertyInterceptorManager,
            IUserContextProvider clientInfoProvider,
            IAuditEventCreator auditEventCreator,
            IAuditLogStore auditLogStore,
            DbContextOptions<ProductDbContext> dbContextOptions)
            : base(options, dbContextOptions, auditPropertyInterceptorManager, clientInfoProvider, auditEventCreator, auditLogStore)
        { }

        public DbSet<CatalogBrand> CatalogBrand => Set<CatalogBrand>();
        public DbSet<CatalogItem> CatalogItem => Set<CatalogItem>();
        public DbSet<CatalogType> CatalogType => Set<CatalogType>();

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
