using DevExtreme.AspNet.Data.ResponseModel;
using MediatR;
using Touride.Framework.Abstractions.Application.Models;
using Touride.Framework.DevExtreme;

namespace Touride.Application.Handler.Queries.UserQueries.GetAllDevExtremeQueries
{
    public class GetAllDevExtremeQuery : IRequest<Result<LoadResult>>
    {
        public DataSourceLoadOptions loadOptions { get; set; }
    }
}
