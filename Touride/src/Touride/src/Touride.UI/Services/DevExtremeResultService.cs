using Newtonsoft.Json;
using NuGet.Protocol;
using Touride.UI.Models;

namespace Touride.UI.Services
{
    public class DevExtremeResultService<T> : IDevExtremeResultService<T>
        where T : class
    {
        public DevExtremeResponseModel<List<DevExtremeGroupModelDto<List<T>>>> GetGroupResult(DataSourceLoadOptions mapper, LoadResult loadResult)
        {
            var jsonModel = JsonConvert.DeserializeObject<List<DevExtremeGroupModelDto<List<T>>>>(loadResult.Data.ToJson());
            var resultModel = new DevExtremeResponseModel<List<DevExtremeGroupModelDto<List<T>>>>(jsonModel, loadResult.TotalCount, loadResult.GroupCount);
            return resultModel;
        }

        public DevExtremeResponseModel<List<T>> GetLoadResult(DataSourceLoadOptions mapper, LoadResult loadResult)
        {
            var jsonModel = JsonConvert.DeserializeObject<List<T>>(loadResult.Data.ToJson());
            var resultModel = new DevExtremeResponseModel<List<T>>(jsonModel, loadResult.TotalCount, loadResult.GroupCount);
            return resultModel;
        }
    }
}
