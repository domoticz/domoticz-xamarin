﻿using ModernHttpClient;
using NL.HNOGames.Domoticz.Helpers;
using NL.HNOGames.Domoticz.Models;
using Plugin.Connectivity;
using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.Data
{
    /// <summary>
    /// Data service class handles all the data calls to Axis
    /// </summary>
    public class ConnectionService : IDisposable
    {
        #region Variables

        /// <summary>
        /// Defines the Client
        /// </summary>
        public HttpClient Client;

        /// <summary>
        /// Defines the _latestUsedbaseUrl
        /// </summary>
        private string _latestUsedbaseUrl = string.Empty;

        /// <summary>
        /// Defines the _cookieHandler
        /// </summary>
        private readonly NativeCookieHandler _cookieHandler;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionService"/> class.
        /// </summary>
        public ConnectionService()
        {
            _cookieHandler = new NativeCookieHandler();
            switch (Xamarin.Forms.Device.RuntimePlatform)
            {
                case Xamarin.Forms.Device.Android:
                    Client = new HttpClient(DependencyService.Get<IHTTPClientHandlerCreationService>().GetInsecureHandler())
                    {
                        MaxResponseContentBufferSize = 25600000,
                        Timeout = TimeSpan.FromMilliseconds(10000),
                    };
                    break;
                default:
                    Client = new HttpClient(new NativeMessageHandler(false, true, _cookieHandler))
                    {
                        MaxResponseContentBufferSize = 25600000,
                        Timeout = TimeSpan.FromMilliseconds(10000),
                    };
                    break;
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// Construct Domoticz API Url
        /// </summary>
        /// <param name="server">The server<see cref="ServerSettings"/></param>
        /// <param name="jsonUrl">The jsonUrl<see cref="string"/></param>
        /// <returns>The <see cref="Task{string}"/></returns>
        public async Task<string> ConstructGetUrlAsync(ServerSettings server, string jsonUrl)
        {
            if (server == null)
                return null;
            string protocol, url, port, directory;
            if (await IsUserOnLocalWifiAsync(server))
            {
                protocol = server.LOCAL_SERVER_PROTOCOL == 0
                    ? ConstantValues.Url.Protocol.HTTP
                    : ConstantValues.Url.Protocol.HTTPS;
                url = server.LOCAL_SERVER_URL;
                port = server.LOCAL_SERVER_PORT;
                directory = server.LOCAL_SERVER_DIRECTORY;

                if (!string.IsNullOrEmpty(server.LOCAL_SERVER_USERNAME))
                {
                    var byteArray = Encoding.UTF8.GetBytes(
                        server.LOCAL_SERVER_USERNAME +
                        ":" +
                        server.LOCAL_SERVER_PASSWORD);
                    Client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
                            Convert.ToBase64String(byteArray));
                }
                else if (!string.IsNullOrEmpty(server.REMOTE_SERVER_USERNAME))
                {
                    var byteArray = Encoding.UTF8.GetBytes(
                        server.REMOTE_SERVER_USERNAME +
                        ":" +
                        server.REMOTE_SERVER_PASSWORD);
                    Client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
                            Convert.ToBase64String(byteArray));
                }
            }
            else
            {
                protocol = server.REMOTE_SERVER_PROTOCOL == 0
                    ? ConstantValues.Url.Protocol.HTTP
                    : ConstantValues.Url.Protocol.HTTPS;
                url = server.REMOTE_SERVER_URL;
                port = server.REMOTE_SERVER_PORT;
                directory = server.REMOTE_SERVER_DIRECTORY;

                if (!string.IsNullOrEmpty(server.REMOTE_SERVER_USERNAME))
                {
                    var byteArray =
                        Encoding.UTF8.GetBytes(server.REMOTE_SERVER_USERNAME + ":" + server.REMOTE_SERVER_PASSWORD);
                    Client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
                            Convert.ToBase64String(byteArray));
                }
            }

            _latestUsedbaseUrl = $"{protocol}{url}:{port}{(string.IsNullOrEmpty(directory) ? "" : "/" + directory)}";
            var fullString = $"{_latestUsedbaseUrl}{jsonUrl}";
            return fullString;
        }

        /// <summary>
        /// Create the Url for settings (Post) values
        /// </summary>
        /// <param name="server">The server<see cref="ServerSettings"/></param>
        /// <param name="jsonSetUrl">The jsonSetUrl<see cref="int"/></param>
        /// <param name="idx">The idx<see cref="string"/></param>
        /// <param name="action">The action<see cref="int"/></param>
        /// <param name="value">The value<see cref="double"/></param>
        /// <returns>The <see cref="Task{string}"/></returns>
        public async Task<string> ConstructSetUrlAsync(ServerSettings server, int jsonSetUrl, string idx, int action,
            double value)
        {
            if (server == null)
                return null;
            string protocol, baseUrl, port, directory, jsonUrl = null, actionUrl;
            if (await IsUserOnLocalWifiAsync(server))
            {
                protocol = server.LOCAL_SERVER_PROTOCOL == 0
                    ? ConstantValues.Url.Protocol.HTTP
                    : ConstantValues.Url.Protocol.HTTPS;
                baseUrl = server.LOCAL_SERVER_URL;
                port = server.LOCAL_SERVER_PORT;
                directory = server.LOCAL_SERVER_DIRECTORY;

                if (!string.IsNullOrEmpty(server.LOCAL_SERVER_USERNAME))
                {
                    var byteArray = Encoding.UTF8.GetBytes(
                        server.LOCAL_SERVER_USERNAME +
                        ":" +
                        server.LOCAL_SERVER_PASSWORD);
                    Client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
                            Convert.ToBase64String(byteArray));
                }
                else if (!string.IsNullOrEmpty(server.REMOTE_SERVER_USERNAME))
                {
                    var byteArray = Encoding.UTF8.GetBytes(
                        server.REMOTE_SERVER_USERNAME +
                        ":" +
                        server.REMOTE_SERVER_PASSWORD);
                    Client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
                            Convert.ToBase64String(byteArray));
                }
            }
            else
            {
                protocol = server.REMOTE_SERVER_PROTOCOL == 0
                    ? ConstantValues.Url.Protocol.HTTP
                    : ConstantValues.Url.Protocol.HTTPS;
                baseUrl = server.REMOTE_SERVER_URL;
                port = server.REMOTE_SERVER_PORT;
                directory = server.REMOTE_SERVER_DIRECTORY;
                if (!string.IsNullOrEmpty(server.REMOTE_SERVER_USERNAME))
                {
                    var byteArray =
                        Encoding.UTF8.GetBytes(server.REMOTE_SERVER_USERNAME + ":" + server.REMOTE_SERVER_PASSWORD);
                    Client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
                            Convert.ToBase64String(byteArray));
                }
            }

            switch (action)
            {
                case ConstantValues.Device.Scene.Action.ON:
                    actionUrl = ConstantValues.Url.Action.ON;
                    break;

                case ConstantValues.Device.Scene.Action.OFF:
                    actionUrl = ConstantValues.Url.Action.OFF;
                    break;

                case ConstantValues.Device.Switch.Action.ON:
                    actionUrl = ConstantValues.Url.Action.ON;
                    break;

                case ConstantValues.Device.Switch.Action.OFF:
                    actionUrl = ConstantValues.Url.Action.OFF;
                    break;

                case ConstantValues.Device.Blind.Action.UP:
                    actionUrl = ConstantValues.Url.Action.UP;
                    break;

                case ConstantValues.Device.Blind.Action.STOP:
                    actionUrl = ConstantValues.Url.Action.STOP;
                    break;

                case ConstantValues.Device.Blind.Action.DOWN:
                    actionUrl = ConstantValues.Url.Action.DOWN;
                    break;

                case ConstantValues.Device.Blind.Action.ON:
                    actionUrl = ConstantValues.Url.Action.ON;
                    break;

                case ConstantValues.Device.Blind.Action.OFF:
                    actionUrl = ConstantValues.Url.Action.OFF;
                    break;

                case ConstantValues.Device.Thermostat.Action.MIN:
                    actionUrl = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case ConstantValues.Device.Thermostat.Action.PLUS:
                    actionUrl = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case ConstantValues.Device.Favorite.ON:
                    actionUrl = ConstantValues.Favorite.Action.ON;
                    break;

                case ConstantValues.Device.Favorite.OFF:
                    actionUrl = ConstantValues.Favorite.Action.OFF;
                    break;

                case ConstantValues.Device.Dimmer.Action.DIM_LEVEL:
                    actionUrl = ConstantValues.Url.Switch.DIM_LEVEL + value.ToString();
                    break;

                case ConstantValues.Device.Dimmer.Action.COLOR:
                    actionUrl = ConstantValues.Url.Switch.COLOR;
                    break;

                case ConstantValues.Device.Dimmer.Action.HEXCOLOR:
                    actionUrl = ConstantValues.Url.Switch.HEXCOLOR;
                    break;

                case ConstantValues.Device.ModalSwitch.Action.AUTO:
                    actionUrl = ConstantValues.Url.ModalAction.AUTO;
                    break;

                case ConstantValues.Device.ModalSwitch.Action.ECONOMY:
                    actionUrl = ConstantValues.Url.ModalAction.ECONOMY;
                    break;

                case ConstantValues.Device.ModalSwitch.Action.AWAY:
                    actionUrl = ConstantValues.Url.ModalAction.AWAY;
                    break;

                case ConstantValues.Device.ModalSwitch.Action.DAY_OFF:
                    actionUrl = ConstantValues.Url.ModalAction.DAY_OFF;
                    break;

                case ConstantValues.Device.ModalSwitch.Action.CUSTOM:
                    actionUrl = ConstantValues.Url.ModalAction.CUSTOM;
                    break;

                case ConstantValues.Device.ModalSwitch.Action.HEATING_OFF:
                    actionUrl = ConstantValues.Url.ModalAction.HEATING_OFF;
                    break;

                case ConstantValues.Event.Action.ON:
                    actionUrl = ConstantValues.Url.Event.ON;
                    break;

                case ConstantValues.Event.Action.OFF:
                    actionUrl = ConstantValues.Url.Event.OFF;
                    break;

                default:
                    throw new Exception(
                        "Action not found in method Domoticz.ConstructSetUrl");
            }

            string url;
            switch (jsonSetUrl)
            {
                case ConstantValues.Json.Url.Set.SCENES:
                    url = ConstantValues.Url.Scene.GET;
                    jsonUrl = url
                              + idx
                              + ConstantValues.Url.Switch.CMD + actionUrl;
                    break;

                case ConstantValues.Json.Url.Set.SWITCHES:
                    url = ConstantValues.Url.Switch.GET;
                    jsonUrl = url
                              + idx
                              + ConstantValues.Url.Switch.CMD + actionUrl;
                    break;

                case ConstantValues.Json.Url.Set.MODAL_SWITCHES:
                    url = ConstantValues.Url.ModalSwitch.GET;
                    jsonUrl = url
                              + idx
                              + ConstantValues.Url.ModalSwitch.STATUS + actionUrl;
                    break;
                case ConstantValues.Json.Url.Set.TEMP:
                    url = ConstantValues.Url.Temp.GET;
                    jsonUrl = url
                              + idx
                              + ConstantValues.Url.Temp.VALUE + actionUrl;
                    break;

                case ConstantValues.Json.Url.Set.SCENEFAVORITE:
                    url = ConstantValues.Url.Favorite.SCENE;
                    jsonUrl = url
                              + idx
                              + ConstantValues.Url.Favorite.VALUE + actionUrl;
                    break;

                case ConstantValues.Json.Url.Set.FAVORITE:
                    url = ConstantValues.Url.Favorite.GET;
                    jsonUrl = url
                              + idx
                              + ConstantValues.Url.Favorite.VALUE + actionUrl;
                    break;

                case ConstantValues.Json.Url.Set.RGBCOLOR:
                    url = ConstantValues.Url.System.RGBCOLOR;
                    jsonUrl = url
                              + idx
                              + actionUrl;
                    break;

                case ConstantValues.Json.Url.Set.EVENTS_UPDATE_STATUS:
                    url = ConstantValues.Url.System.EVENTS_UPDATE_STATUS;
                    jsonUrl = url
                              + idx
                              + actionUrl;
                    break;
            }
            _latestUsedbaseUrl =
                $"{protocol}{baseUrl}:{port}{(string.IsNullOrEmpty(directory) ? "" : "/" + directory)}";
            var fullString = $"{_latestUsedbaseUrl}{jsonUrl}";
            return fullString;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Client.Dispose();
        }

        #endregion

        #region Private

        /// <summary>
        /// Is User On Local Wifi Async
        /// </summary>
        /// <param name="server">The server<see cref="ServerSettings"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
        private static async Task<bool> IsUserOnLocalWifiAsync(ServerSettings server)
        {
            if (server == null || !server.IS_LOCAL_SERVER_ADDRESS_DIFFERENT || string.IsNullOrEmpty(server.LOCAL_SERVER_URL))
                return false;

            var protocol = server.LOCAL_SERVER_PROTOCOL == 0 ? ConstantValues.Url.Protocol.HTTP : ConstantValues.Url.Protocol.HTTPS;
            var localUri = $"{protocol}{server.LOCAL_SERVER_URL}:{server.LOCAL_SERVER_PORT}";
            if (!string.IsNullOrEmpty(server.LOCAL_SERVER_DIRECTORY))
                localUri += $"/{server.LOCAL_SERVER_DIRECTORY}";

            return await CrossConnectivity.Current.IsRemoteReachable(new Uri(localUri), TimeSpan.FromSeconds(5));
        }

        #endregion
    }
}
