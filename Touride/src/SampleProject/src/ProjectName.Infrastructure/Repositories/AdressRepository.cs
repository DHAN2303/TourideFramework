using Microsoft.Extensions.Options;
using ProjectName.Domain.AggregatesModel.AddressAggregate;
using Touride.Framework.Data.Abstractions;
using Touride.Framework.Data.Configuration;
using Touride.Framework.Data.Repository;

namespace ProjectName.Infrastructure.Repositories
{
    public class AddressRepository : ConnectedRepository<Address>, IAddressRepository
    {
        public AddressRepository(IUnitOfWork dbContext, IOptions<UnitOfWorkOptions> options) : base(dbContext, options)
        {
        }
    }
}
