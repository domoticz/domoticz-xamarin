using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NL.HNOGames.Domoticz.Helpers;
using NL.HNOGames.Domoticz.Models;
using Plugin.DeviceInfo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace NL.HNOGames.Domoticz.Data
{
    /// <summary>
    /// Data service class handles all the data calls to Axis
    /// </summary>
    public class DataService
    {
        public HttpResponseMessage response = null;
        public ServerSettings server;

        /// <summary>
        /// Constructor
        /// </summary>
        public DataService()
        {
        }

        private readonly JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
        };

        /// <summary>
        /// Get server App.AppSettings
        /// </summary>
        public ServerSettings Server { get => server; set => server = value; }

        #region Data

        /// <summary>
        /// Domoticz version
        /// </summary>
        public async Task<VersionModel> GetVersion()
        {
            if (Server == null)
                return null;
            String url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.VERSION);
            try
            {
                response = await App.ConnectionService.client.GetAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                    return JsonConvert.DeserializeObject<VersionModel>(content);
                }
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }

            return null;
        }


        /// <summary>
        /// Domoticz get Config and save in prefs
        /// </summary>
        public async void RefreshConfig()
        {
            if (App.AppSettings.ServerConfigDateTime < DateTime.Now.AddDays(-3))
            {
                //refresh needed of the config
                if (Server == null)
                    return;

                String url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.System.CONFIG);
                try
                {
                    response = await App.ConnectionService.client.GetAsync(new Uri(url));
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                        App.AppSettings.ServerConfig = JsonConvert.DeserializeObject<ConfigModel>(content);
                        App.AppSettings.ServerConfigDateTime = DateTime.Now;
                    }
                }
                catch (Exception ex)
                {
                    App.AddLog(ex.Message);
                }
            }
        }

        /// <summary>
        /// Domoticz get all plans
        /// </summary>
        public async Task<PlansModel> GetPlans()
        {
            if (Server == null)
                return null;
            String url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.PLANS);

            try
            {
                response = await App.ConnectionService.client.GetAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                    return JsonConvert.DeserializeObject<PlansModel>(content);
                }
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Domoticz get all devices
        /// </summary>
        public async Task<DevicesModel> GetDevices(int plan, String filter)
        {
            if (Server == null)
                return null;
            String url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.DEVICES);
            if (!String.IsNullOrEmpty(filter))
                url = url.Replace("filter=all", "filter=" + filter);
            if (plan > 0)
                url += "&plan=" + plan;

            try
            {
                response = await App.ConnectionService.client.GetAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                    return JsonConvert.DeserializeObject<DevicesModel>(content);
                }
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Domoticz get notifications for a device
        /// </summary>
        public async Task<NotificationModel> GetNotifications(Models.Device device)
        {
            if (Server == null)
                return null;
            String url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Notification.NOTIFICATION) + device.idx;

            try
            {
                response = await App.ConnectionService.client.GetAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                    return JsonConvert.DeserializeObject<NotificationModel>(content);
                }
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Domoticz get timers for a device
        /// </summary>
        public async Task<TimerModel> GetTimers(String idx)
        {
            if (Server == null)
                return null;
            String url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.SWITCHTIMER) + idx;

            try
            {
                response = await App.ConnectionService.client.GetAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                    return JsonConvert.DeserializeObject<TimerModel>(content);
                }
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Domoticz get logs for the server
        /// </summary>
        public async Task<ServerLogsModel> GetServerLogs()
        {
            if (Server == null)
                return null;
            String url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Log.GET_LOG);

            try
            {
                response = await App.ConnectionService.client.GetAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                    return JsonConvert.DeserializeObject<ServerLogsModel>(content);
                }
            }

            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }
            return null;
        }


        /// <summary>
        /// Domoticz get user variables for this server
        /// </summary>
        public async Task<UserVariableModel> GetUserVariables()
        {
            if (Server == null)
                return null;
            String url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.System.USERVARIABLES);

            try
            {
                response = await App.ConnectionService.client.GetAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                    return JsonConvert.DeserializeObject<UserVariableModel>(content);
                }
            }

            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Domoticz get events
        /// </summary>
        public async Task<EventModel> GetEvents()
        {
            if (Server == null)
                return null;
            String url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.System.EVENTS);

            try
            {
                response = await App.ConnectionService.client.GetAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                    return JsonConvert.DeserializeObject<EventModel>(content);
                }
            }

            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Domoticz get logs for a device
        /// </summary>
        public async Task<LogModel> GetLogs(string deviceIDX, bool isScene = false, bool textLog = false)
        {
            if (Server == null)
                return null;
            String url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.SWITCHLOG) + deviceIDX;
            if (isScene)
                url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.SCENELOG) + deviceIDX;
            else if (textLog)
                url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.TEXTLOG) + deviceIDX;

            try
            {
                response = await App.ConnectionService.client.GetAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                    return JsonConvert.DeserializeObject<LogModel>(content);
                }
            }

            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }
            return null;

        }

        /// <summary>
        /// Domoticz get all scene and groups
        /// </summary>
        public async Task<SceneModel> GetScenes(String filter)
        {
            if (Server == null)
                return null;
            String url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.SCENES);
            if (!String.IsNullOrEmpty(filter))
                url = url.Replace("filter=all", "filter=" + filter);

            try
            {
                response = await App.ConnectionService.client.GetAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                    return JsonConvert.DeserializeObject<SceneModel>(content);
                }
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Domoticz get all temperature devices
        /// </summary>
        public async Task<DevicesModel> GetTemperature(String filter)
        {
            if (Server == null)
                return null;
            String url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.TEMPERATURE);
            if (!String.IsNullOrEmpty(filter))
                url = url.Replace("filter=all", "filter=" + filter);

            try
            {
                response = await App.ConnectionService.client.GetAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                    return JsonConvert.DeserializeObject<DevicesModel>(content);
                }
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Domoticz get all weather devices
        /// </summary>
        public async Task<DevicesModel> GetWeather(String filter)
        {
            if (Server == null)
                return null;
            String url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.WEATHER);
            if (!String.IsNullOrEmpty(filter))
                url = url.Replace("filter=all", "filter=" + filter);

            try
            {
                response = await App.ConnectionService.client.GetAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                    return JsonConvert.DeserializeObject<DevicesModel>(content);
                }
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Domoticz get all utilities devices
        /// </summary>
        public async Task<DevicesModel> getUtilities(String filter)
        {
            if (Server == null)
                return null;
            String url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.UTILITIES);
            if (!String.IsNullOrEmpty(filter))
                url = url.Replace("filter=all", "filter=" + filter);

            try
            {
                response = await App.ConnectionService.client.GetAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                    return JsonConvert.DeserializeObject<DevicesModel>(content);
                }
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Domoticz get favorite devices
        /// </summary>
        public async Task<DevicesModel> GetFavorites(String filter)
        {
            if (Server == null)
                return null;
            String url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.FAVORITES);
            if (!String.IsNullOrEmpty(filter))
                url = url.Replace("filter=all", "filter=" + filter);

            try
            {
                response = await App.ConnectionService.client.GetAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                    return JsonConvert.DeserializeObject<DevicesModel>(content);
                }
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }
            return null;
        }


        /// <summary>
        /// Domoticz get Graph data
        /// </summary>
        public async Task<GraphModel> GetGraphData(String idx, String type, ConstantValues.GraphRange range = ConstantValues.GraphRange.Day)
        {
            if (Server == null)
                return null;
            String url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Log.GRAPH) + idx;
            url += ConstantValues.Url.Log.GRAPH_RANGE + range.ToString().ToLower();
            url += ConstantValues.Url.Log.GRAPH_TYPE + type;

            try
            {
                response = await App.ConnectionService.client.GetAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                    return JsonConvert.DeserializeObject<GraphModel>(content);
                }
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }
            return null;
        }

        #endregion Data


        #region Actions

        /// <summary>
        /// Domoticz set action
        /// </summary>
        public async Task<ActionModel> SetAction(String idx, int jsonUrl, int jsonAction, double value, String password)
        {
            if (Server == null)
                return null;

            String url = await App.ConnectionService.ConstructSetUrlAsync(Server, jsonUrl, idx, jsonAction, value);
            url += !String.IsNullOrEmpty(password) ? "&passcode=" + password : "&passcode=";

            try
            {
                response = await App.ConnectionService.client.GetAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                    return JsonConvert.DeserializeObject<ActionModel>(content);
                }
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Domoticz get favorite devices
        /// </summary>
        public async Task<bool> SetFavorite(String idx, bool isScene, bool favorite)
        {
            if (Server == null)
                return false;

            try
            {
                var result = await SetAction(idx, isScene ? ConstantValues.Json.Url.Set.SCENEFAVORITE : ConstantValues.Json.Url.Set.FAVORITE,
                favorite ? ConstantValues.Device.Favorite.ON : ConstantValues.Device.Favorite.OFF,
                0, null);

                if (result != null &&
                    String.Compare(result.status, "ok", StringComparison.OrdinalIgnoreCase) == 0)
                    return true;
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }

            return false;
        }

        /// <summary>
        /// Domoticz set dimmer value level
        /// </summary>
        public async Task<bool> SetDimmer(String idx, float value, String password = null)
        {
            if (Server == null)
                return false;

            try
            {
                var result = await SetAction(idx, ConstantValues.Json.Url.Set.SWITCHES,
                ConstantValues.Device.Dimmer.Action.DIM_LEVEL,
                value, password);
                if (result != null &&
                    String.Compare(result.status, "ok", StringComparison.OrdinalIgnoreCase) == 0)
                    return true;
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }

            return false;
        }

        /// <summary>
        /// Domoticz set switch on off value
        /// </summary>
        public async Task<bool> SetSwitch(String idx, bool value, bool isScene, String password = null)
        {
            if (Server == null)
                return false;

            try
            {
                if (!isScene)
                {
                    var result = await SetAction(idx, ConstantValues.Json.Url.Set.SWITCHES,
                    value ? ConstantValues.Device.Switch.Action.ON : ConstantValues.Device.Switch.Action.OFF,
                    0, password);
                    if (result != null &&
                        String.Compare(result.status, "ok", StringComparison.OrdinalIgnoreCase) == 0)
                        return true;
                }
                else
                {
                    var result = await SetAction(idx, ConstantValues.Json.Url.Set.SCENES,
                   value ? ConstantValues.Device.Scene.Action.ON : ConstantValues.Device.Scene.Action.OFF,
                   0, password);
                    if (result != null &&
                        String.Compare(result.status, "ok", StringComparison.OrdinalIgnoreCase) == 0)
                        return true;
                }
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }

            return false;
        }

        /// <summary>
        /// Domoticz set point to new float value
        /// </summary>
        public async Task<bool> SetPoint(String idx, double value, double oldvalue, String password = null)
        {
            if (Server == null)
                return false;
            try
            {
                var result = await SetAction(idx, ConstantValues.Json.Url.Set.TEMP,
                value < oldvalue ? ConstantValues.Device.Thermostat.Action.MIN : ConstantValues.Device.Thermostat.Action.PLUS,
                value, password);

                if (result != null &&
                    String.Compare(result.status, "ok", StringComparison.OrdinalIgnoreCase) == 0)
                    return true;
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }

            return false;
        }

        /// <summary>
        /// Domoticz set switch on off value
        /// </summary>
        public async Task<bool> SetBlind(String idx, int value = ConstantValues.Device.Switch.Action.OFF, String password = null)
        {
            if (Server == null)
                return false;

            try
            {
                var result = await SetAction(idx, ConstantValues.Json.Url.Set.SWITCHES,
                value,
                0, password);
                if (result != null &&
                    String.Compare(result.status, "ok", StringComparison.OrdinalIgnoreCase) == 0)
                    return true;
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }

            return false;
        }


        /// <summary>
        /// Domoticz set state of security panel
        /// </summary>
        public async Task<bool> SetSecurityPanel(int secStatus, String secMD5code)
        {
            if (Server == null)
                return false;

            String url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.System.SETSECURITY);
            url += "&secstatus=" + secStatus;
            url += "&seccode=" + secMD5code;

            try
            {
                response = await App.ConnectionService.client.GetAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                    var result = JsonConvert.DeserializeObject<ActionModel>(content);
                    if (result != null &&
                    String.Compare(result.status, "ok", StringComparison.OrdinalIgnoreCase) == 0)
                        return true;
                }
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }

            return false;
        }

        /// <summary>
        /// Domoticz clean device id
        /// </summary>
        public async Task<bool> CleanRegisteredDevice(String DeviceID)
        {
            if (Server == null || String.IsNullOrEmpty(DeviceID))
                return false;

            String url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.System.CLEAN_MOBILE_DEVICE);
            url += "&uuid=" + DeviceID;
            try
            {
                response = await App.ConnectionService.client.GetAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                    return true;
                }
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }

            return true;
        }

        /// <summary>
        /// Domoticz register device for GCM
        /// </summary>
        public async Task<bool> RegisterDevice(String DeviceID, String SenderId)
        {
            if (Server == null || String.IsNullOrEmpty(DeviceID) || String.IsNullOrEmpty(SenderId))
                return false;

            String url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.System.ADD_MOBILE_DEVICE);
            url += "&uuid=" + DeviceID;
            url += "&senderid=" + SenderId;
            url += "&name=" + CrossDeviceInfo.Current.Model;
            url += "&devicetype=" + CrossDeviceInfo.Current.Platform.ToString() + " / " + CrossDeviceInfo.Current.Version;
            App.AddLog("Domoticz Full Url: " + url);

            try
            {
                response = await App.ConnectionService.client.GetAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                    return true;
                }
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }

            return true;
        }

        #endregion Actions
    }
}
