using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Newtonsoft.Json;
using System;

namespace NL.HNOGames.Domoticz.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Cache
    {
        private static ISettings AppCache
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        /// <summary>
        /// Get cache from settings
        /// </summary>
        public static T GetCache<T>(String key)
        {
            try
            {
                if (String.IsNullOrEmpty(key))
                    return default(T);

                String resultCache = AppCache.GetValueOrDefault<String>(key, String.Empty);
                if (!String.IsNullOrEmpty(resultCache))
                {
                    var value = JsonConvert.DeserializeObject<T>(resultCache);
                    return value;
                }
                else
                    return default(T);
            }
            catch (Exception) { }
            return default(T);
        }

        /// <summary>
        /// Set cache from settings
        /// </summary>
        public static void SetCache(String key, object value)
        {
            if (String.IsNullOrEmpty(key) || value == null)
                return;
            AppCache.AddOrUpdateValue<string>(key, JsonConvert.SerializeObject(value));
        }
    }
}