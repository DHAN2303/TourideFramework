namespace Touride.Framework.Utilities
{
    public static class AppRule
    {
        public static void NotNull(object value, string parameter)
        {
            if (value == null)
                throw new ArgumentNullException(parameter);
        }

        public static void NotNullOrEmpty(string value, string parameter)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(parameter);
        }
    }
}