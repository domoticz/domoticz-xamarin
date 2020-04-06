using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
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
        #region Properties

        /// <summary>
        /// Gets the AppCache
        /// </summary>
        private static ISettings AppCache
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// Get cache from settings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key<see cref="String"/></param>
        /// <returns>The <see cref="T"/></returns>
        public static T GetCache<T>(String key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                    return default(T);

                String resultCache = AppCache.GetValueOrDefault(key, string.Empty);
                if (!string.IsNullOrEmpty(resultCache))
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
        /// <param name="key">The key<see cref="String"/></param>
        /// <param name="value">The value<see cref="object"/></param>
        public static void SetCache(String key, object value)
        {
            if (string.IsNullOrEmpty(key) || value == null)
                return;
            AppCache.AddOrUpdateValue(key, JsonConvert.SerializeObject(value));
        }

        #endregion
    }
}
