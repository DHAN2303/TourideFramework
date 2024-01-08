namespace ProjectName.UI.Models
{
    public class DevExtremeGroupModelDto<T>
    {
        public DevExtremeGroupModelDto(T items)
        {
            if (items is not null)
            {
                Items = items;
            }
        }
        public string Key { get; set; }
        public T? Items { get; set; }
        public int Count { get; set; }
    }
}
