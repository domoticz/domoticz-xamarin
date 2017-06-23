using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NL.HNOGames.Domoticz.Models;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace NL.HNOGames.Domoticz.Data
{
    /// <summary>
    /// Data service class handles all the data calls to Axis
    /// </summary>
    public class ConnectionService
    {
        public HttpClient client;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConnectionService()
        { RefreshClient(); }

        public void RefreshClient()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMilliseconds(5000);
        }

        /// <summary>
        /// Construct Domoticz API Url
        /// </summary>
        public async Task<string> ConstructGetUrlAsync(ServerSettings server, String jsonUrl)
        {
            if (server == null)
                return null;
            String protocol, url, port, directory;
            if (await IsUserOnLocalWifiAsync(server))
            {
                if(server.LOCAL_SERVER_PROTOCOL == 0)
                    protocol = ConstantValues.Url.Protocol.HTTP;
                else protocol = ConstantValues.Url.Protocol.HTTPS;

                url = server.LOCAL_SERVER_URL;
                port = server.LOCAL_SERVER_PORT;
                directory = server.LOCAL_SERVER_DIRECTORY;

                if (!String.IsNullOrEmpty(server.LOCAL_SERVER_USERNAME))
                {
                    RefreshClient();
                    var byteArray = Encoding.UTF8.GetBytes(
                        server.LOCAL_SERVER_USERNAME +
                        ":" +
                        server.LOCAL_SERVER_PASSWORD);
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                }
                else
                {
                    if (!String.IsNullOrEmpty(server.REMOTE_SERVER_USERNAME))
                    {
                        RefreshClient();
                        var byteArray = Encoding.UTF8.GetBytes(
                            server.REMOTE_SERVER_USERNAME +
                            ":" +
                            server.REMOTE_SERVER_PASSWORD);
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    }
                }
            }
            else
            {
                if (server.REMOTE_SERVER_PROTOCOL == 0)
                    protocol = ConstantValues.Url.Protocol.HTTP;
                else protocol = ConstantValues.Url.Protocol.HTTPS;

                url = server.REMOTE_SERVER_URL;
                port = server.REMOTE_SERVER_PORT;
                directory = server.REMOTE_SERVER_DIRECTORY;

                if (!String.IsNullOrEmpty(server.REMOTE_SERVER_USERNAME))
                {
                    RefreshClient();
                    var byteArray = Encoding.UTF8.GetBytes(server.REMOTE_SERVER_USERNAME + ":" + server.REMOTE_SERVER_PASSWORD);
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                }
            }

            String fullString = String.Format("{0}{1}:{2}{3}{4}", protocol, url, port, 
                String.IsNullOrEmpty(directory) ? "" : "/" + directory, 
                jsonUrl);

            System.Diagnostics.Debug.WriteLine("Domoticz Url: " + fullString);
            return fullString;
        }


        /// <summary>
        /// Create the Url for settings (Post) values
        /// </summary>
        public async Task<string> ConstructSetUrlAsync(ServerSettings server, int jsonSetUrl, String idx, int action, double value)
        {
            if (server == null)
                return null;
            String protocol, baseUrl, port, directory, jsonUrl = null, actionUrl;
            if (await IsUserOnLocalWifiAsync(server))
            {
                if (server.LOCAL_SERVER_PROTOCOL == 0)
                    protocol = ConstantValues.Url.Protocol.HTTP;
                else protocol = ConstantValues.Url.Protocol.HTTPS;

                baseUrl = server.LOCAL_SERVER_URL;
                port = server.LOCAL_SERVER_PORT;
                directory = server.LOCAL_SERVER_DIRECTORY;

                if (!String.IsNullOrEmpty(server.LOCAL_SERVER_USERNAME))
                {
                    RefreshClient();
                    var byteArray = Encoding.UTF8.GetBytes(
                        server.LOCAL_SERVER_USERNAME +
                        ":" +
                        server.LOCAL_SERVER_PASSWORD);
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                }
                else
                {
                    if (!String.IsNullOrEmpty(server.REMOTE_SERVER_USERNAME))
                    {
                        RefreshClient();
                        var byteArray = Encoding.UTF8.GetBytes(
                            server.REMOTE_SERVER_USERNAME +
                            ":" +
                            server.REMOTE_SERVER_PASSWORD);
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    }
                }
            }
            else
            {
                if (server.REMOTE_SERVER_PROTOCOL == 0)
                    protocol = ConstantValues.Url.Protocol.HTTP;
                else protocol = ConstantValues.Url.Protocol.HTTPS;

                baseUrl = server.REMOTE_SERVER_URL;
                port = server.REMOTE_SERVER_PORT;
                directory = server.REMOTE_SERVER_DIRECTORY;

                if (!String.IsNullOrEmpty(server.REMOTE_SERVER_USERNAME))
                {
                    RefreshClient();
                    var byteArray = Encoding.UTF8.GetBytes(server.REMOTE_SERVER_USERNAME + ":" + server.REMOTE_SERVER_PASSWORD);
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
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

            String url = "";
            switch (jsonSetUrl)
            {
                case ConstantValues.Json.Url.Set.SCENES:
                    url = ConstantValues.Url.Scene.GET;
                    jsonUrl = url
                            +  idx
                            + ConstantValues.Url.Switch.CMD + actionUrl;
                    break;

                case ConstantValues.Json.Url.Set.SWITCHES:
                    url = ConstantValues.Url.Switch.GET;
                    jsonUrl = url
                            +  idx
                            + ConstantValues.Url.Switch.CMD + actionUrl;
                    break;

                case ConstantValues.Json.Url.Set.MODAL_SWITCHES:
                    url = ConstantValues.Url.ModalSwitch.GET;
                    jsonUrl = url
                            +  idx
                            + ConstantValues.Url.ModalSwitch.STATUS + actionUrl;
                    break;
                case ConstantValues.Json.Url.Set.TEMP:
                    url = ConstantValues.Url.Temp.GET;
                    jsonUrl = url
                            +  idx
                            + ConstantValues.Url.Temp.VALUE + actionUrl;
                    break;

                case ConstantValues.Json.Url.Set.SCENEFAVORITE:
                    url = ConstantValues.Url.Favorite.SCENE;
                    jsonUrl = url
                            +  idx
                            + ConstantValues.Url.Favorite.VALUE + actionUrl;
                    break;

                case ConstantValues.Json.Url.Set.FAVORITE:
                    url = ConstantValues.Url.Favorite.GET;
                    jsonUrl = url
                            +  idx
                            + ConstantValues.Url.Favorite.VALUE + actionUrl;
                    break;

                case ConstantValues.Json.Url.Set.RGBCOLOR:
                    url = ConstantValues.Url.System.RGBCOLOR;
                    jsonUrl = url
                            +  idx
                            + actionUrl;
                    break;

                case ConstantValues.Json.Url.Set.EVENTS_UPDATE_STATUS:
                    url = ConstantValues.Url.System.EVENTS_UPDATE_STATUS;
                    jsonUrl = url
                            + idx
                            + actionUrl;
                    break;
            }

            String fullString = String.Format("{0}{1}:{2}{3}{4}", protocol, baseUrl, port,
                String.IsNullOrEmpty(directory) ? "" : "/" + directory,
                jsonUrl);

            System.Diagnostics.Debug.WriteLine("Domoticz Action Url: " + fullString);
            return fullString;
        }

        /// <summary>
        /// Is User On Local Wifi Async
        /// </summary>
        public async Task<bool> IsUserOnLocalWifiAsync(ServerSettings server)
        {
            if (server != null && !String.IsNullOrEmpty(server.LOCAL_SERVER_URL))
                return await CrossConnectivity.Current.IsReachable(server.LOCAL_SERVER_URL, 1000);
            return false;
        }
    }
}
