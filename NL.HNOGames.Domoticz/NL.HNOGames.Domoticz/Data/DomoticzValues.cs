using System;

namespace NL.HNOGames.Domoticz.Data
{
    /// <summary>
    /// Defines the <see cref="DomoticzValues" />
    /// </summary>
    public static class DomoticzValues
    {
        /// <summary>
        /// Defines the <see cref="Url" />
        /// </summary>
        public static class Url
        {
            /// <summary>
            /// Defines the <see cref="Action" />
            /// </summary>
            public static class Action
            {
                #region Variables

                /// <summary>
                /// Defines the ON
                /// </summary>
                public static String ON = "On";

                /// <summary>
                /// Defines the OFF
                /// </summary>
                public static String OFF = "Off";

                /// <summary>
                /// Defines the UP
                /// </summary>
                public static String UP = "Up";

                /// <summary>
                /// Defines the STOP
                /// </summary>
                public static String STOP = "Stop";

                /// <summary>
                /// Defines the DOWN
                /// </summary>
                public static String DOWN = "Down";

                /// <summary>
                /// Defines the PLUS
                /// </summary>
                public static String PLUS = "Plus";

                /// <summary>
                /// Defines the MIN
                /// </summary>
                public static String MIN = "Min";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="ModalAction" />
            /// </summary>
            public static class ModalAction
            {
                #region Variables

                /// <summary>
                /// Defines the AUTO
                /// </summary>
                public static String AUTO = "Auto";

                /// <summary>
                /// Defines the ECONOMY
                /// </summary>
                public static String ECONOMY = "AutoWithEco";

                /// <summary>
                /// Defines the AWAY
                /// </summary>
                public static String AWAY = "Away";

                /// <summary>
                /// Defines the DAY_OFF
                /// </summary>
                public static String DAY_OFF = "DayOff";

                /// <summary>
                /// Defines the CUSTOM
                /// </summary>
                public static String CUSTOM = "Custom";

                /// <summary>
                /// Defines the HEATING_OFF
                /// </summary>
                public static String HEATING_OFF = "HeatingOff";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Category" />
            /// </summary>
            public static class Category
            {
                #region Variables

                /// <summary>
                /// Defines the ALLDEVICES
                /// </summary>
                public static String ALLDEVICES = "/json.htm?type=devices";

                /// <summary>
                /// Defines the DEVICES
                /// </summary>
                public static String DEVICES = "/json.htm?type=devices&filter=all&used=true";

                /// <summary>
                /// Defines the FAVORITES
                /// </summary>
                public static String FAVORITES = "/json.htm?type=devices&filter=all&used=true&favorite=1";

                /// <summary>
                /// Defines the VERSION
                /// </summary>
                public static String VERSION = "/json.htm?type=command&param=getversion";

                /// <summary>
                /// Defines the DASHBOARD
                /// </summary>
                public static String DASHBOARD = ALLDEVICES + "&filter=all";

                /// <summary>
                /// Defines the SCENES
                /// </summary>
                public static String SCENES = "/json.htm?type=scenes";

                /// <summary>
                /// Defines the SWITCHES
                /// </summary>
                public static String SWITCHES = "/json.htm?type=command&param=getlightswitches";

                /// <summary>
                /// Defines the WEATHER
                /// </summary>
                public static String WEATHER = ALLDEVICES + "&filter=weather&used=true";

                /// <summary>
                /// Defines the CAMERAS
                /// </summary>
                public static String CAMERAS = "/json.htm?type=cameras";

                /// <summary>
                /// Defines the CAMERA
                /// </summary>
                public static String CAMERA = "/camsnapshot.jpg?idx=";

                /// <summary>
                /// Defines the UTILITIES
                /// </summary>
                public static String UTILITIES = ALLDEVICES + "&filter=utility&used=true";

                /// <summary>
                /// Defines the PLANS
                /// </summary>
                public static String PLANS = "/json.htm?type=plans";

                /// <summary>
                /// Defines the TEMPERATURE
                /// </summary>
                public static String TEMPERATURE = ALLDEVICES + "&filter=temp&used=true";

                /// <summary>
                /// Defines the SWITCHLOG
                /// </summary>
                public static String SWITCHLOG = "/json.htm?type=lightlog&idx=";

