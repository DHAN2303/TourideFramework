namespace Touride.Framework.Client.Abstractions
{
    public interface IHttpClientProvider
    {
        //Task<T> ExecuteAsync<TService, T>(Func<TService, Task<T>> func);
        Task<T> GetAsync<T>(string clientName, string apiUrl, string apiKey, string apiSecret, object parameter = null, CancellationToken token = default);
        Task<T> GetAsync<T>(string clientName, string apiUrl, object parameter = null, CancellationToken token = default);
        Task<T> GetAsync<T>(string clientName, string apiUrl, string customToken, object parameter = null, CancellationToken token = default);


        //Task<T[]> GetMultipleItemsRequest<T>(string apiUrl, CancellationToken token = default);
        Task<TResult> PostAsync<T, TResult>(string clientName, string apiUrl, T postObject, string apiKey, string apiSecret, CancellationToken token = default);
        Task<TResult> PostAsync<T, TResult>(string clientName, string apiUrl, T postObject, CancellationToken token = default);
        Task<TResult> PostAsync<T, TResult>(string clientName, string apiUrl, T postObject, string customToken, CancellationToken token = default);

        Task PutAsync<T>(string clientName, string apiUrl, T putObject, string apiKey, string apiSecret, CancellationToken token = default);
        Task PutAsync<T>(string clientName, string apiUrl, T putObject, CancellationToken token = default);
        Task PutAsync<T>(string clientName, string apiUrl, T putObject, string customToken, CancellationToken token = default);
        Task<TResult> PutAsync<T, TResult>(string clientName, string apiUrl, T putObject, CancellationToken token = default);


        Task DeleteAsync(string clientName, string apiUrl, string apiKey, string apiSecret, CancellationToken token = default);
        Task DeleteAsync(string clientName, string apiUrl, CancellationToken token = default);
        Task DeleteAsync<T>(string clientName, string apiUrl, T putObject, string customToken, CancellationToken token = default);


    }
}
