using Microsoft.Extensions.Options;
using Product.Domain.AggregatesModel.CatalogBrandAggregate;
using Touride.Framework.Data.Abstractions;
using Touride.Framework.Data.Configuration;
using Touride.Framework.Data.Repository;

namespace Product.Infrastructure.Repositories
{
    public class CatalogBrandRepository : ConnectedRepository<CatalogBrand>, ICatalogBrandRepository
    {
        public CatalogBrandRepository(IUnitOfWork dbContext, IOptions<UnitOfWorkOptions> options) : base(dbContext, options)
        {
        }
    }
}
