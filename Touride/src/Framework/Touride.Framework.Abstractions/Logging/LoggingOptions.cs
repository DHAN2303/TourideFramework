namespace Touride.Framework.Abstractions.Logging
{

    public class LoggingOptions
    {
        public const string OptionsSection = "Touride.Framework:Logging";
        /// <summary>
        /// Log kayıtlarında bulunacak uygulama adı bilgisi.
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// Uygulama seviyesinde kullanılacak log provider.
        /// </summary>
        public LoggerType LoggerType { get; set; }
        /// <summary>
        /// Log provider ın kendi loglarını kayıt altına alıp almayacağı bilgisi.
        /// </summary>
        public bool EnableSelfLog { get; set; }
        /// <summary>
        /// Log providerın log seviyesi ayarlarının microsoftun ayarlarını ezip ezmeyeceği bilgisi.
        /// </summary>
        public bool OverrideMicrosoftLevels { get; set; }
        /// <summary>
        /// Logların consola yazılıp yazılmayacağı bilgisi.
        /// </summary>
        public bool EnableWriteToConsole { get; set; }
        /// <summary>
        /// Default log providerların uygulama seviyesinde geçersiz kılınıp kılınmayacağı bilgisi.
        /// </summary>
        public bool ClearDefaultLogProviders { get; set; }
        /// <summary>
        /// Request response verilerinin loglanıp loglanmayacağı bilgisi.
        /// </summary>
        public bool EnableRequestResponseLogging { get; set; }
        /// <summary>
        /// Logların debug a yazılıp yazılmayacağının bilgisi.
        /// </summary>
        public bool EnableWriteToDebug { get; set; }
        /// <summary>
        /// Logların dosyaya yazılıp yazılmayacağı bilgisi.
        /// </summary>
        public bool EnableWriteToFile { get; set; }
        /// <summary>
        /// Logların dosyaya yazılıp yazılmayacağı bilgisi.
        /// </summary>
        public bool EnableWriteToRabbitMq { get; set; }

        /// <summary>
        /// Logların rabbitmq yazılması durumunda kullanılacak dosyaya yazma konfigurasyonu.
        /// </summary>
        public LogToRabbitMqOptions LogToRabbitMqOptions { get; set; } = new LogToRabbitMqOptions();
        /// <summary>
        /// Logların dosyaya yazılması durumunda kullanılacak dosyaya yazma konfigurasyonu.
        /// </summary>
        public LogToFileOptions LogToFileOptions { get; set; }
        /// <summary>
        /// Logların ElasticSearch e yazılıp yazılmayacağı bilgisi.
        /// </summary>
        public bool EnableWriteToElasticSearch { get; set; }
        /// <summary>
        /// Logların ElasticSearch e yazılması durumunda kullanılacak ElasticSearch e yazmak için kullanılacak konfigurasyon.
        /// </summary>
        public LogToElasticSearchOptions LogToElasticSearchOptions { get; set; }
        /// <summary>
        /// Logların MsSql Server a yazılıp yazılmayacağı bilgisi.
        /// </summary>
        public bool EnableWriteToMsSqlServer { get; set; }
        /// <summary>
        /// Logların MsSql Server a yazılması durumunda kullanılacak MsSql e yazma konfigurasyonu.
        /// </summary>
        public LogToMsSqlServerOptions LogToMsSqlServerOptions { get; set; }
        /// <summary>
        /// Logların ApplicationInsights a yazılıp yazılmayacağı bilgisi.
        /// </summary>
        public bool EnableWriteToAzureApplicationInsights { get; set; }
        /// <summary>
        /// Logların GrayLog e yazılıp yazılmayacağı bilgisi.
        /// </summary>
        public bool EnableWriteGraylog { get; set; }
        /// <summary>
        /// Logların GrayLog e yazılması durumunda kullanılacak GrayLog e yazmak için kullanılacak konfigurasyon.
        /// </summary>
        public LogToGraylogOptions LogToGraylogOptions { get; set; }

    }
}
