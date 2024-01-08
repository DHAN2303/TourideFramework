using Touride.Framework.Utilities;

namespace Touride.Framework.DevExtreme
{
    public class DataSourceLoadPascalCaseConvert
    {
        public static DataSourceLoadOptions DxPascalCaseConvert(DataSourceLoadOptions loadOptions)
        {
            if (loadOptions.Group is not null)
            {
                foreach (var item in loadOptions.Group)
                {
                    if (item.Selector == "id")
                    {
                        item.Selector = "Id";
                    }
                    else
                    {
                        item.Selector = PascalCaseConvert.ToPascalCase(item.Selector);
                    }
                }
            }

            return loadOptions;
        }
    }
}
