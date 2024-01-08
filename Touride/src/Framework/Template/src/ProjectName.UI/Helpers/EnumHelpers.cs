﻿namespace ProjectName.UI.Helpers
{
    public static class EnumHelpers
    {
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