                /// <summary>
                /// Defines the TEXTLOG
                /// </summary>
                public static String TEXTLOG = "/json.htm?type=textlog&idx=";

                /// <summary>
                /// Defines the SCENELOG
                /// </summary>
                public static String SCENELOG = "/json.htm?type=scenelog&idx=";

                /// <summary>
                /// Defines the SWITCHTIMER
                /// </summary>
                public static String SWITCHTIMER = "/json.htm?type=timers&idx=";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Switch" />
            /// </summary>
            public static class Switch
            {
                #region Variables

                /// <summary>
                /// Defines the DIM_LEVEL
                /// </summary>
                public static String DIM_LEVEL = "Set%20Level&level=";

                /// <summary>
                /// Defines the COLOR
                /// </summary>
                public static String COLOR = "&hue=%hue%&brightness=%bright%&iswhite=false";

                /// <summary>
                /// Defines the GET
                /// </summary>
                public static String GET = "/json.htm?type=command&param=switchlight&idx=";

                /// <summary>
                /// Defines the CMD
                /// </summary>
                public static String CMD = "&switchcmd=";

                /// <summary>
                /// Defines the LEVEL
                /// </summary>
                public static String LEVEL = "&level=";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="ModalSwitch" />
            /// </summary>
            public static class ModalSwitch
            {
                #region Variables

                /// <summary>
                /// Defines the GET
                /// </summary>
                public static String GET = "/json.htm?type=command&param=switchmodal&idx=";

                /// <summary>
                /// Defines the STATUS
                /// </summary>
                public static String STATUS = "&status=";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Scene" />
            /// </summary>
            public static class Scene
            {
                #region Variables

                /// <summary>
                /// Defines the GET
                /// </summary>
                public static String GET = "/json.htm?type=command&param=switchscene&idx=";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Temp" />
            /// </summary>
            public static class Temp
            {
                #region Variables

                /// <summary>
                /// Defines the GET
                /// </summary>
                public static String GET = "/json.htm?type=command&param=udevice&idx=";

                /// <summary>
                /// Defines the VALUE
                /// </summary>
                public static String VALUE = "&nvalue=0&svalue=";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Favorite" />
            /// </summary>
            public static class Favorite
            {
                #region Variables

                /// <summary>
                /// Defines the GET
                /// </summary>
                public static String GET = "/json.htm?type=command&param=makefavorite&idx=";

                /// <summary>
                /// Defines the SCENE
                /// </summary>
                public static String SCENE = "/json.htm?type=command&param=makescenefavorite&idx=";

                /// <summary>
                /// Defines the VALUE
                /// </summary>
                public static String VALUE = "&isfavorite=";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Protocol" />
            /// </summary>
            public static class Protocol
            {
                #region Variables

                /// <summary>
                /// Defines the HTTP
                /// </summary>
                public static String HTTP = "http://";

                /// <summary>
                /// Defines the HTTPS
                /// </summary>
                public static String HTTPS = "https://";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Device" />
            /// </summary>
            public static class Device
            {
                #region Variables

                /// <summary>
                /// Defines the STATUS
                /// </summary>
                public static String STATUS = "/json.htm?type=devices&rid=";

                /// <summary>
                /// Defines the SET_USED
                /// </summary>
                public static String SET_USED = "/json.htm?type=setused&idx=";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Sunrise" />
            /// </summary>
            public static class Sunrise
            {
                #region Variables

                /// <summary>
                /// Defines the GET
                /// </summary>
                public static String GET = "/json.htm?type=command&param=getSunRiseSet";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Plan" />
            /// </summary>
            public static class Plan
            {
                #region Variables

                /// <summary>
                /// Defines the GET
                /// </summary>
                public static String GET = "/json.htm?type=plans";

                /// <summary>
                /// Defines the DEVICES
                /// </summary>
                public static String DEVICES = "/json.htm?type=command&param=getplandevices&idx=";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Log" />
            /// </summary>
            public static class Log
            {
                #region Variables

                /// <summary>
                /// Defines the GRAPH
                /// </summary>
                public static String GRAPH = "/json.htm?type=graph&idx=";

                /// <summary>
                /// Defines the GRAPH_RANGE
                /// </summary>
                public static String GRAPH_RANGE = "&range=";

                /// <summary>
                /// Defines the GRAPH_TYPE
                /// </summary>
                public static String GRAPH_TYPE = "&sensor=";

