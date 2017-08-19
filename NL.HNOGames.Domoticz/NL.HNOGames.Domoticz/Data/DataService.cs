using Newtonsoft.Json;
using NL.HNOGames.Domoticz.Models;
using Plugin.DeviceInfo;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Net;

namespace NL.HNOGames.Domoticz.Data
{
    /// <summary>
    /// Data service class handles all the data calls to Axis
    /// </summary>
    public class DataService
    {
        public HttpResponseMessage Response;

        /// <summary>
        /// Get server App.AppSettings
        /// </summary>
        public ServerSettings Server { get; set; }

        #region Data

        /// <summary>
        /// Domoticz version
        /// </summary>
        public async Task<VersionModel> GetVersion()
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.VERSION);
            try
            {
                Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
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

                var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.System.CONFIG);
                try
                {
                    Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                    if (Response.IsSuccessStatusCode)
                    {
                        var content = await Response.Content.ReadAsStringAsync();
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
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.PLANS);

            try
            {
                Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
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
        /// Domoticz get a specific device object
        /// </summary>
        public async Task<Device> GetDevice(string idx, bool isSceneOrGroup)
        {
            if (Server == null)
                return null;
            var devices = await GetDevices();

            if (devices == null || devices.result == null)
                return null;
            else
                return devices.result.FirstOrDefault(o => string.Compare(o.idx, idx, StringComparison.OrdinalIgnoreCase) == 0 && o.IsScene == isSceneOrGroup);
        }

        /// <summary>
        /// Domoticz get all cameras
        /// </summary>
        public async Task<CameraModel> GetCameras()
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.CAMERAS);
            try
            {
                Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
                    if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                    return JsonConvert.DeserializeObject<CameraModel>(content);
                }
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }
            return null;
        }


        /// <summary>
        /// Domoticz camera image stream
        /// </summary>
        public async Task<Stream> GetCameraStream(string idx)
        {
            if (Server == null || string.IsNullOrEmpty(idx))
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.CAMERA)+ idx;

            try
            {
                using (var httpResponse = await App.ConnectionService.Client.GetAsync(url))
                {
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        var data = await httpResponse.Content.ReadAsByteArrayAsync();
                        Stream stream = new MemoryStream(data);
                        return stream;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }


        /// <summary>
        /// Domoticz get all devices
        /// </summary>
        public async Task<DevicesModel> GetDevices(int plan = 0, string filter = null)
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.DEVICES);
            if (!string.IsNullOrEmpty(filter))
                url = url.Replace("filter=all", "filter=" + filter);
            if (plan > 0)
                url += "&plan=" + plan;

            try
            {
                Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
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
        public async Task<NotificationModel> GetNotifications(Device device)
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Notification.NOTIFICATION) + device.idx;

            try
            {
                Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
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
        public async Task<TimerModel> GetTimers(string idx)
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.SWITCHTIMER) + idx;

            try
            {
                Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
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
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Log.GET_LOG);

            try
            {
                Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
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
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.System.USERVARIABLES);

            try
            {
                Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
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
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.System.EVENTS);

            try
            {
                Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
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
        public async Task<LogModel> GetLogs(string deviceIdx, bool isScene = false, bool textLog = false)
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.SWITCHLOG) + deviceIdx;
            if (isScene)
                url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.SCENELOG) + deviceIdx;
            else if (textLog)
                url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.TEXTLOG) + deviceIdx;

            try
            {
                Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
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
        public async Task<SceneModel> GetScenes(string filter)
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.SCENES);
            if (!string.IsNullOrEmpty(filter))
                url = url.Replace("filter=all", "filter=" + filter);

            try
            {
                Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
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
        public async Task<DevicesModel> GetTemperature(string filter)
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.TEMPERATURE);
            if (!string.IsNullOrEmpty(filter))
                url = url.Replace("filter=all", "filter=" + filter);

            try
            {
                Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
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
        public async Task<DevicesModel> GetWeather(string filter)
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.WEATHER);
            if (!string.IsNullOrEmpty(filter))
                url = url.Replace("filter=all", "filter=" + filter);

            try
            {
                Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
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
        public async Task<DevicesModel> GetUtilities(string filter)
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.UTILITIES);
            if (!string.IsNullOrEmpty(filter))
                url = url.Replace("filter=all", "filter=" + filter);

            try
            {
                Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
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
        public async Task<DevicesModel> GetFavorites(string filter)
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.FAVORITES);
            if (!string.IsNullOrEmpty(filter))
                url = url.Replace("filter=all", "filter=" + filter);

            try
            {
                Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
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
        public async Task<GraphModel> GetGraphData(string idx, string type, ConstantValues.GraphRange range = ConstantValues.GraphRange.Day)
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Log.GRAPH) + idx;
            url += ConstantValues.Url.Log.GRAPH_RANGE + range.ToString().ToLower();
            url += ConstantValues.Url.Log.GRAPH_TYPE + type;

            try
            {
                Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
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
        public async Task<ActionModel> SetAction(string idx, int jsonUrl, int jsonAction, double value, string password)
        {
            if (Server == null)
                return null;

            var url = await App.ConnectionService.ConstructSetUrlAsync(Server, jsonUrl, idx, jsonAction, value);
            url += !string.IsNullOrEmpty(password) ? "&passcode=" + password : "&passcode=";

            try
            {
                Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
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
        public async Task<bool> SetFavorite(string idx, bool isScene, bool favorite)
        {
            if (Server == null)
                return false;

            try
            {
                var result = await SetAction(idx, isScene ? ConstantValues.Json.Url.Set.SCENEFAVORITE : ConstantValues.Json.Url.Set.FAVORITE,
                favorite ? ConstantValues.Device.Favorite.ON : ConstantValues.Device.Favorite.OFF,
                0, null);

                if (result != null &&
                    string.Compare(result.status, "ok", StringComparison.OrdinalIgnoreCase) == 0)
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
        public async Task<bool> SetDimmer(string idx, float value, string password = null)
        {
            if (Server == null)
                return false;

            try
            {
                var result = await SetAction(idx, ConstantValues.Json.Url.Set.SWITCHES,
                ConstantValues.Device.Dimmer.Action.DIM_LEVEL,
                value, password);
                if (result != null &&
                    string.Compare(result.status, "ok", StringComparison.OrdinalIgnoreCase) == 0)
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
        public async Task<bool> SetSwitch(string idx, bool value, bool isScene, string password = null)
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
                        string.Compare(result.status, "ok", StringComparison.OrdinalIgnoreCase) == 0)
                        return true;
                }
                else
                {
                    var result = await SetAction(idx, ConstantValues.Json.Url.Set.SCENES,
                   value ? ConstantValues.Device.Scene.Action.ON : ConstantValues.Device.Scene.Action.OFF,
                   0, password);
                    if (result != null &&
                        string.Compare(result.status, "ok", StringComparison.OrdinalIgnoreCase) == 0)
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
        public async Task<bool> SetPoint(string idx, double value, double oldvalue, string password = null)
        {
            if (Server == null)
                return false;
            try
            {
                var result = await SetAction(idx, ConstantValues.Json.Url.Set.TEMP,
                value < oldvalue ? ConstantValues.Device.Thermostat.Action.MIN : ConstantValues.Device.Thermostat.Action.PLUS,
                value, password);

                if (result != null &&
                    string.Compare(result.status, "ok", StringComparison.OrdinalIgnoreCase) == 0)
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
        public async Task<bool> SetBlind(string idx, int value = ConstantValues.Device.Switch.Action.OFF, string password = null)
        {
            if (Server == null)
                return false;

            try
            {
                var result = await SetAction(idx, ConstantValues.Json.Url.Set.SWITCHES,
                value,
                0, password);
                if (result != null &&
                    string.Compare(result.status, "ok", StringComparison.OrdinalIgnoreCase) == 0)
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
        public async Task<bool> SetSecurityPanel(int secStatus, string secMd5Code)
        {
            if (Server == null)
                return false;

            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.System.SETSECURITY);
            url += "&secstatus=" + secStatus;
            url += "&seccode=" + secMd5Code;

            try
            {
                Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
                    if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                    var result = JsonConvert.DeserializeObject<ActionModel>(content);
                    if (result != null &&
                    string.Compare(result.status, "ok", StringComparison.OrdinalIgnoreCase) == 0)
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
        public async Task<bool> CleanRegisteredDevice(string deviceId)
        {
            if (Server == null || string.IsNullOrEmpty(deviceId))
                return false;

            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.System.CLEAN_MOBILE_DEVICE);
            url += "&uuid=" + deviceId;
            try
            {
                Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
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
        public async Task<bool> RegisterDevice(string deviceId, string senderId)
        {
            if (Server == null || string.IsNullOrEmpty(deviceId) || string.IsNullOrEmpty(senderId))
                return false;

            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.System.ADD_MOBILE_DEVICE);
            url += "&uuid=" + deviceId;
            url += "&senderid=" + senderId;
            url += "&name=" + CrossDeviceInfo.Current.Model.Replace(" ", "");
            url += "&devicetype=" + (CrossDeviceInfo.Current.Platform.ToString() + CrossDeviceInfo.Current.Version).Replace(" ", "");
            url += "&active=" + (App.AppSettings.EnableNotifications ? "1" : "0");
            App.AddLog("Domoticz Full Url: " + url);

            try
            {
                Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
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
        /// Handle a toggle of a switch from a service like qrcode/speech/geo
        /// </summary>
        public async Task<bool> HandleSwitch(string idx, string password, int inputJsonAction, string value, bool isSceneOrGroup = false)
        {
            if (string.IsNullOrEmpty(idx))
                return false;

            var mDevicesInfo = await GetDevice(idx, isSceneOrGroup);
            if (mDevicesInfo == null)
                return false;

            int jsonAction;
            var jsonUrl = ConstantValues.Json.Url.Set.SWITCHES;
            var jsonValue = 0;

            if (!isSceneOrGroup)
            {
                if (inputJsonAction < 0)
                {
                    if (mDevicesInfo.SwitchTypeVal == ConstantValues.Device.Type.Value.BLINDS ||
                            mDevicesInfo.SwitchTypeVal == ConstantValues.Device.Type.Value.BLINDPERCENTAGE)
                    {
                        if (!mDevicesInfo.StatusBoolean)
                            jsonAction = ConstantValues.Device.Switch.Action.OFF;
                        else
                        {
                            jsonAction = ConstantValues.Device.Switch.Action.ON;
                            if (!string.IsNullOrEmpty(value))
                            {
                                jsonAction = ConstantValues.Device.Dimmer.Action.DIM_LEVEL;
                                jsonValue = ConstantValues.GetSelectorValue(mDevicesInfo, value);
                            }
                        }
                    }
                    else
                    {
                        if (!mDevicesInfo.StatusBoolean)
                        {
                            jsonAction = ConstantValues.Device.Switch.Action.ON;
                            if (!string.IsNullOrEmpty(value))
                            {
                                jsonAction = ConstantValues.Device.Dimmer.Action.DIM_LEVEL;
                                jsonValue = ConstantValues.GetSelectorValue(mDevicesInfo, value);
                            }
                        }
                        else
                            jsonAction = ConstantValues.Device.Switch.Action.OFF;
                    }
                }
                else
                {
                    if (mDevicesInfo.SwitchTypeVal == ConstantValues.Device.Type.Value.BLINDS ||
                            mDevicesInfo.SwitchTypeVal == ConstantValues.Device.Type.Value.BLINDPERCENTAGE)
                    {
                        if (inputJsonAction == 1)
                            jsonAction = ConstantValues.Device.Switch.Action.OFF;
                        else
                        {
                            jsonAction = ConstantValues.Device.Switch.Action.ON;
                            if (!string.IsNullOrEmpty(value))
                            {
                                jsonAction = ConstantValues.Device.Dimmer.Action.DIM_LEVEL;
                                jsonValue = ConstantValues.GetSelectorValue(mDevicesInfo, value);
                            }
                        }
                    }
                    else
                    {
                        if (inputJsonAction == 1)
                        {
                            jsonAction = ConstantValues.Device.Switch.Action.ON;
                            if (!string.IsNullOrEmpty(value))
                            {
                                jsonAction = ConstantValues.Device.Dimmer.Action.DIM_LEVEL;
                                jsonValue = ConstantValues.GetSelectorValue(mDevicesInfo, value);
                            }
                        }
                        else
                            jsonAction = ConstantValues.Device.Switch.Action.OFF;
                    }
                }

                switch (mDevicesInfo.SwitchTypeVal)
                {
                    case ConstantValues.Device.Type.Value.PUSH_ON_BUTTON:
                        jsonAction = ConstantValues.Device.Switch.Action.ON;
                        break;
                    case ConstantValues.Device.Type.Value.PUSH_OFF_BUTTON:
                        jsonAction = ConstantValues.Device.Switch.Action.OFF;
                        break;
                }
            }
            else
            {
                jsonUrl = ConstantValues.Json.Url.Set.SCENES;
                if (inputJsonAction < 0)
                {
                    if (!mDevicesInfo.StatusBoolean)
                    {
                        jsonAction = ConstantValues.Device.Scene.Action.ON;
                    }
                    else
                        jsonAction = ConstantValues.Device.Scene.Action.OFF;
                }
                else
                {
                    if (inputJsonAction == 1)
                    {
                        jsonAction = ConstantValues.Device.Scene.Action.ON;
                    }
                    else
                        jsonAction = ConstantValues.Device.Scene.Action.OFF;
                }

                if (string.Compare(mDevicesInfo.Type, ConstantValues.Device.Scene.Type.SCENE, StringComparison.OrdinalIgnoreCase) == 0)
                    jsonAction = ConstantValues.Device.Scene.Action.ON;
            }

            var result = await SetAction(idx, jsonUrl, jsonAction, jsonValue, password);
            if (result != null &&
                string.Compare(result.status, "ok", StringComparison.OrdinalIgnoreCase) == 0)
                return true;

            return false;
        }

        #endregion Actions
    }
}
