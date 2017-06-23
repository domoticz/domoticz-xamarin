using NL.HNOGames.Domoticz.Models;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace NL.HNOGames.Domoticz.Helpers
{
    /// <summary>
    /// This is the Settings  class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public  class Settings
    {
        private ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string WelcomeSettingsKey = "welcome_completed";
        private const string SeverSettingsKey = "server_settings_key";

        private const string ConfigSettingsKey = "config_settings_key";
        private const string ConfigDateTimeSettingsKey = "config_datetime_settings_key";

        private const string StartUpScreenSettingsKey = "startup_settings_key";
        private const string NoSortSettingsKey = "nosort_settings_key";
        private const string EnableNotificationsSettingsKey = "gcmnotifications_settings_key";
        private const string ShowSwitchesSettingsKey = "showswitches_settings_key";
        private const string ExtraDataSettingsKey = "extradata_settings_key";

        private const string EnableScreenItems = "enablescreens_settings_key";
        
        #endregion

        /// <summary>
        /// Welcome completed
        /// </summary>
        public  bool WelcomeCompleted
        {
            get
            {
                return AppSettings.GetValueOrDefault<bool>(WelcomeSettingsKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue<bool>(WelcomeSettingsKey, value);
            }
        }

        /// <summary>
        /// Get all server settings
        /// </summary>
        public  ServerSettings ActiveServerSettings
        {
            get
            {
                string settingJSON = AppSettings.GetValueOrDefault<string>(SeverSettingsKey, null);
                if (string.IsNullOrEmpty(settingJSON))
                    return null;
                return JsonConvert.DeserializeObject<ServerSettings>(settingJSON);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(SeverSettingsKey, JsonConvert.SerializeObject(value));
            }
        }

        /// <summary>
        /// Get server config settings
        /// </summary>
        public ConfigModel ServerConfig
        {
            get
            {
                string settingJSON = AppSettings.GetValueOrDefault<string>(ConfigSettingsKey, null);
                if (string.IsNullOrEmpty(settingJSON))
                    return null;
                return JsonConvert.DeserializeObject<ConfigModel>(settingJSON);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(ConfigSettingsKey, JsonConvert.SerializeObject(value));
            }
        }

        /// <summary>
        /// set enabled/disabled screens
        /// </summary>
        public List<ScreenModel> EnabledScreens
        {
            get
            {
                string settingJSON = AppSettings.GetValueOrDefault<string>(EnableScreenItems, null);
                if (string.IsNullOrEmpty(settingJSON))
                    return null;
                return JsonConvert.DeserializeObject<List<ScreenModel>>(settingJSON);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(EnableScreenItems, JsonConvert.SerializeObject(value));
            }
        }

        /// <summary>
        /// Get server config settings
        /// </summary>
        public  DateTime ServerConfigDateTime
        {
            get
            {
                return AppSettings.GetValueOrDefault<DateTime>(ConfigDateTimeSettingsKey, DateTime.Now.AddDays(-10));
            }
            set
            {
                AppSettings.AddOrUpdateValue<DateTime>(ConfigDateTimeSettingsKey, value);
            }
        }

        /// <summary>
        /// Get server startup screen index
        /// </summary>
        public  int StartupScreen
        {
            get
            {
                return AppSettings.GetValueOrDefault<int>(StartUpScreenSettingsKey, 0);
            }
            set
            {
                AppSettings.AddOrUpdateValue<int>(StartUpScreenSettingsKey, value);
            }
        }


        /// <summary>
        /// Need to sort on alpha
        /// </summary>
        public bool NoSort
        {
            get
            {
                return AppSettings.GetValueOrDefault<bool>(NoSortSettingsKey, true);
            }
            set
            {
                AppSettings.AddOrUpdateValue<bool>(NoSortSettingsKey, value);
            }
        }

        /// <summary>
        /// Show Switches
        /// </summary>
        public bool ShowSwitches
        {
            get
            {
                return AppSettings.GetValueOrDefault<bool>(ShowSwitchesSettingsKey, true);
            }
            set
            {
                AppSettings.AddOrUpdateValue<bool>(ShowSwitchesSettingsKey, value);
            }
        }

        /// <summary>
        /// Enable Notifications
        /// </summary>
        public bool EnableNotifications
        {
            get
            {
                return AppSettings.GetValueOrDefault<bool>(EnableNotificationsSettingsKey, true);
            }
            set
            {
                AppSettings.AddOrUpdateValue<bool>(EnableNotificationsSettingsKey, value);
            }
        }
        
        /// <summary>
        /// Extra data on dashboard
        /// </summary>
        public  bool ShowExtraData
        {
            get
            {
                return AppSettings.GetValueOrDefault<bool>(ExtraDataSettingsKey, true);
            }
            set
            {
                AppSettings.AddOrUpdateValue<bool>(ExtraDataSettingsKey, value);
            }
        }
    }
}