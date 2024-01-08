using System.Text;
using System.Text.RegularExpressions;

namespace Touride.Framework.Utilities
{
    public class UrlSlugCreate
    {
        public static string ToUrlSlug(string value)
        {
            value = value.ToLowerInvariant();

            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);

            value = Encoding.ASCII.GetString(bytes);

            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            value = Regex.Replace(value, @"[^\w\s\p{Pd}]", "", RegexOptions.Compiled);

            value = value.Trim('-', '_');

            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }
    }
}
