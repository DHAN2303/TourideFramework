namespace Touride.Framework.Data.Entities
{

    public class PagedResponse<T>
    {
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public IList<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public long Total { get; set; }

    }

    public class MetaData
    {
        public int Page { get; set; }
        public int Pages { get; set; }
        public int Perpage { get; set; }
        public int Total { get; set; }
        public string Sort { get; set; }
        public string Field { get; set; }
    }
}
