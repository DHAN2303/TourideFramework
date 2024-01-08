using Touride.Framework.Dapr.Abstractions;

public class DaprStateStore : IDaprStateStore
{
    private readonly ILogger<DaprStateStore> _logger;
    public DaprStateStore(ILogger<DaprStateStore> logger)
    {
        _logger = logger;
    }
    public async Task SaveStateAsync<T>(string storeName, string key, T value, TimeSpan ttl)
    {
        _logger.LogInformation($"Saving state with key '{key}' and TTL '{ttl}' to store '{storeName}'.");

        using var daprClient = new DaprClientBuilder().Build();

        if (ttl != null)
        {
            var metadata = new Dictionary<string, string>
            {
                { "ttlInSeconds", ttl.TotalSeconds.ToString() }
            };

            await daprClient.SaveStateAsync(storeName, key, value, metadata: metadata);
        }
        else
        {
            await daprClient.SaveStateAsync(storeName, key, value);
        }

        _logger.LogInformation($"State with key '{key}' saved successfully.");
    }

    public async Task SaveStateAsync<T>(string storeName, string key, T value)
    {
        _logger.LogInformation($"Saving state with key '{key}' to store '{storeName}'.");

        using var daprClient = new DaprClientBuilder().Build();

        await daprClient.SaveStateAsync(storeName, key, value);

        _logger.LogInformation($"State with key '{key}' saved successfully.");
    }

    public async Task<T> GetStateAsync<T>(string storeName, string key)
    {
        _logger.LogInformation($"Getting state with key '{key}' from store '{storeName}'.");

        using var daprClient = new DaprClientBuilder().Build();

        var result = await daprClient.GetStateAsync<T>(storeName, key);

        _logger.LogInformation($"State with key '{key}' retrieved successfully.");

        return result;
    }

    public async Task<T> UpdateStateAsync<T>(string storeName, string key, T newStore)
    {
        _logger.LogInformation($"Updating state with key '{key}' in store '{storeName}'.");

        using var daprClient = new DaprClientBuilder().Build();

        var state = await daprClient.GetStateEntryAsync<T>(storeName, key);

        state.Value = newStore;

        await state.SaveAsync();

        _logger.LogInformation($"State with key '{key}' updated successfully.");

        return newStore;
    }

    public async Task<T> UpdateStateAsync<T>(string storeName, string key, T newStore, TimeSpan ttl)
    {
        _logger.LogInformation($"Updating state with key '{key}' and TTL '{ttl}' in store '{storeName}'.");

        using var daprClient = new DaprClientBuilder().Build();

        var metadata = new Dictionary<string, string>
        {
            { "ttlInSeconds", ttl.TotalSeconds.ToString() }
        };

        var state = await daprClient.GetStateEntryAsync<T>(storeName, key, metadata: metadata);

        state.Value = newStore;

        await state.SaveAsync();

        _logger.LogInformation($"State with key '{key}' updated successfully.");

        return newStore;
    }

    public async Task<StateQueryResponse<T>> QueryStateAsync<T>(string storeName, string jsonString)
    {
        _logger.LogInformation($"Querying state in store '{storeName}' with query string '{jsonString}'.");

        using var daprClient = new DaprClientBuilder().Build();

        var response = await daprClient.QueryStateAsync<T>(storeName, jsonString);

        _logger.LogInformation($"State query completed successfully.");

        return response;
    }

    public async Task DeleteStateAsync(string storeName, string key)
    {
        _logger.LogInformation($"Deleting state with key '{key}' from store '{storeName}'.");

        using var daprClient = new DaprClientBuilder().Build();
        await daprClient.DeleteStateAsync(storeName, key);

        _logger.LogInformation($"State with key '{key}' deleted successfully.");
    }
}
