using DevExtreme.AspNet.Data.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Touride.Framework.DevExtreme
{
    //public class DataSourceLoadOptionsBinder : IModelBinder
    //{

    //    public Task BindModelAsync(ModelBindingContext bindingContext)
    //    {
    //        var loadOptions = new DataSourceLoadOptions();
    //        DataSourceLoadOptionsParser.Parse(loadOptions, key => bindingContext.ValueProvider.GetValue(key).FirstOrDefault());
    //        bindingContext.Result = ModelBindingResult.Success(loadOptions);
    //        return Task.CompletedTask;
    //    }

    //}
    public class DataSourceLoadOptionsBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            using (var reader = new StreamReader(bindingContext.HttpContext.Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                if (body == "")
                {
                    var loadOptions = new DataSourceLoadOptions();
                    DataSourceLoadOptionsParser.Parse(loadOptions, key => bindingContext.ValueProvider.GetValue(key).FirstOrDefault());
                    bindingContext.Result = ModelBindingResult.Success(loadOptions);
                }
                else
                {
                    try
                    {
                        var loadOptions = JsonConvert.DeserializeObject<DataSourceLoadOptions>(body);
                        bindingContext.Result = ModelBindingResult.Success(loadOptions);
                    }
                    catch (Exception ex)
                    {

                        var loadOptions = new DataSourceLoadOptions();
                        DataSourceLoadOptionsParser.Parse(loadOptions, key => bindingContext.ValueProvider.GetValue(key).FirstOrDefault());
                        bindingContext.Result = ModelBindingResult.Success(loadOptions);
                    }
                }

            }
        }
    }
}
