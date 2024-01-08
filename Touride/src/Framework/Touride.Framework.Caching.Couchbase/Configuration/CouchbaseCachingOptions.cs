using Touride.Framework.Caching.Common.Configuration;

namespace Touride.Framework.Caching.Couchbase.Configuration
{
    /// <summary>
    /// Couchbase cache konfigurasyonu için kullanılır.
    /// </summary>
    public class CouchbaseCachingOptions : CachingOptions
    {
        /// <summary>
        /// Configuration Section.
        /// </summary>
        public const string ConfigurationSection = "OtokocERP.Framework:Caching:Couchbase";

        /// <summary>
        /// Couchbase Configuration Key.
        /// </summary>
        public string ConfigurationKey { get; set; } = "couchbaseCache";
        /// <summary>
        /// Couchbase sunucularının adresleri.
        /// </summary>
        public List<string> Servers { get; set; } = new List<string> { "http://localhost:8091" };
        /// <summary>
        /// Couchbase kullanıcı adı.
        /// </summary>
        public string Username { get; set; } = "Administrator";
        /// <summary>
        /// Couchbase şifresi.
        /// </summary>
        public string Password { get; set; } = "password";
        /// <summary>
        /// Couchbase kullanılacak olan bucket adı.
        /// </summary>
        public string BucketName { get; set; } = "default";
        /// <summary>
        /// SSL kullanımı.
        /// </summary>
        public bool UseSsl { get; set; } = false;
    }
}