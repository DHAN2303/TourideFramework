using Microsoft.Extensions.Options;
using Product.Domain.AggregatesModel.CatalogItemAggregate;
using Touride.Framework.Data.Abstractions;
using Touride.Framework.Data.Configuration;
using Touride.Framework.Data.Repository;

namespace Product.Infrastructure.Repositories
{
    public class CatalogItemRepository : ConnectedRepository<CatalogItem>, ICatalogItemRepository
    {
        public CatalogItemRepository(IUnitOfWork dbContext, IOptions<UnitOfWorkOptions> options) : base(dbContext, options)
        {
        }
    }
}
