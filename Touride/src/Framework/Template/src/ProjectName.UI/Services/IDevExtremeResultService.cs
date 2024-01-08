using ProjectName.Api;
using ProjectName.UI.Models;
using LoadResult = ProjectName.Api.LoadResult;

namespace ProjectName.UI.Services
{
    public interface IDevExtremeResultService<T> where T : class
    {
        DevExtremeResponseModel<List<T>> GetLoadResult(DataSourceLoadOptions mapper, LoadResult loadResult);
        DevExtremeResponseModel<List<DevExtremeGroupModelDto<List<T>>>> GetGroupResult(DataSourceLoadOptions mapper, LoadResult loadResult);
    }
}