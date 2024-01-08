using Touride.Framework.EventBus.Abstractions;
using Touride.Framework.EventBus.Events;

namespace Touride.Framework.EventBus
{
    public class DaprEventBus : IEventBus
    {
        private string _pubSubName;

        private readonly DaprClient _dapr;
        private readonly ILogger _logger;

        public DaprEventBus(DaprClient dapr, ILogger<DaprEventBus> logger, string PubSubName)
        {
            _dapr = dapr;
            _logger = logger;
            _pubSubName = PubSubName;
        }

        public async Task PublishAsync(IntegrationEvent integrationEvent)
        {
            var topicName = integrationEvent.GetType().Name;

            _logger.LogInformation(
                "Publishing event {@Event} to {_pubSubName}.{TopicName}",
                integrationEvent,
                _pubSubName,
                topicName);

            // We need to make sure that we pass the concrete type to PublishEventAsync,
            // which can be accomplished by casting the event to dynamic. This ensures
            // that all event fields are properly serialized.
            await _dapr.PublishEventAsync(_pubSubName, topicName, (object)integrationEvent);
        }
    }
}