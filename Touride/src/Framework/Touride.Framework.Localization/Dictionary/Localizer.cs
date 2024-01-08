using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Text.Json;

namespace Touride.Framework.Localization.Dictionary
{
    public abstract class DictionaryLocalizer<TResource> : IStringLocalizer<TResource>
    {
        public LocalizedString this[string name] => GetString(name);

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = GetString(name);
                var value = string.Format(format ?? name, arguments);
                return new LocalizedString(name, value, resourceNotFound: format == null);
            }
        }

        public abstract string AbsolutePath { get; }


        List<LocalizationSource> LocalizationSources;


        public DictionaryLocalizer()
        {
            LocalizationSources = new List<LocalizationSource>();

            WriteAllJsonFiles();
        }

        public string GetDefaultAbsolutePath(string resourceName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", resourceName.Replace("Resource", ""));
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return LocalizationSources.Where(x => x.Culture == CultureInfo.CurrentUICulture.Name).SelectMany(x => x.Texts)
                .Select(x => new LocalizedString(x.Key, x.Value));
        }


        public LocalizedString GetString(string name)
        {
            return GetString(name, CultureInfo.CurrentUICulture.Name);
        }

        public LocalizedString GetString(string name, string cultureName)
        {
            var localizationFile = LocalizationSources.Where(x => x.Culture == cultureName).FirstOrDefault();

            if (localizationFile == null)
                return new LocalizedString(name, name, true);
            else
            {
                localizationFile.Texts.TryGetValue(name, out string value);

                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
            }
        }

        private void WriteAllJsonFiles()
        {
            var jsonFiles = Directory.GetFiles(AbsolutePath).Where(x => x.EndsWith(".json")).ToList();

            LocalizationSources.AddRange(jsonFiles.Select(x =>
                JsonSerializer.Deserialize<LocalizationSource>(File.ReadAllText(x), new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }))!);
        }
    }
}