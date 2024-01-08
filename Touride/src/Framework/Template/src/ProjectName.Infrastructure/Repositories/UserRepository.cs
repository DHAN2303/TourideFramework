using Microsoft.Extensions.Options;
using ProjectName.Domain.AggregatesModel.UserAggregate;
using Touride.Framework.Data.Abstractions;
using Touride.Framework.Data.Configuration;
using Touride.Framework.Data.Repository;

namespace ProjectName.Infrastructure.Repositories
{
    public class UserRepository : ConnectedRepository<User>, IUserRepository
    {
        public UserRepository(IUnitOfWork dbContext, IOptions<UnitOfWorkOptions> options) : base(dbContext, options)
        {
        }

    }
}
