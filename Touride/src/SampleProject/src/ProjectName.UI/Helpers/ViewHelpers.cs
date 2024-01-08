namespace ProjectName.UI.Helpers
{
    public static class ViewHelpers
    {
        public static string GetComponentName<T>()
        {
            return typeof(T).Name.Replace("ViewComponent", "");
        }


        public static string ShortenThreeDot(this string text, int length)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            if (text.Length <= length)
            {
                return text;
            }

            return $"{text.Substring(0, length)}...";
        }

    }
}
