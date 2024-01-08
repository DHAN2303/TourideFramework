using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Touride.Framework.DataAudit.RabbitMq.Configuration
{
    public class RabbitMqPublisher<TQueueName, TRouteKey> where TQueueName : class
                                                          where TRouteKey : class
    {
        private readonly RabbitMqService<TQueueName, TRouteKey> _rabbitMqService;
        private readonly DataAuditRabbitMqOptions _dataAuditRabbitMqOptions;

        public RabbitMqPublisher(RabbitMqService<TQueueName, TRouteKey> rabbitMqService,
                        IOptions<DataAuditRabbitMqOptions> dataAuditRabbitMqOptions)
        {
            _rabbitMqService = rabbitMqService;
            _dataAuditRabbitMqOptions = dataAuditRabbitMqOptions.Value;
        }

        public void Publish(object message, TQueueName queueName, TRouteKey routeKey)
        {
            var channel = _rabbitMqService.Connect(queueName, routeKey);

            var bodyString = JsonSerializer.Serialize(message);

            var bodyByte = Encoding.UTF8.GetBytes(bodyString);
            var properties = channel.CreateBasicProperties();

            properties.Persistent = true;

            channel.BasicPublish(exchange: _dataAuditRabbitMqOptions.Exchange,
                routingKey: routeKey.ToString(), true, basicProperties: properties, body: bodyByte);
        }
    }
}
