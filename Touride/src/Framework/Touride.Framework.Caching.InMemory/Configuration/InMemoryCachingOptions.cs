﻿using Touride.Framework.Caching.Common.Configuration;

namespace Touride.Framework.Caching.InMemory.Configuration
{
    /// <summary>
    /// InMemory cache konfigurasyonu için kullanılır.
    /// </summary>
    public class InMemoryCachingOptions : CachingOptions
    {
        /// <summary>
        /// Configuration Section.
        /// </summary>
        public const string ConfigurationSection = "Touride.Framework:Caching:InMemory";
        /// <summary>
        /// Serializer olarak json serializer kullanılması için kullanılır.
        /// </summary>
        public virtual bool EnableJsonSerializer { get; set; }
    }
}
