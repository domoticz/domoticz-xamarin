using Newtonsoft.Json;
using NL.HNOGames.Domoticz.Helpers;
using NL.HNOGames.Domoticz.Models;
using Plugin.DeviceInfo;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Device = NL.HNOGames.Domoticz.Models.Device;

namespace NL.HNOGames.Domoticz.Data
{
    /// <summary>
    /// Data service class handles all the data calls to Axis
    /// </summary>
    public class DataService
    {
        #region Variables

        /// <summary>
        /// Defines the Response
        /// </summary>
        public HttpResponseMessage Response;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Server
        /// Get server App.AppSettings
        /// </summary>
        public ServerSettings Server { get; set; }

        #endregion

        #region Public

        /// <summary>
        /// check https certificate
        /// </summary>
        /// <returns>The <see cref="Task{bool}"/></returns>
        public async Task<bool> CheckHttpCertificate()
        {
            if (Server == null)
                return false;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.VERSION);
            App.AddLog("JSON Call: " + url);
            try
            {
                Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message) && ex.Message.Contains("CertPathValidatorException"))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Domoticz version
        /// </summary>
        /// <returns>The <see cref="Task{SunRiseModel}"/></returns>
        public async Task<SunRiseModel> GetSunRise()
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.SUNRISE);
            App.AddLog("JSON Call: " + url);
            try
            {
                Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
                    if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                    return JsonConvert.DeserializeObject<SunRiseModel>(content);
                }
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Domoticz version
        /// </summary>
        /// <returns>The <see cref="Task{VersionModel}"/></returns>
        public async Task<VersionModel> GetVersion()
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.VERSION);
            App.AddLog("JSON Call: " + url);
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
                App.AddLog("JSON Call: " + url);
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
        /// <returns>The <see cref="Task{PlansModel}"/></returns>
        public async Task<PlansModel> GetPlans()
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.PLANS);
            App.AddLog("JSON Call: " + url);

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
        /// <param name="idx">The idx<see cref="string"/></param>
        /// <param name="isSceneOrGroup">The isSceneOrGroup<see cref="bool"/></param>
        /// <returns>The <see cref="Task{Device}"/></returns>
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
        /// <returns>The <see cref="Task{CameraModel}"/></returns>
        public async Task<CameraModel> GetCameras()
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.CAMERAS);
            App.AddLog("JSON Call: " + url);
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
        /// <param name="idx">The idx<see cref="string"/></param>
        /// <returns>The <see cref="Task{byte[]}"/></returns>
        public async Task<byte[]> GetCameraStream(string idx)
        {
            if (Server == null || string.IsNullOrEmpty(idx))
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.CAMERA) + idx;
            App.AddLog("JSON Call: " + url);

            try
            {
                using (var httpResponse = await App.ConnectionService.Client.GetAsync(url))
                {
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        var data = await httpResponse.Content.ReadAsByteArrayAsync();
                        return data;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                App.AddLog(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Domoticz get all devices
        /// </summary>
        /// <param name="plan">The plan<see cref="int"/></param>
        /// <param name="filter">The filter<see cref="string"/></param>
        /// <returns>The <see cref="Task{DevicesModel}"/></returns>
        public async Task<DevicesModel> GetDevices(int plan = 0, string filter = null)
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.DEVICES);
            App.AddLog("JSON Call: " + url);
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
        /// <param name="device">The device<see cref="Device"/></param>
        /// <returns>The <see cref="Task{NotificationModel}"/></returns>
        public async Task<NotificationModel> GetNotifications(Device device)
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Notification.NOTIFICATION) + device.idx;
            App.AddLog("JSON Call: " + url);

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
        /// <param name="idx">The idx<see cref="string"/></param>
        /// <returns>The <see cref="Task{TimerModel}"/></returns>
        public async Task<TimerModel> GetTimers(string idx)
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.SWITCHTIMER) + idx;
            App.AddLog("JSON Call: " + url);

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
        /// <returns>The <see cref="Task{ServerLogsModel}"/></returns>
        public async Task<ServerLogsModel> GetServerLogs()
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Log.GET_LOG);
            App.AddLog("JSON Call: " + url);

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
        /// <returns>The <see cref="Task{UserVariableModel}"/></returns>
        public async Task<UserVariableModel> GetUserVariables()
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.System.USERVARIABLES);
            App.AddLog("JSON Call: " + url);

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
        /// <returns>The <see cref="Task{EventModel}"/></returns>
        public async Task<EventModel> GetEvents()
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.System.EVENTS);
            App.AddLog("JSON Call: " + url);

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
        /// <param name="deviceIdx">The deviceIdx<see cref="string"/></param>
        /// <param name="isScene">The isScene<see cref="bool"/></param>
        /// <param name="textLog">The textLog<see cref="bool"/></param>
        /// <returns>The <see cref="Task{LogModel}"/></returns>
        public async Task<LogModel> GetLogs(string deviceIdx, bool isScene = false, bool textLog = false)
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.SWITCHLOG) + deviceIdx;
            if (isScene)
                url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.SCENELOG) + deviceIdx;
            else if (textLog)
                url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.TEXTLOG) + deviceIdx;
            App.AddLog("JSON Call: " + url);

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
        /// <param name="filter">The filter<see cref="string"/></param>
        /// <returns>The <see cref="Task{SceneModel}"/></returns>
        public async Task<SceneModel> GetScenes(string filter)
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.SCENES);
            if (!string.IsNullOrEmpty(filter))
                url = url.Replace("filter=all", "filter=" + filter);
            App.AddLog("JSON Call: " + url);

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
        /// <param name="filter">The filter<see cref="string"/></param>
        /// <returns>The <see cref="Task{DevicesModel}"/></returns>
        public async Task<DevicesModel> GetTemperature(string filter)
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.TEMPERATURE);
            if (!string.IsNullOrEmpty(filter))
                url = url.Replace("filter=all", "filter=" + filter);
            App.AddLog("JSON Call: " + url);

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
        /// <param name="filter">The filter<see cref="string"/></param>
        /// <returns>The <see cref="Task{DevicesModel}"/></returns>
        public async Task<DevicesModel> GetWeather(string filter)
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.WEATHER);
            if (!string.IsNullOrEmpty(filter))
                url = url.Replace("filter=all", "filter=" + filter);
            App.AddLog("JSON Call: " + url);

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
        /// <param name="filter">The filter<see cref="string"/></param>
        /// <returns>The <see cref="Task{DevicesModel}"/></returns>
        public async Task<DevicesModel> GetUtilities(string filter)
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.UTILITIES);
            if (!string.IsNullOrEmpty(filter))
                url = url.Replace("filter=all", "filter=" + filter);
            App.AddLog("JSON Call: " + url);

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
        /// <param name="filter">The filter<see cref="string"/></param>
        /// <returns>The <see cref="Task{DevicesModel}"/></returns>
        public async Task<DevicesModel> GetFavorites(string filter)
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Category.FAVORITES);
            if (!string.IsNullOrEmpty(filter))
                url = url.Replace("filter=all", "filter=" + filter);
            App.AddLog("JSON Call: " + url);

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
        /// <param name="idx">The idx<see cref="string"/></param>
        /// <param name="type">The type<see cref="string"/></param>
        /// <param name="range">The range<see cref="ConstantValues.GraphRange"/></param>
        /// <returns>The <see cref="Task{GraphModel}"/></returns>
        public async Task<GraphModel> GetGraphData(string idx, string type, ConstantValues.GraphRange range = ConstantValues.GraphRange.Day)
        {
            if (Server == null)
                return null;
            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.Log.GRAPH) + idx;
            url += ConstantValues.Url.Log.GRAPH_RANGE + range.ToString().ToLower();
            url += ConstantValues.Url.Log.GRAPH_TYPE + type;
            App.AddLog("JSON Call: " + url);

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

        /// <summary>
        /// Domoticz set action
        /// </summary>
        /// <param name="idx">The idx<see cref="string"/></param>
        /// <param name="jsonUrl">The jsonUrl<see cref="int"/></param>
        /// <param name="jsonAction">The jsonAction<see cref="int"/></param>
        /// <param name="value">The value<see cref="double"/></param>
        /// <param name="password">The password<see cref="string"/></param>
        /// <returns>The <see cref="Task{ActionModel}"/></returns>
        public async Task<ActionModel> SetAction(string idx, int jsonUrl, int jsonAction, double value, string password)
        {
            if (Server == null)
                return null;

            var url = await App.ConnectionService.ConstructSetUrlAsync(Server, jsonUrl, idx, jsonAction, value);
            url += !string.IsNullOrEmpty(password) ? "&passcode=" + password : "&passcode=";
            App.AddLog("Action Call: " + url);

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
        /// Domoticz set RGB action
        /// </summary>
        /// <param name="idx">The idx<see cref="string"/></param>
        /// <param name="jsonUrl">The jsonUrl<see cref="int"/></param>
        /// <param name="hex">The hex<see cref="string"/></param>
        /// <param name="brightness">The brightness<see cref="int"/></param>
        /// <param name="password">The password<see cref="string"/></param>
        /// <returns>The <see cref="Task{ActionModel}"/></returns>
        public async Task<ActionModel> SetRGBAction(string idx, int jsonUrl, string hex, int brightness, string password)
        {
            if (Server == null)
                return null;
            bool isWhite = (hex.ToLower() == "ffffff");

            var url = await App.ConnectionService.ConstructSetUrlAsync(Server, jsonUrl, idx, ConstantValues.Device.Dimmer.Action.HEXCOLOR, 0);
            url = url.Replace("%hex%", hex).Replace("%bright%", brightness.ToString());
            if (isWhite)
                url = url.Replace("&iswhite=false", "&iswhite=true");
            url += !string.IsNullOrEmpty(password) ? "&passcode=" + password : "&passcode=";
            App.AddLog("Action Call: " + url);

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
        /// <param name="idx">The idx<see cref="string"/></param>
        /// <param name="isScene">The isScene<see cref="bool"/></param>
        /// <param name="favorite">The favorite<see cref="bool"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
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
        /// <param name="idx">The idx<see cref="string"/></param>
        /// <param name="value">The value<see cref="float"/></param>
        /// <param name="password">The password<see cref="string"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
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
        /// Domoticz set color
        /// </summary>
        /// <param name="idx">The idx<see cref="string"/></param>
        /// <param name="value">The value<see cref="Color"/></param>
        /// <param name="password">The password<see cref="string"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
        public async Task<bool> SetColor(string idx, Color value, string password = null)
        {
            if (Server == null)
                return false;
            try
            {
                var result = await SetRGBAction(idx, ConstantValues.Json.Url.Set.RGBCOLOR, UsefulBits.GetHexString(value), (int)(value.A * 100), password);
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
        /// Domoticz set events on off value
        /// </summary>
        /// <param name="idx">The idx<see cref="string"/></param>
        /// <param name="value">The value<see cref="bool"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
        public async Task<bool> SetEvent(string idx, bool value)
        {
            if (Server == null)
                return false;
            try
            {
                var result = await SetAction(idx, ConstantValues.Json.Url.Set.EVENTS_UPDATE_STATUS,
                value ? ConstantValues.Event.Action.ON : ConstantValues.Event.Action.OFF,
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
        /// Domoticz get all temperature devices
        /// </summary>
        /// <param name="idx">The idx<see cref="string"/></param>
        /// <param name="name">The name<see cref="string"/></param>
        /// <param name="type">The type<see cref="string"/></param>
        /// <param name="newValue">The newValue<see cref="string"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
        public async Task<bool> SetUserVariable(string idx, string name, string type, string newValue)
        {
            if (Server == null || string.IsNullOrEmpty(idx) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(type))
                return false;

            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.UserVariable.UPDATE);
            url += "&idx=" + idx;
            url += "&vname=" + name;
            url += "&vtype=" + type;
            url += "&vvalue=" + newValue;
            App.AddLog("JSON Call: " + url);

            try
            {
                Response = await App.ConnectionService.Client.GetAsync(new Uri(url));
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
                    if (App.AppSettings.EnableJSONDebugging) App.AddLog("JSON: " + content.Replace(Environment.NewLine, ""));
                    var resultContent = JsonConvert.DeserializeObject<ActionModel>(content);
                    if (resultContent != null &&
                        string.Compare(resultContent.status, "ok", StringComparison.OrdinalIgnoreCase) == 0)
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
        /// Domoticz set switch on off value
        /// </summary>
        /// <param name="idx">The idx<see cref="string"/></param>
        /// <param name="value">The value<see cref="bool"/></param>
        /// <param name="isScene">The isScene<see cref="bool"/></param>
        /// <param name="password">The password<see cref="string"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
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
        /// <param name="idx">The idx<see cref="string"/></param>
        /// <param name="value">The value<see cref="double"/></param>
        /// <param name="oldvalue">The oldvalue<see cref="double"/></param>
        /// <param name="password">The password<see cref="string"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
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
        /// <param name="idx">The idx<see cref="string"/></param>
        /// <param name="value">The value<see cref="int"/></param>
        /// <param name="password">The password<see cref="string"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
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
        /// <param name="secStatus">The secStatus<see cref="int"/></param>
        /// <param name="secMd5Code">The secMd5Code<see cref="string"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
        public async Task<bool> SetSecurityPanel(int secStatus, string secMd5Code)
        {
            if (Server == null)
                return false;

            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.System.SETSECURITY);
            url += "&secstatus=" + secStatus;
            url += "&seccode=" + secMd5Code;
            App.AddLog("JSON Call: " + url);

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
        /// <param name="deviceId">The deviceId<see cref="string"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
        public async Task<bool> CleanRegisteredDevice(string deviceId)
        {
            if (Server == null || string.IsNullOrEmpty(deviceId))
                return false;

            var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.System.CLEAN_MOBILE_DEVICE);
            url += "&uuid=" + deviceId;
            App.AddLog("JSON Call: " + url);
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
        /// <param name="deviceId">The deviceId<see cref="string"/></param>
        /// <param name="senderId">The senderId<see cref="string"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
        public async Task<bool> RegisterDevice(string deviceId, string senderId)
        {
            try
            {
                if (Server == null || string.IsNullOrEmpty(deviceId) || string.IsNullOrEmpty(senderId))
                    return false;

                var url = await App.ConnectionService.ConstructGetUrlAsync(Server, ConstantValues.Url.System.ADD_MOBILE_DEVICE);
                url += "&uuid=" + deviceId;
                url += "&senderid=" + senderId;
                url += "&name=" + CrossDeviceInfo.Current.Model.Replace(" ", "");
                url += "&devicetype=" + (CrossDeviceInfo.Current.Platform.ToString() + CrossDeviceInfo.Current.Version).Replace(" ", "");
                url += "&active=" + (App.AppSettings.EnableNotifications ? "1" : "0");
                App.AddLog("JSON Call: " + url);

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
        /// <param name="idx">The idx<see cref="string"/></param>
        /// <param name="password">The password<see cref="string"/></param>
        /// <param name="inputJsonAction">The inputJsonAction<see cref="int"/></param>
        /// <param name="value">The value<see cref="string"/></param>
        /// <param name="isSceneOrGroup">The isSceneOrGroup<see cref="bool"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
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

        #endregion
    }
}
