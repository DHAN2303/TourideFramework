using Touride.Abstraction.Services;
using Touride.Framework.Client.Abstractions;
using Touride.Framework.Dapr.Abstractions;

namespace Touride.Application.Services
{
    public class DataCollectorService : IDataCollectorService
    {
        private readonly IHttpClientProvider _httpClient;
        private readonly IDaprStateStore _daprStateStore;
        public DataCollectorService(IHttpClientProvider httpClient, IDaprStateStore daprStateStore)
        {
            _httpClient = httpClient;
            _daprStateStore = daprStateStore;
        }

        public async Task GetandSave()
        {
            Console.WriteLine("Hangfire çalıştı");
            //var res = await _httpClient.GetAsync<TestDto>("TestApi1", "/v1/sorting");

            //if (res != null)
            //{

            //    await _daprStateStore.SaveStateAsync("sortingData", res);

            //}
        }
    }
}
