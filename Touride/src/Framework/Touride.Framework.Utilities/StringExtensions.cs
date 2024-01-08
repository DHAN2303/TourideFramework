using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Touride.Framework.Utilities
{
    public static class StringExtensions
    {

        private static readonly char[] ToCSharpEscapeChars;

        private static readonly char[] CleanForXssChars = "*?(){}[];:%<>/\\|&'\"".ToCharArray();


        static StringExtensions()
        {
            var escapes = new[] { "\aa", "\bb", "\ff", "\nn", "\rr", "\tt", "\vv", "\"\"", "\\\\", "??", "\00" };
            ToCSharpEscapeChars = new char[escapes.Max(e => e[0]) + 1];
            foreach (var escape in escapes)
            {
                ToCSharpEscapeChars[escape[0]] = escape[1];
            }
        }
        public static string CleanForXss(this string input, params char[] ignoreFromClean)
        {
            input = input.StripHtml();

            return input.ExceptChars(new HashSet<char>(CleanForXssChars.Except(ignoreFromClean)));
        }
        public static string StripHtml(this string text)
        {
            const string pattern = @"<(.|\n)*?>";
            return Regex.Replace(text, pattern, string.Empty, RegexOptions.Compiled);
        }
        public static string ExceptChars(this string str, HashSet<char> toExclude)
        {
            var sb = new StringBuilder(str.Length);
            foreach (var c in str.Where(c => toExclude.Contains(c) == false))
            {
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}