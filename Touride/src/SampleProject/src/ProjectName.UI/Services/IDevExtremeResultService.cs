using ProjectName.UI;
using ProjectName.UI.Models;

namespace ProjectName.UI.Services
{
    public interface IDevExtremeResultService<T> where T : class
    {
        DevExtremeResponseModel<List<T>> GetLoadResult(DataSourceLoadOptions mapper, LoadResult loadResult);
        DevExtremeResponseModel<List<DevExtremeGroupModelDto<List<T>>>> GetGroupResult(DataSourceLoadOptions mapper, LoadResult loadResult);
    }
}