                /// <summary>
                /// Defines the GET_LOG
                /// </summary>
                public static String GET_LOG = "/json.htm?type=command&param=getlog";

                /// <summary>
                /// Defines the GET_FROMLASTLOGTIME
                /// </summary>
                public static String GET_FROMLASTLOGTIME = "/json.htm?type=command&param=getlog&lastlogtime=";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Notification" />
            /// </summary>
            public static class Notification
            {
                #region Variables

                /// <summary>
                /// Defines the NOTIFICATION
                /// </summary>
                public static String NOTIFICATION = "/json.htm?type=notifications&idx=";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Security" />
            /// </summary>
            public static class Security
            {
                #region Variables

                /// <summary>
                /// Defines the GET
                /// </summary>
                public static String GET = "/json.htm?type=command&param=getsecstatus";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="UserVariable" />
            /// </summary>
            public static class UserVariable
            {
                #region Variables

                /// <summary>
                /// Defines the UPDATE
                /// </summary>
                public static String UPDATE = "/json.htm?type=command&param=updateuservariable";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="System" />
            /// </summary>
            public static class System
            {
                #region Variables

                /// <summary>
                /// Defines the UPDATE
                /// </summary>
                public static String UPDATE = "/json.htm?type=command&param=checkforupdate&forced=true";

                /// <summary>
                /// Defines the USERVARIABLES
                /// </summary>
                public static String USERVARIABLES = "/json.htm?type=command&param=getuservariables";

                /// <summary>
                /// Defines the EVENTS
                /// </summary>
                public static String EVENTS = "/json.htm?type=events&param=list";

                /// <summary>
                /// Defines the EVENTS_UPDATE_STATUS
                /// </summary>
                public static String EVENTS_UPDATE_STATUS = "/json.htm?type=events&param=updatestatus&eventid=";

                /// <summary>
                /// Defines the RGBCOLOR
                /// </summary>
                public static String RGBCOLOR = "/json.htm?type=command&param=setcolbrightnessvalue&idx=";

                /// <summary>
                /// Defines the SETTINGS
                /// </summary>
                public static String SETTINGS = "/json.htm?type=settings";

                /// <summary>
                /// Defines the CONFIG
                /// </summary>
                public static String CONFIG = "/json.htm?type=command&param=getconfig";

                /// <summary>
                /// Defines the SETSECURITY
                /// </summary>
                public static String SETSECURITY = "/json.htm?type=command&param=setsecstatus";

                /// <summary>
                /// Defines the DOWNLOAD_READY
                /// </summary>
                public static String DOWNLOAD_READY = "/json.htm?type=command&param=downloadready";

                /// <summary>
                /// Defines the UPDATE_DOMOTICZ_SERVER
                /// </summary>
                public static String UPDATE_DOMOTICZ_SERVER = "/json.htm?type=command&param=execute_script&scriptname=update_domoticz&direct=true";

                /// <summary>
                /// Defines the ADD_MOBILE_DEVICE
                /// </summary>
                public static String ADD_MOBILE_DEVICE = "/json.htm?type=command&param=addmobiledevice";

                /// <summary>
                /// Defines the CLEAN_MOBILE_DEVICE
                /// </summary>
                public static String CLEAN_MOBILE_DEVICE = "/json.htm?type=command&param=deletemobiledevice";

                /// <summary>
                /// Defines the LANGUAGE_TRANSLATIONS
                /// </summary>
                public static String LANGUAGE_TRANSLATIONS = "/i18n/domoticz-";

                /// <summary>
                /// Defines the USERS
                /// </summary>
                public static String USERS = "/json.htm?type=users";

                /// <summary>
                /// Defines the AUTH
                /// </summary>
                public static String AUTH = "/json.htm?type=command&param=getauth";

                /// <summary>
                /// Defines the LOGOFF
                /// </summary>
                public static String LOGOFF = "/json.htm?type=command&param=dologout";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Event" />
            /// </summary>
            public static class Event
            {
                #region Variables

                /// <summary>
                /// Defines the ON
                /// </summary>
                public static String ON = "&eventstatus=1";

                /// <summary>
                /// Defines the OFF
                /// </summary>
                public static String OFF = "&eventstatus=0";

                #endregion
            }
        }
    }
}
