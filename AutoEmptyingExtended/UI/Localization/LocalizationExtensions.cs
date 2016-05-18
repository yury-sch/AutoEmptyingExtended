using System;

namespace AutoEmptyingExtended.UI.Localization
{
    public static class LocalizationExtensions
    {
        public static string Translate(this String key)
        {
            var localization = LocalizationManager.Instance;
            return localization.GetString(key);

        }
    }
}