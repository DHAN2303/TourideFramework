using ProjectName.UI;

namespace ProjectName.UI.Mappers
{
    public class DataSourceLoadOptionsMapper
    {
        public static DataSourceLoadOptions DataSourcemap(Touride.Framework.DevExtreme.DataSourceLoadOptions loadOptions)
        {
            var group = loadOptions.Group?.Select(p => new GroupingInfo { Desc = p.Desc }).ToList();
            return new DataSourceLoadOptions
            {
                AllowAsyncOverSync = loadOptions.AllowAsyncOverSync,
                DefaultSort = loadOptions.DefaultSort,
                ExpandLinqSumType = loadOptions.ExpandLinqSumType,
                Filter = loadOptions.Filter as ICollection<object>,
                Group = loadOptions.Group?.Select(p => new GroupingInfo
                {
                    Desc = p.Desc,
                    GroupInterval = p.GroupInterval,
                    IsExpanded = p.IsExpanded,
                    Selector = p.Selector,
                }).ToList(),
                Select = loadOptions.Select,
                GroupSummary = loadOptions.GroupSummary?.Select(p => new SummaryInfo
                {
                    Selector = p.Selector,
                    SummaryType = p.SummaryType
                }).ToList(),
                IsCountQuery = loadOptions.IsCountQuery,
                IsSummaryQuery = loadOptions.IsSummaryQuery,
                PaginateViaPrimaryKey = loadOptions.PaginateViaPrimaryKey,
                PreSelect = loadOptions.PreSelect,
                PrimaryKey = loadOptions.PrimaryKey,
                RemoteGrouping = loadOptions.RemoteGrouping,
                RemoteSelect = loadOptions.RemoteSelect,
                RequireGroupCount = loadOptions.RequireGroupCount,
                RequireTotalCount = loadOptions.RequireTotalCount,
                Skip = loadOptions.Skip,
                Sort = loadOptions.Sort?.Select(p => new SortingInfo
                {
                    Desc = p.Desc,
                    Selector = p.Selector
                }).ToList(),
                SortByPrimaryKey = loadOptions.SortByPrimaryKey,
                StringToLower = loadOptions.StringToLower,
                Take = loadOptions.Take,
                TotalSummary = loadOptions.TotalSummary?.Select(p => new SummaryInfo { SummaryType = p.SummaryType }).ToList()
            };
        }
    }
}
