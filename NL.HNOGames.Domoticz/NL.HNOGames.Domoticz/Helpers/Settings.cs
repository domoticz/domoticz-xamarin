using Newtonsoft.Json;
using NL.HNOGames.Domoticz.Models;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
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
        #region Constants

        /// <summary>
        /// Defines the WelcomeSettingsKey
        /// </summary>
        private const string WelcomeSettingsKey = "welcome_completed";

        /// <summary>
        /// Defines the SeverSettingsKey
        /// </summary>
        private const string SeverSettingsKey = "server_settings_key";

        /// <summary>
        /// Defines the ConfigSettingsKey
        /// </summary>
        private const string ConfigSettingsKey = "config_settings_key";

        /// <summary>
        /// Defines the ConfigDateTimeSettingsKey
        /// </summary>
        private const string ConfigDateTimeSettingsKey = "config_datetime_settings_key";

        /// <summary>
        /// Defines the StartUpScreenSettingsKey
        /// </summary>
        private const string StartUpScreenSettingsKey = "startup_settings_key";

        /// <summary>
        /// Defines the NoSortSettingsKey
        /// </summary>
        private const string NoSortSettingsKey = "nosort_settings_key";

        /// <summary>
        /// Defines the MultiServerSettingsKey
        /// </summary>
        private const string MultiServerSettingsKey = "multiserver_settings_key";

        /// <summary>
        /// Defines the DarkThemeSettingsKey
        /// </summary>
        private const string DarkThemeSettingsKey = "darktheme_settings_key";

        /// <summary>
        /// Defines the EnableNotificationsSettingsKey
        /// </summary>
        private const string EnableNotificationsSettingsKey = "gcmnotifications_settings_key";

        /// <summary>
        /// Defines the EnableFingerprintSecuritySettingsKey
        /// </summary>
        private const string EnableFingerprintSecuritySettingsKey = "fingerprint_settings_key";

        /// <summary>
        /// Defines the ShowSwitchesSettingsKey
        /// </summary>
        private const string ShowSwitchesSettingsKey = "showswitches_settings_key";

        /// <summary>
        /// Defines the ExtraDataSettingsKey
        /// </summary>
        private const string ExtraDataSettingsKey = "extradata_settings_key";

        /// <summary>
        /// Defines the EnableScreenItemsSettingsKey
        /// </summary>
        private const string EnableScreenItemsSettingsKey = "enablescreens_settings_key";

        /// <summary>
        /// Defines the EnableTalkBackSettingsKey
        /// </summary>
        private const string EnableTalkBackSettingsKey = "enable_talkback_settings_key2";

        /// <summary>
        /// Defines the EnableQRCodeSettingsKey
        /// </summary>
        private const string EnableQRCodeSettingsKey = "enable_qrcode_settings_key2";

        /// <summary>
        /// Defines the EnableNFCSettingsKey
        /// </summary>
        private const string EnableNFCSettingsKey = "enable_nfc_settings_key2";

        /// <summary>
        /// Defines the NFCSettingsKey
        /// </summary>
        private const string NFCSettingsKey = "nfc_settings_key2";

        /// <summary>
        /// Defines the QRCodesSettingsKey
        /// </summary>
        private const string QRCodesSettingsKey = "qrcode_settings_key2";

        /// <summary>
        /// Defines the EnableSpeechSettingsKey
        /// </summary>
        private const string EnableSpeechSettingsKey = "enable_speech_settings_key2";

        /// <summary>
        /// Defines the SpeechSettingsKey
        /// </summary>
        private const string SpeechSettingsKey = "speech_settings_key2";

        /// <summary>
        /// Defines the EnableGeofenceSettingsKey
        /// </summary>
        private const string EnableGeofenceSettingsKey = "enable_geofence_settings_key2";

        /// <summary>
        /// Defines the EnableGeofenceSettingsKey
        /// </summary>
        private const string EnableGeofenceNotificationsSettingsKey = "enable_geofence_notifications_settings_key2";

        /// <summary>
        /// Defines the GeofenceSettingsKey
        /// </summary>
        private const string GeofenceSettingsKey = "geofence_settings_key2";

        /// <summary>
        /// Defines the EnableDebugInfoSettingsKey
        /// </summary>
        private const string EnableDebugInfoSettingsKey = "Enable_DebugInfo_settings_key";

        /// <summary>
        /// Defines the EnableJSONDebugInfoSettingsKey
        /// </summary>
        private const string EnableJSONDebugInfoSettingsKey = "Enable_JSON_DebugInfo_settings_key";

        /// <summary>
        /// Defines the DebugInfoSettingsKey
        /// </summary>
        private const string DebugInfoSettingsKey = "DebugInfo_settings_key";

        /// <summary>
        /// Defines the PremiumSettingsKey
        /// </summary>
        private const string PremiumSettingsKey = "premium_settings_key2";

        /// <summary>
        /// Defines the LanguageSettingsKey
        /// </summary>
        private const string LanguageSettingsKey = "language_settings_key";

        #endregion

        #region Properties

        /// <summary>
        /// Gets the AppSettings
        /// </summary>
        private ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether WelcomeCompleted
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
        /// Gets or sets the ActiveServerSettings
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
        /// Gets or sets the ServerConfig
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
        /// Gets or sets the EnabledScreens
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
        /// Gets or sets the ServerConfigDateTime
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
        /// Gets or sets the StartupScreen
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
        /// Gets or sets the SpecifiedLanguage
        /// Specified language
        /// </summary>
        public String SpecifiedLanguage
        {
            get
            {
                return AppSettings.GetValueOrDefault(LanguageSettingsKey, null);
            }
            set
            {
                AppSettings.AddOrUpdateValue(LanguageSettingsKey, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether NoSort
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
        /// Gets or sets a value indicating whether MultiServer
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
        /// Gets or sets a value indicating whether TalkBackEnabled
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
        /// Gets or sets a value indicating whether NFCEnabled
        /// Enable the NFC feature
        /// </summary>
        public bool NFCEnabled
        {
            get
            {
                return AppSettings.GetValueOrDefault(EnableNFCSettingsKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(EnableNFCSettingsKey, value);
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether QRCodeEnabled
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
        /// Gets or sets the NFCTags
        /// Specify NFC Tags objects
        /// </summary>
        public List<NFCModel> NFCTags
        {
            get
            {
                try
                {
                    string resultCache = AppSettings.GetValueOrDefault(NFCSettingsKey, string.Empty);
                    if (!string.IsNullOrEmpty(resultCache))
                    {
                        var value = JsonConvert.DeserializeObject<List<NFCModel>>(resultCache);
                        return value;
                    }
                    else
                        return new List<NFCModel>();
                }
                catch (Exception) { }
                return new List<NFCModel>();
            }
            set
            {
                if (value == null)
                    return;
                AppSettings.AddOrUpdateValue(NFCSettingsKey, JsonConvert.SerializeObject(value));
            }
        }
        
        /// <summary>
         /// Gets or sets the QRCodes
         /// Specify QR Code objects
         /// </summary>
        public List<QRCodeModel> QRCodes
        {
            get
            {
                try
                {
                    String resultCache = AppSettings.GetValueOrDefault(QRCodesSettingsKey, string.Empty);
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
        /// Gets or sets a value indicating whether SpeechEnabled
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
        /// Gets or sets the SpeechCommands
        /// Specify Speech models
        /// </summary>
        public List<SpeechModel> SpeechCommands
        {
            get
            {
                try
                {
                    String resultCache = AppSettings.GetValueOrDefault(SpeechSettingsKey, string.Empty);
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
        /// Gets or sets a value indicating whether GeofenceEnabled
        /// Enable the Geofence feature
        /// </summary>
        public bool GeofenceEnabled
        {
            get
            {
                return AppSettings.GetValueOrDefault(EnableGeofenceSettingsKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(EnableGeofenceSettingsKey, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether GeofenceEnabled
        /// Enable the Geofence feature
        /// </summary>
        public bool GeofenceNotificationsEnabled
        {
            get
            {
                return AppSettings.GetValueOrDefault(EnableGeofenceNotificationsSettingsKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(EnableGeofenceNotificationsSettingsKey, value);
            }
        }

        /// <summary>
        /// Gets or sets the Geofences
        /// Specify Geofence models
        /// </summary>
        public List<GeofenceModel> Geofences
        {
            get
            {
                try
                {
                    String resultCache = AppSettings.GetValueOrDefault(GeofenceSettingsKey, string.Empty);
                    if (!string.IsNullOrEmpty(resultCache))
                    {
                        var value = JsonConvert.DeserializeObject<List<GeofenceModel>>(resultCache);
                        return value;
                    }
                    else
                        return new List<GeofenceModel>();
                }
                catch (Exception) { }
                return new List<GeofenceModel>();
            }
            set
            {
                if (value == null)
                    return;
                AppSettings.AddOrUpdateValue(GeofenceSettingsKey, JsonConvert.SerializeObject(value));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether DarkTheme
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
        /// Gets or sets a value indicating whether ShowSwitches
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
        /// Gets or sets a value indicating whether EnableNotifications
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
        /// Gets or sets a value indicating whether EnableFingerprintSecurity
        /// Enable FingerprintSecurity
        /// </summary>
        public bool EnableFingerprintSecurity
        {
            get
            {
                return AppSettings.GetValueOrDefault(EnableFingerprintSecuritySettingsKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(EnableFingerprintSecuritySettingsKey, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether ShowExtraData
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
        /// Gets or sets a value indicating whether EnableDebugging
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
        /// Gets or sets a value indicating whether EnableJSONDebugging
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
        /// Gets or sets the DebugInfo
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
        /// Gets or sets a value indicating whether PremiumBought
        /// Premium Bought ?
        /// </summary>
        public bool PremiumBought
        {
            get
            {
#if DEBUG
                return true;
#endif

                if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
                    return true;//only support paid/premium for iOS for now
                return AppSettings.GetValueOrDefault(PremiumSettingsKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(PremiumSettingsKey, value);
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// Add new info to debug info
        /// </summary>
        /// <param name="text">The text<see cref="String"/></param>
        public void AddDebugInfo(String text)
        {
            if (EnableDebugging)
                DebugInfo = string.IsNullOrEmpty(DebugInfo) ? text : DebugInfo + Environment.NewLine + text;
        }

        #endregion
    }
}
