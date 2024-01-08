using System;

namespace Touride.Framework.Utilities
{
    public static class NameValueCollectionHelper
    {
        public static string StringCheck(string value)
        {
            if (value != null && value.Trim() != "undefined" && value.Trim() != "")
                return value.Trim();
            else
                return "";
        }

        public static int? IntegerCheck(string value)
        {
            if (value != null && value.Trim() != "undefined" && value.Trim() != "")
                return Convert.ToInt32(value.Trim());
            else
                return null;
        }

        public static long? LongCheck(string value)
        {
            if (value != null && value.Trim() != "undefined" && value.Trim() != "")
                return Convert.ToInt64(value.Trim());
            else
                return null;
        }

        public static int SayfaNoCheck(string value)
        {
            if (value != null && value.Trim() != "undefined" && value.Trim() != "")
                return Convert.ToInt32(value.Trim());
            else
                return 0;
        }

        public static int SayfaBasinaKayitSayisiCheck(string value)
        {
            if (value != null && value.Trim() != "undefined" && value.Trim() != "")
                return Convert.ToInt32(value.Trim());
            else
                return 15;
        }

        public static bool? BoolCheck(string value)
        {
            if (value != null && value.Trim() != "undefined" && value.Trim() != "")
                return Convert.ToBoolean(value);
            else
                return null;
        }

        public static DateTime? DateTimeCheck(string value)
        {
            if (value != null && value.Trim() != "undefined" && value.Trim() != "")
                return DateTime.Parse(value);
            else
                return null;
        }
    }
}
