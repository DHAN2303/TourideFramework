using System.Collections.Generic;

namespace Touride.Framework.Localization
{
    public class LocalizationSource
    {
        public string? Culture { get; set; }
        public Dictionary<string, string>? Texts { get; set; }

        public string? Resource { get; set; }
    }
}