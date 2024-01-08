using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Touride.Framework.DataAudit.RabbitMq.Configuration
{
    public class RabbitMqService<TQueueName, TRouteKey> : IDisposable
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        private readonly DataAuditRabbitMqOptions _dataAuditRabbitMqOptions;
        public RabbitMqService(
            IOptions<DataAuditRabbitMqOptions> dataAuditRabbitMqOptions,
            ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _dataAuditRabbitMqOptions = dataAuditRabbitMqOptions.Value;
        }

        public IModel Connect(TQueueName queueName, TRouteKey routeKey)
        {

            _connection = _connectionFactory.CreateConnection();

            if (_channel is { IsOpen: true })
            {
                return _channel;
            }

            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(_dataAuditRabbitMqOptions.Exchange, type: _dataAuditRabbitMqOptions.ExchangeType, true, false);
            _channel.QueueDeclare(queueName.ToString(), true, false, false, null);
            _channel.QueueBind(exchange: _dataAuditRabbitMqOptions.Exchange, queue: queueName.ToString(), routingKey: routeKey.ToString());

            return _channel;
        }


        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();

            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
