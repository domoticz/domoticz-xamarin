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
    public class Settings
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
        private const string MultiServerSettingsKey = "multiserver_settings_key";
        private const string DarkThemeSettingsKey = "darktheme_settings_key";
        private const string EnableNotificationsSettingsKey = "gcmnotifications_settings_key";
        private const string ShowSwitchesSettingsKey = "showswitches_settings_key";
        private const string ExtraDataSettingsKey = "extradata_settings_key";

        private const string EnableScreenItemsSettingsKey = "enablescreens_settings_key";
        private const string EnableTalkBackSettingsKey = "enable_talkback_settings_key2";

        private const string EnableQRCodeSettingsKey = "enable_qrcode_settings_key2";
        private const string QRCodesSettingsKey = "qrcode_settings_key2";

        private const string EnableSpeechSettingsKey = "enable_speech_settings_key2";
        private const string SpeechSettingsKey = "speech_settings_key2";

        private const string EnableDebugInfoSettingsKey = "Enable_DebugInfo_settings_key";
        private const string EnableJSONDebugInfoSettingsKey = "Enable_JSON_DebugInfo_settings_key";
        private const string DebugInfoSettingsKey = "DebugInfo_settings_key";

        private const string PremiumSettingsKey = "premium_settings_key2";

        #endregion

        /// <summary>
        /// Welcome completed
        /// </summary>
        public bool WelcomeCompleted
        {
            get
            {
                return AppSettings.GetValueOrDefault(WelcomeSettingsKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(WelcomeSettingsKey, value);
            }
        }

        /// <summary>
        /// Get all server settings
        /// </summary>
        public ServerSettings ActiveServerSettings
        {
            get
            {
                string settingJSON = AppSettings.GetValueOrDefault(SeverSettingsKey, null);
                if (string.IsNullOrEmpty(settingJSON))
                    return null;
                return JsonConvert.DeserializeObject<ServerSettings>(settingJSON);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SeverSettingsKey, JsonConvert.SerializeObject(value));
            }
        }

        /// <summary>
        /// Get server config settings
        /// </summary>
        public ConfigModel ServerConfig
        {
            get
            {
                string settingJSON = AppSettings.GetValueOrDefault(ConfigSettingsKey, null);
                if (string.IsNullOrEmpty(settingJSON))
                    return null;
                return JsonConvert.DeserializeObject<ConfigModel>(settingJSON);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ConfigSettingsKey, JsonConvert.SerializeObject(value));
            }
        }

        /// <summary>
        /// set enabled/disabled screens
        /// </summary>
        public List<ScreenModel> EnabledScreens
        {
            get
            {
                string settingJSON = AppSettings.GetValueOrDefault(EnableScreenItemsSettingsKey, null);
                if (string.IsNullOrEmpty(settingJSON))
                    return null;
                return JsonConvert.DeserializeObject<List<ScreenModel>>(settingJSON);
            }
            set
            {
                AppSettings.AddOrUpdateValue(EnableScreenItemsSettingsKey, JsonConvert.SerializeObject(value));
            }
        }

        /// <summary>
        /// Get server config settings
        /// </summary>
        public DateTime ServerConfigDateTime
        {
            get
            {
                return AppSettings.GetValueOrDefault(ConfigDateTimeSettingsKey, DateTime.Now.AddDays(-10));
            }
            set
            {
                AppSettings.AddOrUpdateValue(ConfigDateTimeSettingsKey, value);
            }
        }

        /// <summary>
        /// Get server startup screen index
        /// </summary>
        public int StartupScreen
        {
            get
            {
                return AppSettings.GetValueOrDefault(StartUpScreenSettingsKey, 0);
            }
            set
            {
                AppSettings.AddOrUpdateValue(StartUpScreenSettingsKey, value);
            }
        }

        /// <summary>
        /// Need to sort on alpha
        /// </summary>
        public bool NoSort
        {
            get
            {
                return AppSettings.GetValueOrDefault(NoSortSettingsKey, true);
            }
            set
            {
                AppSettings.AddOrUpdateValue(NoSortSettingsKey, value);
            }
        }

        /// <summary>
        /// enable/disable multi server support
        /// </summary>
        public bool MultiServer
        {
            get
            {
                return AppSettings.GetValueOrDefault(MultiServerSettingsKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(MultiServerSettingsKey, value);
            }
        }

        /// <summary>
        /// Is the talk back feature enabled?
        /// </summary>
        public bool TalkBackEnabled
        {
            get
            {
                return AppSettings.GetValueOrDefault(EnableTalkBackSettingsKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(EnableTalkBackSettingsKey, value);
            }
        }

        /// <summary>
        /// Enable the QRCode feature
        /// </summary>
        public bool QRCodeEnabled
        {
            get
            {
                return AppSettings.GetValueOrDefault(EnableQRCodeSettingsKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(EnableQRCodeSettingsKey, value);
            }
        }

        /// <summary>
        /// Specify QR Code objects
        /// </summary>
        public List<QRCodeModel> QRCodes
        {
            get
            {
                try
                {
                    String resultCache = AppSettings.GetValueOrDefault(QRCodesSettingsKey, String.Empty);
                    if (!string.IsNullOrEmpty(resultCache))
                    {
                        var value = JsonConvert.DeserializeObject<List<QRCodeModel>>(resultCache);
                        return value;
                    }
                    else
                        return new List<QRCodeModel>();
                }
                catch (Exception) { }
                return new List<QRCodeModel>();
            }
            set
            {
                if (value == null)
                    return;
                AppSettings.AddOrUpdateValue(QRCodesSettingsKey, JsonConvert.SerializeObject(value));
            }
        }


        /// <summary>
        /// Enable the Speech feature
        /// </summary>
        public bool SpeechEnabled
        {
            get
            {
                return AppSettings.GetValueOrDefault(EnableSpeechSettingsKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(EnableSpeechSettingsKey, value);
            }
        }


        /// <summary>
        /// Specify Speech models
        /// </summary>
        public List<SpeechModel> SpeechCommands
        {
            get
            {
                try
                {
                    String resultCache = AppSettings.GetValueOrDefault(SpeechSettingsKey, String.Empty);
                    if (!string.IsNullOrEmpty(resultCache))
                    {
                        var value = JsonConvert.DeserializeObject<List<SpeechModel>>(resultCache);
                        return value;
                    }
                    else
                        return new List<SpeechModel>();
                }
                catch (Exception) { }
                return new List<SpeechModel>();
            }
            set
            {
                if (value == null)
                    return;
                AppSettings.AddOrUpdateValue(SpeechSettingsKey, JsonConvert.SerializeObject(value));
            }
        }

        /// <summary>
        /// Dark theme
        /// </summary>
        public bool DarkTheme
        {
            get
            {
                return AppSettings.GetValueOrDefault(DarkThemeSettingsKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(DarkThemeSettingsKey, value);
            }
        }

        /// <summary>
        /// Show Switches
        /// </summary>
        public bool ShowSwitches
        {
            get
            {
                return AppSettings.GetValueOrDefault(ShowSwitchesSettingsKey, true);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ShowSwitchesSettingsKey, value);
            }
        }

        /// <summary>
        /// Enable Notifications
        /// </summary>
        public bool EnableNotifications
        {
            get
            {
                return AppSettings.GetValueOrDefault(EnableNotificationsSettingsKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(EnableNotificationsSettingsKey, value);
            }
        }

        /// <summary>
        /// Extra data on dashboard
        /// </summary>
        public bool ShowExtraData
        {
            get
            {
                return AppSettings.GetValueOrDefault(ExtraDataSettingsKey, true);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ExtraDataSettingsKey, value);
            }
        }

        /// <summary>
        /// Enable Debugging
        /// </summary>
        public bool EnableDebugging
        {
            get
            {
                return AppSettings.GetValueOrDefault(EnableDebugInfoSettingsKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(EnableDebugInfoSettingsKey, value);
            }
        }

        /// <summary>
        /// Enable JSON Debugging
        /// </summary>
        public bool EnableJSONDebugging
        {
            get
            {
                return AppSettings.GetValueOrDefault(EnableJSONDebugInfoSettingsKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(EnableJSONDebugInfoSettingsKey, value);
            }
        }

        /// <summary>
        /// Debug Trace
        /// </summary>
        public String DebugInfo
        {
            get
            {
                return AppSettings.GetValueOrDefault(DebugInfoSettingsKey, "");
            }
            set
            {
                AppSettings.AddOrUpdateValue(DebugInfoSettingsKey, value);
            }
        }

        /// <summary>
        /// Add new info to debug info
        /// </summary>
        public void AddDebugInfo(String text)
        {
            if(EnableDebugging)
                DebugInfo = string.IsNullOrEmpty(DebugInfo) ? text : DebugInfo + Environment.NewLine + text;
        }



        /// <summary>
        /// Premium Bought ?
        /// </summary>
        public bool PremiumBought
        {
            get
            {
                if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
                    return true;//only support paid/premium for iOS for now
                return AppSettings.GetValueOrDefault(PremiumSettingsKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(PremiumSettingsKey, value);
            }
        }
    }
}