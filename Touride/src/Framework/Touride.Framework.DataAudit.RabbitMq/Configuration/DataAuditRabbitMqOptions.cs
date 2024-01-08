namespace Touride.Framework.DataAudit.RabbitMq.Configuration
{
    public class DataAuditRabbitMqOptions
    {
        /// <summary>
        /// DataAuditElasticOptions ın konfigurasyonda bulunduğu section bilgisi. 
        /// </summary>
        /// 
        public const string DataAuditRabbitMqOptionsSection = "Touride.Framework:DataAudit:RabbitMq";
        public virtual string Hostname { get; set; } = string.Empty;
        public virtual string Username { get; set; } = string.Empty;
        public virtual string Password { get; set; } = string.Empty;
        public virtual string Exchange { get; set; } = string.Empty;
        public virtual string QueueName { get; set; } = string.Empty;
        public virtual string ExchangeType { get; set; } = string.Empty;
        public virtual string DeliveryMode { get; set; } = string.Empty;
        public virtual string RouteKey { get; set; } = string.Empty;
        public virtual int Port { get; set; }
        public virtual string VHost { get; set; } = string.Empty;
        public virtual bool RunAsTransactional { get; set; } = true;
    }
}
