using Microsoft.Extensions.Options;
using Order.Domain.AggregatesModel.OrderAggregate;
using Touride.Framework.Data.Abstractions;
using Touride.Framework.Data.Configuration;
using Touride.Framework.Data.Repository;

namespace Order.Infrastructure.Repositories
{
    public class OrderRepository : ConnectedRepository<Domain.AggregatesModel.OrderAggregate.Order>, IOrderRepository
    {
        public OrderRepository(IUnitOfWork dbContext, IOptions<UnitOfWorkOptions> options) : base(dbContext, options)
        {
        }
    }
}
