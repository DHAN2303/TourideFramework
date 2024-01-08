namespace Touride.Framework.TaskScheduling.Hangfire.Configuration
{
    /// <summary>
    /// TaskScheduling Engine servisinin konfigurasyonu için kullanılır.
    /// </summary>
    public class TaskSchedulingOptions
    {
        public const string TaskSchedulingkOptionsSection = "Touride.Framework:TaskScheduling:Hangfire";

        /// <summary>
        /// TaskScheduler Engine in çalışıp çalışmayacağı bilgisi.
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Connectionstring.
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// Database adı.
        /// </summary>
        public DataBaseName DataBaseName { get; set; }
        /// <summary>
        /// Uygulamanın arka planda işleri işleyip işlemeyeceği bilgisi.
        /// </summary>
        public bool SelfBackgroundJobServer { get; set; }
        /// <summary>
        /// Kuyruk isimleri.
        /// </summary>
        public string[] Queues { get; set; }
        /// <summary>
        /// Sunucu ismi
        /// </summary>
        public string ServerName { get; set; }
        /// <summary>
        /// Uygulama kapatılırken çalışan işlerin tamamlanması için beklenecek max süre(saniye).
        /// </summary>
        public int ShutDownTimeout { get; set; }
    }
}
