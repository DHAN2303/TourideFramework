namespace Touride.Framework.Abstractions.Auth
{
    public class TransantionInfoProvider
    {
        public Guid Id { get; set; }
        public string? Number { get; set; }

        public string? Name { get; set; }

        public string? NameEN { get; set; }

        public string? NameRU { get; set; }

        public Guid UrlId { get; set; }

        public int TransectionSequence { get; set; }

        public string Shortcut { get; set; }

        public int ModuleNumber { get; set; }
    }
}
