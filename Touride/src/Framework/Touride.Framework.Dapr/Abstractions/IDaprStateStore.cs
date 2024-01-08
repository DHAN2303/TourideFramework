namespace Touride.Framework.Dapr.Abstractions
{
    public interface IDaprStateStore
    {
        Task SaveStateAsync<T>(string storeName, string key, T value);
        Task SaveStateAsync<T>(string storeName, string key, T value, TimeSpan ttl);
        Task<T> GetStateAsync<T>(string storeName, string key);
        Task<T> UpdateStateAsync<T>(string storeName, string key, T newStore);
        Task<T> UpdateStateAsync<T>(string storeName, string key, T newStore, TimeSpan ttl);
        Task DeleteStateAsync(string storeName, string key);
        Task<StateQueryResponse<T>> QueryStateAsync<T>(string storeName, string jsonString);
    }
}
