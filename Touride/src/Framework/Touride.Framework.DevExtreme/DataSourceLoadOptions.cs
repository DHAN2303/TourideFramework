using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;

namespace Touride.Framework.DevExtreme
{
    [ModelBinder(BinderType = typeof(DataSourceLoadOptionsBinder))]
    public class DataSourceLoadOptions : DataSourceLoadOptionsBase
    {
    }

}
