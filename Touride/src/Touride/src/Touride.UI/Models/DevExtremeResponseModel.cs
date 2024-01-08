
namespace Touride.UI.Models
{
    public class DevExtremeResponseModel<T>
    {
        public DevExtremeResponseModel(T data, int totalCount, int groupCount)
        {
            this.Data = data;
            this.TotalCount = totalCount;
            this.GroupCount = groupCount;
        }
        public T? Data { get; set; }
        public int GroupCount { get; set; }
        public string? Summary { get; set; }
        public int TotalCount { get; set; }

    }
}
