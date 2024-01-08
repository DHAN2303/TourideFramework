using Touride.Framework.DevExtreme;
using Touride.UI.Models;
using DevExtreme.AspNet.Data.ResponseModel;

namespace Touride.UI.Services
{
    public interface IDevExtremeResultService<T> where T : class
    {
        DevExtremeResponseModel<List<T>> GetLoadResult(DataSourceLoadOptions mapper, LoadResult loadResult);
        DevExtremeResponseModel<List<DevExtremeGroupModelDto<List<T>>>> GetGroupResult(DataSourceLoadOptions mapper, LoadResult loadResult);
    }
}