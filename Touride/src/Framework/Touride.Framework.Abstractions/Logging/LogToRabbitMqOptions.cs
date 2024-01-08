namespace Touride.Framework.Abstractions.Logging
{
    public class LogToRabbitMqOptions
    {
        public IList<string> Hostnames { get; } = new List<string>();


        public string Username { get; set; } = string.Empty;


        public string Password { get; set; } = string.Empty;


        public string Exchange { get; set; } = string.Empty;


        public string ExchangeType { get; set; } = string.Empty;


        public string DeliveryMode { get; set; } = string.Empty;


        public string RouteKey { get; set; } = string.Empty;


        public int Port { get; set; }

        public string VHost { get; set; } = string.Empty;

        public ushort Heartbeat { get; set; }

        public bool UseBackgroundThreadsForIO { get; set; }

    }
}
