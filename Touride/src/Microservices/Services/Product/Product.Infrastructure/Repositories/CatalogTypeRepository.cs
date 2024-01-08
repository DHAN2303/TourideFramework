using Microsoft.Extensions.Options;
using Product.Domain.AggregatesModel.CatalogTypeAggregate;
using Touride.Framework.Data.Abstractions;
using Touride.Framework.Data.Configuration;
using Touride.Framework.Data.Repository;

namespace Product.Infrastructure.Repositories
{
    public class CatalogTypeRepository : ConnectedRepository<CatalogType>, ICatalogTypeRepository
    {
        public CatalogTypeRepository(IUnitOfWork dbContext, IOptions<UnitOfWorkOptions> options) : base(dbContext, options)
        {
        }
    }
}
