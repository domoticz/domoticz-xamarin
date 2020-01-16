using System;
using System.Collections.Generic;
using System.Linq;

namespace NL.HNOGames.Domoticz.Data
{
    /// <summary>
    /// Defines the <see cref="ConstantValues" />
    /// </summary>
    public static class ConstantValues
    {
        #region Public

        /// <summary>
        /// The GetSelectorValue
        /// </summary>
        /// <param name="mDevicesInfo">The mDevicesInfo<see cref="Models.Device"/></param>
        /// <param name="value">The value<see cref="string"/></param>
        /// <returns>The <see cref="int"/></returns>
        public static int GetSelectorValue(Models.Device mDevicesInfo, string value)
        {
            if (mDevicesInfo?.LevelNamesArray == null)
                return 0;
            var jsonValue = 0;
            if (string.IsNullOrEmpty(value)) return jsonValue;
            var levelNames = new List<string>(mDevicesInfo.LevelNamesArray);
            var counter = levelNames.TakeWhile(l => string.Compare(l, value, StringComparison.OrdinalIgnoreCase) != 0).Sum(l => 10);
            jsonValue = counter;
            return jsonValue;
        }

        /// <summary>
        /// The CanHandleAutomatedToggle
        /// </summary>
        /// <param name="mDeviceInfo">The mDeviceInfo<see cref="Models.Device"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public static bool CanHandleAutomatedToggle(Models.Device mDeviceInfo)
        {
            if (mDeviceInfo == null)
                return false;
            if (mDeviceInfo.SwitchTypeVal == 0 &&
                (mDeviceInfo.SwitchType == null))
            {
                if (mDeviceInfo.SubType != null && mDeviceInfo.SubType ==
                    Device.Utility.SubType.SMARTWARES)
                    return true;
                switch (mDeviceInfo.Type)
                {
                    case Device.Scene.Type.GROUP:
                        return true;
                    case Device.Scene.Type.SCENE:
                        return true;
                    case Device.Utility.Type.THERMOSTAT:
                        return false;
                    case Device.Utility.Type.HEATING:
                        return false;
                    default:
                        return false;
                }
            }
            if ((mDeviceInfo.SwitchType == null))
                return false;
            switch (mDeviceInfo.SwitchTypeVal)
            {
                case Device.Type.Value.ON_OFF:
                case Device.Type.Value.MEDIAPLAYER:
                case Device.Type.Value.DOORLOCK:
                case Device.Type.Value.DOORCONTACT:
                    switch (mDeviceInfo.SwitchType)
                    {
                        case Device.Type.Name.SECURITY:
                            return false;
                        default:
                            return true;
                    }
                case Device.Type.Value.X10SIREN:
                case Device.Type.Value.MOTION:
                case Device.Type.Value.CONTACT:
                case Device.Type.Value.DUSKSENSOR:
                case Device.Type.Value.SMOKE_DETECTOR:
                case Device.Type.Value.DOORBELL:
                case Device.Type.Value.PUSH_ON_BUTTON:
                case Device.Type.Value.PUSH_OFF_BUTTON:
                case Device.Type.Value.DIMMER:
                case Device.Type.Value.BLINDPERCENTAGE:
                case Device.Type.Value.BLINDPERCENTAGEINVERTED:
                case Device.Type.Value.SELECTOR:
                case Device.Type.Value.BLINDS:
                case Device.Type.Value.BLINDINVERTED:
                case Device.Type.Value.BLINDVENETIAN:
                case Device.Type.Value.BLINDVENETIANUS:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// The CanHandleStopButton
        /// </summary>
        /// <param name="mDeviceInfo">The mDeviceInfo<see cref="Models.Device"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public static bool CanHandleStopButton(Models.Device mDeviceInfo)
        {
            return (mDeviceInfo.SubType.Contains("RAEX")) ||
                   (mDeviceInfo.SubType.Contains("A-OK")) ||
                   (mDeviceInfo.SubType.Contains("Harrison")) ||
                   (mDeviceInfo.SubType.Contains("RFY")) ||
                   (mDeviceInfo.SubType.Contains("ASA")) ||
                   (mDeviceInfo.SubType.Contains("Hasta")) ||
                   (mDeviceInfo.SubType.Contains("Media Mount")) ||
                   (mDeviceInfo.SubType.Contains("Forest")) ||
                   (mDeviceInfo.SubType.Contains("Chamberlain")) ||
                   (mDeviceInfo.SubType.Contains("Sunpery")) ||
                   (mDeviceInfo.SubType.Contains("Dolat")) ||
                   (mDeviceInfo.SubType.Contains("DC106")) ||
                   (mDeviceInfo.SubType.Contains("Confexx")) ||
                   (mDeviceInfo.SubType.Contains("ASP"));
        }

        #endregion

        /// <summary>
        /// Defines the GraphRange
        /// </summary>
        public enum GraphRange
        {
            /// <summary>
            /// Defines the Day
            /// </summary>
            Day,

            /// <summary>
            /// Defines the Month
            /// </summary>
            Month,

            /// <summary>
            /// Defines the Year
            /// </summary>
            Year
        }

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
                #region Constants

                /// <summary>
                /// Defines the ON
                /// </summary>
                public const string ON = "On";

                /// <summary>
                /// Defines the OFF
                /// </summary>
                public const string OFF = "Off";

                /// <summary>
                /// Defines the UP
                /// </summary>
                public const string UP = "Up";

                /// <summary>
                /// Defines the STOP
                /// </summary>
                public const string STOP = "Stop";

                /// <summary>
                /// Defines the DOWN
                /// </summary>
                public const string DOWN = "Down";

                /// <summary>
                /// Defines the PLUS
                /// </summary>
                public const string PLUS = "Plus";

                /// <summary>
                /// Defines the MIN
                /// </summary>
                public const string MIN = "Min";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="ModalAction" />
            /// </summary>
            public static class ModalAction
            {
                #region Constants

                /// <summary>
                /// Defines the AUTO
                /// </summary>
                public const string AUTO = "Auto";

                /// <summary>
                /// Defines the ECONOMY
                /// </summary>
                public const string ECONOMY = "AutoWithEco";

                /// <summary>
                /// Defines the AWAY
                /// </summary>
                public const string AWAY = "Away";

                /// <summary>
                /// Defines the DAY_OFF
                /// </summary>
                public const string DAY_OFF = "DayOff";

                /// <summary>
                /// Defines the CUSTOM
                /// </summary>
                public const string CUSTOM = "Custom";

                /// <summary>
                /// Defines the HEATING_OFF
                /// </summary>
                public const string HEATING_OFF = "HeatingOff";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Category" />
            /// </summary>
            public static class Category
            {
                #region Constants

                /// <summary>
                /// Defines the ALLDEVICES
                /// </summary>
                public const string ALLDEVICES = "/json.htm?type=devices";

                /// <summary>
                /// Defines the DEVICES
                /// </summary>
                public const string DEVICES = "/json.htm?type=devices&filter=all&used=true";

                /// <summary>
                /// Defines the FAVORITES
                /// </summary>
                public const string FAVORITES = "/json.htm?type=devices&filter=all&used=true&favorite=1";

                /// <summary>
                /// Defines the VERSION
                /// </summary>
                public const string VERSION = "/json.htm?type=command&param=getversion";

                /// <summary>
                /// Defines the SUNRISE
                /// </summary>
                public const string SUNRISE = "/json.htm?type=command&param=getSunRiseSet";

                /// <summary>
                /// Defines the DASHBOARD
                /// </summary>
                public const string DASHBOARD = ALLDEVICES + "&filter=all";

                /// <summary>
                /// Defines the SCENES
                /// </summary>
                public const string SCENES = "/json.htm?type=scenes";

                /// <summary>
                /// Defines the SWITCHES
                /// </summary>
                public const string SWITCHES = "/json.htm?type=command&param=getlightswitches";

                /// <summary>
                /// Defines the WEATHER
                /// </summary>
                public const string WEATHER = ALLDEVICES + "&filter=weather&used=true";

                /// <summary>
                /// Defines the CAMERAS
                /// </summary>
                public const string CAMERAS = "/json.htm?type=cameras";

                /// <summary>
                /// Defines the CAMERA
                /// </summary>
                public const string CAMERA = "/camsnapshot.jpg?idx=";

                /// <summary>
                /// Defines the UTILITIES
                /// </summary>
                public const string UTILITIES = ALLDEVICES + "&filter=utility&used=true";

                /// <summary>
                /// Defines the PLANS
                /// </summary>
                public const string PLANS = "/json.htm?type=plans";

                /// <summary>
                /// Defines the TEMPERATURE
                /// </summary>
                public const string TEMPERATURE = ALLDEVICES + "&filter=temp&used=true";

                /// <summary>
                /// Defines the SWITCHLOG
                /// </summary>
                public const string SWITCHLOG = "/json.htm?type=lightlog&idx=";

                /// <summary>
                /// Defines the TEXTLOG
                /// </summary>
                public const string TEXTLOG = "/json.htm?type=textlog&idx=";

                /// <summary>
                /// Defines the SCENELOG
                /// </summary>
                public const string SCENELOG = "/json.htm?type=scenelog&idx=";

                /// <summary>
                /// Defines the SWITCHTIMER
                /// </summary>
                public const string SWITCHTIMER = "/json.htm?type=timers&idx=";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Switch" />
            /// </summary>
            public static class Switch
            {
                #region Constants

                /// <summary>
                /// Defines the DIM_LEVEL
                /// </summary>
                public const string DIM_LEVEL = "Set%20Level&level=";

                /// <summary>
                /// Defines the COLOR
                /// </summary>
                public const string COLOR = "&hue=%hue%&brightness=%bright%&iswhite=false";

                /// <summary>
                /// Defines the HEXCOLOR
                /// </summary>
                public const string HEXCOLOR = "&hex=%hex%&brightness=%bright%&iswhite=false";

                /// <summary>
                /// Defines the GET
                /// </summary>
                public const string GET = "/json.htm?type=command&param=switchlight&idx=";

                /// <summary>
                /// Defines the CMD
                /// </summary>
                public const string CMD = "&switchcmd=";

                /// <summary>
                /// Defines the LEVEL
                /// </summary>
                public const string LEVEL = "&level=";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="ModalSwitch" />
            /// </summary>
            public static class ModalSwitch
            {
                #region Constants

                /// <summary>
                /// Defines the GET
                /// </summary>
                public const string GET = "/json.htm?type=command&param=switchmodal&idx=";

                /// <summary>
                /// Defines the STATUS
                /// </summary>
                public const string STATUS = "&status=";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Scene" />
            /// </summary>
            public static class Scene
            {
                #region Constants

                /// <summary>
                /// Defines the GET
                /// </summary>
                public const string GET = "/json.htm?type=command&param=switchscene&idx=";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Temp" />
            /// </summary>
            public static class Temp
            {
                #region Constants

                /// <summary>
                /// Defines the GET
                /// </summary>
                public const string GET = "/json.htm?type=command&param=udevice&idx=";

                /// <summary>
                /// Defines the VALUE
                /// </summary>
                public const string VALUE = "&nvalue=0&svalue=";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Favorite" />
            /// </summary>
            public static class Favorite
            {
                #region Constants

                /// <summary>
                /// Defines the GET
                /// </summary>
                public const string GET = "/json.htm?type=command&param=makefavorite&idx=";

                /// <summary>
                /// Defines the SCENE
                /// </summary>
                public const string SCENE = "/json.htm?type=command&param=makescenefavorite&idx=";

                /// <summary>
                /// Defines the VALUE
                /// </summary>
                public const string VALUE = "&isfavorite=";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Protocol" />
            /// </summary>
            public static class Protocol
            {
                #region Constants

                /// <summary>
                /// Defines the HTTP
                /// </summary>
                public const string HTTP = "http://";

                /// <summary>
                /// Defines the HTTPS
                /// </summary>
                public const string HTTPS = "https://";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Device" />
            /// </summary>
            public static class Device
            {
                #region Constants

                /// <summary>
                /// Defines the STATUS
                /// </summary>
                public const string STATUS = "/json.htm?type=devices&rid=";

                /// <summary>
                /// Defines the SET_USED
                /// </summary>
                public const string SET_USED = "/json.htm?type=setused&idx=";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Sunrise" />
            /// </summary>
            public static class Sunrise
            {
                #region Constants

                /// <summary>
                /// Defines the GET
                /// </summary>
                public const string GET = "/json.htm?type=command&param=getSunRiseSet";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Plan" />
            /// </summary>
            public static class Plan
            {
                #region Constants

                /// <summary>
                /// Defines the GET
                /// </summary>
                public const string GET = "/json.htm?type=plans";

                /// <summary>
                /// Defines the DEVICES
                /// </summary>
                public const string DEVICES = "/json.htm?type=command&param=getplandevices&idx=";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Log" />
            /// </summary>
            public static class Log
            {
                #region Constants

                /// <summary>
                /// Defines the GRAPH
                /// </summary>
                public const string GRAPH = "/json.htm?type=graph&idx=";

                /// <summary>
                /// Defines the GRAPH_RANGE
                /// </summary>
                public const string GRAPH_RANGE = "&range=";

                /// <summary>
                /// Defines the GRAPH_TYPE
                /// </summary>
                public const string GRAPH_TYPE = "&sensor=";

                /// <summary>
                /// Defines the GET_LOG
                /// </summary>
                public const string GET_LOG = "/json.htm?type=command&param=getlog";

                /// <summary>
                /// Defines the GET_FROMLASTLOGTIME
                /// </summary>
                public const string GET_FROMLASTLOGTIME = "/json.htm?type=command&param=getlog&lastlogtime=";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Notification" />
            /// </summary>
            public static class Notification
            {
                #region Constants

                /// <summary>
                /// Defines the NOTIFICATION
                /// </summary>
                public const string NOTIFICATION = "/json.htm?type=notifications&idx=";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Security" />
            /// </summary>
            public static class Security
            {
                #region Constants

                /// <summary>
                /// Defines the GET
                /// </summary>
                public const string GET = "/json.htm?type=command&param=getsecstatus";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="UserVariable" />
            /// </summary>
            public static class UserVariable
            {
                #region Constants

                /// <summary>
                /// Defines the UPDATE
                /// </summary>
                public const string UPDATE = "/json.htm?type=command&param=updateuservariable";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="System" />
            /// </summary>
            public static class System
            {
                #region Constants

                /// <summary>
                /// Defines the UPDATE
                /// </summary>
                public const string UPDATE = "/json.htm?type=command&param=checkforupdate&forced=true";

                /// <summary>
                /// Defines the USERVARIABLES
                /// </summary>
                public const string USERVARIABLES = "/json.htm?type=command&param=getuservariables";

                /// <summary>
                /// Defines the EVENTS
                /// </summary>
                public const string EVENTS = "/json.htm?type=events&param=list";

                /// <summary>
                /// Defines the EVENTS_UPDATE_STATUS
                /// </summary>
                public const string EVENTS_UPDATE_STATUS = "/json.htm?type=events&param=updatestatus&eventid=";

                /// <summary>
                /// Defines the RGBCOLOR
                /// </summary>
                public const string RGBCOLOR = "/json.htm?type=command&param=setcolbrightnessvalue&idx=";

                /// <summary>
                /// Defines the SETTINGS
                /// </summary>
                public const string SETTINGS = "/json.htm?type=settings";

                /// <summary>
                /// Defines the CONFIG
                /// </summary>
                public const string CONFIG = "/json.htm?type=command&param=getconfig";

                /// <summary>
                /// Defines the SETSECURITY
                /// </summary>
                public const string SETSECURITY = "/json.htm?type=command&param=setsecstatus";

                /// <summary>
                /// Defines the DOWNLOAD_READY
                /// </summary>
                public const string DOWNLOAD_READY = "/json.htm?type=command&param=downloadready";

                /// <summary>
                /// Defines the UPDATE_DOMOTICZ_SERVER
                /// </summary>
                public const string UPDATE_DOMOTICZ_SERVER =
                "/json.htm?type=command&param=execute_script&scriptname=update_domoticz&direct=true";

                /// <summary>
                /// Defines the ADD_MOBILE_DEVICE
                /// </summary>
                public const string ADD_MOBILE_DEVICE = "/json.htm?type=command&param=addmobiledevice";

                /// <summary>
                /// Defines the CLEAN_MOBILE_DEVICE
                /// </summary>
                public const string CLEAN_MOBILE_DEVICE = "/json.htm?type=command&param=deletemobiledevice";

                /// <summary>
                /// Defines the LANGUAGE_TRANSLATIONS
                /// </summary>
                public const string LANGUAGE_TRANSLATIONS = "/i18n/domoticz-";

                /// <summary>
                /// Defines the USERS
                /// </summary>
                public const string USERS = "/json.htm?type=users";

                /// <summary>
                /// Defines the AUTH
                /// </summary>
                public const string AUTH = "/json.htm?type=command&param=getauth";

                /// <summary>
                /// Defines the LOGOFF
                /// </summary>
                public const string LOGOFF = "/json.htm?type=command&param=dologout";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Event" />
            /// </summary>
            public static class Event
            {
                #region Constants

                /// <summary>
                /// Defines the ON
                /// </summary>
                public const string ON = "&eventstatus=1";

                /// <summary>
                /// Defines the OFF
                /// </summary>
                public const string OFF = "&eventstatus=0";

                #endregion
            }
        }

        /// <summary>
        /// Defines the <see cref="Event" />
        /// </summary>
        public static class Event
        {
            /// <summary>
            /// Defines the <see cref="Type" />
            /// </summary>
            public static class Type
            {
                #region Constants

                /// <summary>
                /// Defines the EVENT
                /// </summary>
                public const string EVENT = "Event";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Action" />
            /// </summary>
            public static class Action
            {
                #region Constants

                /// <summary>
                /// Defines the ON
                /// </summary>
                public const int ON = 55;

                /// <summary>
                /// Defines the OFF
                /// </summary>
                public const int OFF = 56;

                #endregion
            }
        }

        /// <summary>
        /// Defines the <see cref="Security" />
        /// </summary>
        public static class Security
        {
            /// <summary>
            /// Defines the <see cref="Status" />
            /// </summary>
            public static class Status
            {
                #region Constants

                /// <summary>
                /// Defines the ARMHOME
                /// </summary>
                public const int ARMHOME = 1;

                /// <summary>
                /// Defines the ARMAWAY
                /// </summary>
                public const int ARMAWAY = 2;

                /// <summary>
                /// Defines the DISARM
                /// </summary>
                public const int DISARM = 0;

                #endregion
            }
        }

        /// <summary>
        /// Defines the <see cref="Device" />
        /// </summary>
        public static class Device
        {
            /// <summary>
            /// Defines the <see cref="Scene" />
            /// </summary>
            public static class Scene
            {
                /// <summary>
                /// Defines the <see cref="Type" />
                /// </summary>
                public static class Type
                {
                    #region Constants

                    /// <summary>
                    /// Defines the GROUP
                    /// </summary>
                    public const string GROUP = "Group";

                    /// <summary>
                    /// Defines the SCENE
                    /// </summary>
                    public const string SCENE = "Scene";

                    #endregion
                }

                /// <summary>
                /// Defines the <see cref="Action" />
                /// </summary>
                public static class Action
                {
                    #region Constants

                    /// <summary>
                    /// Defines the ON
                    /// </summary>
                    public const int ON = 40;

                    /// <summary>
                    /// Defines the OFF
                    /// </summary>
                    public const int OFF = 41;

                    #endregion
                }
            }

            /// <summary>
            /// Defines the <see cref="Door" />
            /// </summary>
            public static class Door
            {
                /// <summary>
                /// Defines the <see cref="State" />
                /// </summary>
                public static class State
                {
                    #region Constants

                    /// <summary>
                    /// Defines the UNLOCKED
                    /// </summary>
                    public const string UNLOCKED = "Unlocked";

                    /// <summary>
                    /// Defines the OPEN
                    /// </summary>
                    public const string OPEN = "Open";

                    #endregion
                }
            }

            /// <summary>
            /// Defines the <see cref="Switch" />
            /// </summary>
            public static class Switch
            {
                /// <summary>
                /// Defines the <see cref="Action" />
                /// </summary>
                public static class Action
                {
                    #region Constants

                    /// <summary>
                    /// Defines the ON
                    /// </summary>
                    public const int ON = 10;

                    /// <summary>
                    /// Defines the OFF
                    /// </summary>
                    public const int OFF = 11;

                    #endregion
                }
            }

            /// <summary>
            /// Defines the <see cref="Hardware" />
            /// </summary>
            public static class Hardware
            {
                #region Constants

                /// <summary>
                /// Defines the EVOHOME
                /// </summary>
                public const string EVOHOME = "evohome";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Dimmer" />
            /// </summary>
            public static class Dimmer
            {
                /// <summary>
                /// Defines the <see cref="Action" />
                /// </summary>
                public static class Action
                {
                    #region Constants

                    /// <summary>
                    /// Defines the DIM_LEVEL
                    /// </summary>
                    public const int DIM_LEVEL = 20;

                    /// <summary>
                    /// Defines the COLOR
                    /// </summary>
                    public const int COLOR = 21;

                    /// <summary>
                    /// Defines the HEXCOLOR
                    /// </summary>
                    public const int HEXCOLOR = 222;

                    #endregion
                }
            }

            /// <summary>
            /// Defines the <see cref="Utility" />
            /// </summary>
            public static class Utility
            {
                /// <summary>
                /// Defines the <see cref="Type" />
                /// </summary>
                public static class Type
                {
                    #region Constants

                    /// <summary>
                    /// Defines the HEATING
                    /// </summary>
                    public const string HEATING = "Heating";

                    /// <summary>
                    /// Defines the THERMOSTAT
                    /// </summary>
                    public const string THERMOSTAT = "Thermostat";

                    #endregion
                }

                /// <summary>
                /// Defines the <see cref="SubType" />
                /// </summary>
                public static class SubType
                {
                    #region Constants

                    /// <summary>
                    /// Defines the TEXT
                    /// </summary>
                    public const string TEXT = "Text";

                    /// <summary>
                    /// Defines the ALERT
                    /// </summary>
                    public const string ALERT = "Alert";

                    /// <summary>
                    /// Defines the PERCENTAGE
                    /// </summary>
                    public const string PERCENTAGE = "Percentage";

                    /// <summary>
                    /// Defines the ENERGY
                    /// </summary>
                    public const string ENERGY = "Energy";

                    /// <summary>
                    /// Defines the KWH
                    /// </summary>
                    public const string KWH = "kWh";

                    /// <summary>
                    /// Defines the GAS
                    /// </summary>
                    public const string GAS = "Gas";

                    /// <summary>
                    /// Defines the ELECTRIC
                    /// </summary>
                    public const string ELECTRIC = "Electric";

                    /// <summary>
                    /// Defines the VOLTCRAFT
                    /// </summary>
                    public const string VOLTCRAFT = "Voltcraft";

                    /// <summary>
                    /// Defines the SETPOINT
                    /// </summary>
                    public const string SETPOINT = "SetPoint";

                    /// <summary>
                    /// Defines the YOULESS
                    /// </summary>
                    public const string YOULESS = "YouLess";

                    /// <summary>
                    /// Defines the SMARTWARES
                    /// </summary>
                    public const string SMARTWARES = "Smartwares";

                    #endregion
                }
            }

            /// <summary>
            /// Defines the <see cref="Blind" />
            /// </summary>
            public static class Blind
            {
                /// <summary>
                /// Defines the <see cref="State" />
                /// </summary>
                public static class State
                {
                    #region Constants

                    /// <summary>
                    /// Defines the CLOSED
                    /// </summary>
                    public const string CLOSED = "Closed";

                    /// <summary>
                    /// Defines the OPEN
                    /// </summary>
                    public const string OPEN = "Open";

                    /// <summary>
                    /// Defines the STOPPED
                    /// </summary>
                    public const string STOPPED = "Stopped";

                    /// <summary>
                    /// Defines the ON
                    /// </summary>
                    public const string ON = "On";

                    /// <summary>
                    /// Defines the OFF
                    /// </summary>
                    public const string OFF = "Off";

                    #endregion
                }

                /// <summary>
                /// Defines the <see cref="Action" />
                /// </summary>
                public static class Action
                {
                    #region Constants

                    /// <summary>
                    /// Defines the UP
                    /// </summary>
                    public const int UP = 30;

                    /// <summary>
                    /// Defines the STOP
                    /// </summary>
                    public const int STOP = 31;

                    /// <summary>
                    /// Defines the ON
                    /// </summary>
                    public const int ON = 33;

                    /// <summary>
                    /// Defines the OFF
                    /// </summary>
                    public const int OFF = 34;

                    /// <summary>
                    /// Defines the DOWN
                    /// </summary>
                    public const int DOWN = 32;

                    #endregion
                }
            }

            /// <summary>
            /// Defines the <see cref="Thermostat" />
            /// </summary>
            public static class Thermostat
            {
                /// <summary>
                /// Defines the <see cref="Action" />
                /// </summary>
                public static class Action
                {
                    #region Constants

                    /// <summary>
                    /// Defines the MIN
                    /// </summary>
                    public const int MIN = 50;

                    /// <summary>
                    /// Defines the PLUS
                    /// </summary>
                    public const int PLUS = 51;

                    #endregion
                }
            }

            /// <summary>
            /// Defines the <see cref="ModalSwitch" />
            /// </summary>
            public static class ModalSwitch
            {
                /// <summary>
                /// Defines the <see cref="Action" />
                /// </summary>
                public static class Action
                {
                    #region Constants

                    /// <summary>
                    /// Defines the AUTO
                    /// </summary>
                    public const int AUTO = 60;

                    /// <summary>
                    /// Defines the ECONOMY
                    /// </summary>
                    public const int ECONOMY = 61;

                    /// <summary>
                    /// Defines the AWAY
                    /// </summary>
                    public const int AWAY = 62;

                    /// <summary>
                    /// Defines the DAY_OFF
                    /// </summary>
                    public const int DAY_OFF = 63;

                    /// <summary>
                    /// Defines the CUSTOM
                    /// </summary>
                    public const int CUSTOM = 64;

                    /// <summary>
                    /// Defines the HEATING_OFF
                    /// </summary>
                    public const int HEATING_OFF = 65;

                    #endregion
                }
            }

            /// <summary>
            /// Defines the <see cref="Type" />
            /// </summary>
            public static class Type
            {
                /// <summary>
                /// Defines the <see cref="Value" />
                /// </summary>
                public static class Value
                {
                    #region Constants

                    /// <summary>
                    /// Defines the DOORBELL
                    /// </summary>
                    public const int DOORBELL = 1;

                    /// <summary>
                    /// Defines the CONTACT
                    /// </summary>
                    public const int CONTACT = 2;

                    /// <summary>
                    /// Defines the BLINDS
                    /// </summary>
                    public const int BLINDS = 3;

                    /// <summary>
                    /// Defines the SMOKE_DETECTOR
                    /// </summary>
                    public const int SMOKE_DETECTOR = 5;

                    /// <summary>
                    /// Defines the DIMMER
                    /// </summary>
                    public const int DIMMER = 7;

                    /// <summary>
                    /// Defines the MOTION
                    /// </summary>
                    public const int MOTION = 8;

                    /// <summary>
                    /// Defines the PUSH_ON_BUTTON
                    /// </summary>
                    public const int PUSH_ON_BUTTON = 9;

                    /// <summary>
                    /// Defines the PUSH_OFF_BUTTON
                    /// </summary>
                    public const int PUSH_OFF_BUTTON = 10;

                    /// <summary>
                    /// Defines the ON_OFF
                    /// </summary>
                    public const int ON_OFF = 0;

                    /// <summary>
                    /// Defines the SECURITY
                    /// </summary>
                    public const int SECURITY = 0;

                    /// <summary>
                    /// Defines the X10SIREN
                    /// </summary>
                    public const int X10SIREN = 4;

                    /// <summary>
                    /// Defines the MEDIAPLAYER
                    /// </summary>
                    public const int MEDIAPLAYER = 17;

                    /// <summary>
                    /// Defines the DUSKSENSOR
                    /// </summary>
                    public const int DUSKSENSOR = 12;

                    /// <summary>
                    /// Defines the DOORCONTACT
                    /// </summary>
                    public const int DOORCONTACT = 11;

                    /// <summary>
                    /// Defines the BLINDPERCENTAGE
                    /// </summary>
                    public const int BLINDPERCENTAGE = 13;

                    /// <summary>
                    /// Defines the BLINDVENETIAN
                    /// </summary>
                    public const int BLINDVENETIAN = 15;

                    /// <summary>
                    /// Defines the BLINDVENETIANUS
                    /// </summary>
                    public const int BLINDVENETIANUS = 14;

                    /// <summary>
                    /// Defines the BLINDINVERTED
                    /// </summary>
                    public const int BLINDINVERTED = 6;

                    /// <summary>
                    /// Defines the BLINDPERCENTAGEINVERTED
                    /// </summary>
                    public const int BLINDPERCENTAGEINVERTED = 16;

                    /// <summary>
                    /// Defines the SELECTOR
                    /// </summary>
                    public const int SELECTOR = 18;

                    /// <summary>
                    /// Defines the DOORLOCK
                    /// </summary>
                    public const int DOORLOCK = 19;

                    /// <summary>
                    /// Defines the DOORLOCKINVERTED
                    /// </summary>
                    public const int DOORLOCKINVERTED = 20;

                    /// <summary>
                    /// Defines the TEMP
                    /// </summary>
                    public const int TEMP = 21;

                    #endregion
                }

                /// <summary>
                /// Defines the <see cref="Name" />
                /// </summary>
                public static class Name
                {
                    #region Constants

                    /// <summary>
                    /// Defines the DOORBELL
                    /// </summary>
                    public const string DOORBELL = "Doorbell";

                    /// <summary>
                    /// Defines the CONTACT
                    /// </summary>
                    public const string CONTACT = "Contact";

                    /// <summary>
                    /// Defines the BLINDS
                    /// </summary>
                    public const string BLINDS = "Blinds";

                    /// <summary>
                    /// Defines the SMOKE_DETECTOR
                    /// </summary>
                    public const string SMOKE_DETECTOR = "Smoke Detector";

                    /// <summary>
                    /// Defines the DIMMER
                    /// </summary>
                    public const string DIMMER = "Dimmer";

                    /// <summary>
                    /// Defines the MOTION
                    /// </summary>
                    public const string MOTION = "Motion Sensor";

                    /// <summary>
                    /// Defines the PUSH_ON_BUTTON
                    /// </summary>
                    public const string PUSH_ON_BUTTON = "Push On Button";

                    /// <summary>
                    /// Defines the PUSH_OFF_BUTTON
                    /// </summary>
                    public const string PUSH_OFF_BUTTON = "Push Off Button";

                    /// <summary>
                    /// Defines the ON_OFF
                    /// </summary>
                    public const string ON_OFF = "On/Off";

                    /// <summary>
                    /// Defines the SECURITY
                    /// </summary>
                    public const string SECURITY = "Security";

                    /// <summary>
                    /// Defines the X10SIREN
                    /// </summary>
                    public const string X10SIREN = "X10 Siren";

                    /// <summary>
                    /// Defines the MEDIAPLAYER
                    /// </summary>
                    public const string MEDIAPLAYER = "Media Player";

                    /// <summary>
                    /// Defines the DUSKSENSOR
                    /// </summary>
                    public const string DUSKSENSOR = "Dusk Sensor";

                    /// <summary>
                    /// Defines the DOORLOCK
                    /// </summary>
                    public const string DOORLOCK = "Door Lock";

                    /// <summary>
                    /// Defines the DOORCONTACT
                    /// </summary>
                    public const string DOORCONTACT = "Door Contact";

                    /// <summary>
                    /// Defines the BLINDPERCENTAGE
                    /// </summary>
                    public const string BLINDPERCENTAGE = "Blinds Percentage";

                    /// <summary>
                    /// Defines the BLINDVENETIAN
                    /// </summary>
                    public const string BLINDVENETIAN = "Venetian Blinds EU";

                    /// <summary>
                    /// Defines the BLINDVENETIANUS
                    /// </summary>
                    public const string BLINDVENETIANUS = "Venetian Blinds US";

                    /// <summary>
                    /// Defines the BLINDINVERTED
                    /// </summary>
                    public const string BLINDINVERTED = "Blinds Inverted";

                    /// <summary>
                    /// Defines the BLINDPERCENTAGEINVERTED
                    /// </summary>
                    public const string BLINDPERCENTAGEINVERTED = "Blinds Percentage Inverted";

                    /// <summary>
                    /// Defines the DOORLOCKINVERTED
                    /// </summary>
                    public const string DOORLOCKINVERTED = "Door Lock Inverted";

                    /// <summary>
                    /// Defines the TEMPHUMIDITYBARO
                    /// </summary>
                    public const string TEMPHUMIDITYBARO = "Temp + Humidity + Baro";

                    /// <summary>
                    /// Defines the WIND
                    /// </summary>
                    public const string WIND = "Wind";

                    /// <summary>
                    /// Defines the SELECTOR
                    /// </summary>
                    public const string SELECTOR = "Selector";

                    /// <summary>
                    /// Defines the EVOHOME
                    /// </summary>
                    public const string EVOHOME = "evohome";

                    #endregion
                }
            }

            /// <summary>
            /// Defines the <see cref="SubType" />
            /// </summary>
            public static class SubType
            {
                /// <summary>
                /// Defines the <see cref="Value" />
                /// </summary>
                public static class Value
                {
                    #region Constants

                    /// <summary>
                    /// Defines the RGB
                    /// </summary>
                    public const int RGB = 1;

                    /// <summary>
                    /// Defines the SECURITYPANEL
                    /// </summary>
                    public const int SECURITYPANEL = 2;

                    /// <summary>
                    /// Defines the EVOHOME
                    /// </summary>
                    public const int EVOHOME = 3;

                    #endregion
                }

                /// <summary>
                /// Defines the <see cref="Name" />
                /// </summary>
                public static class Name
                {
                    #region Constants

                    /// <summary>
                    /// Defines the RGB
                    /// </summary>
                    public const string RGB = "RGB";

                    /// <summary>
                    /// Defines the SECURITYPANEL
                    /// </summary>
                    public const string SECURITYPANEL = "Security Panel";

                    /// <summary>
                    /// Defines the EVOHOME
                    /// </summary>
                    public const string EVOHOME = "Evohome";

                    #endregion
                }
            }

            /// <summary>
            /// Defines the <see cref="Favorite" />
            /// </summary>
            public static class Favorite
            {
                #region Constants

                /// <summary>
                /// Defines the ON
                /// </summary>
                public const int ON = 208;

                /// <summary>
                /// Defines the OFF
                /// </summary>
                public const int OFF = 209;

                #endregion
            }
        }

        /// <summary>
        /// Defines the <see cref="Json" />
        /// </summary>
        public static class Json
        {
            /// <summary>
            /// Defines the <see cref="Field" />
            /// </summary>
            public static class Field
            {
                #region Constants

                /// <summary>
                /// Defines the RESULT
                /// </summary>
                public const string RESULT = "result";

                /// <summary>
                /// Defines the STATUS
                /// </summary>
                public const string STATUS = "status";

                /// <summary>
                /// Defines the VERSION
                /// </summary>
                public const string VERSION = "version";

                /// <summary>
                /// Defines the MESSAGE
                /// </summary>
                public const string MESSAGE = "message";

                /// <summary>
                /// Defines the ERROR
                /// </summary>
                public const string ERROR = "ERROR";

                #endregion
            }

            /// <summary>
            /// Defines the <see cref="Url" />
            /// </summary>
            public static class Url
            {
                /// <summary>
                /// Defines the <see cref="Request" />
                /// </summary>
                public static class Request
                {
                    #region Constants

                    /// <summary>
                    /// Defines the DASHBOARD
                    /// </summary>
                    public const int DASHBOARD = 1;

                    /// <summary>
                    /// Defines the SCENES
                    /// </summary>
                    public const int SCENES = 2;

                    /// <summary>
                    /// Defines the SWITCHES
                    /// </summary>
                    public const int SWITCHES = 3;

                    /// <summary>
                    /// Defines the UTILITIES
                    /// </summary>
                    public const int UTILITIES = 4;

                    /// <summary>
                    /// Defines the TEMPERATURE
                    /// </summary>
                    public const int TEMPERATURE = 5;

                    /// <summary>
                    /// Defines the WEATHER
                    /// </summary>
                    public const int WEATHER = 6;

                    /// <summary>
                    /// Defines the CAMERAS
                    /// </summary>
                    public const int CAMERAS = 7;

                    /// <summary>
                    /// Defines the CAMERA
                    /// </summary>
                    public const int CAMERA = 21;

                    /// <summary>
                    /// Defines the SUNRISE_SUNSET
                    /// </summary>
                    public const int SUNRISE_SUNSET = 8;

                    /// <summary>
                    /// Defines the VERSION
                    /// </summary>
                    public const int VERSION = 9;

                    /// <summary>
                    /// Defines the DEVICES
                    /// </summary>
                    public const int DEVICES = 10;

                    /// <summary>
                    /// Defines the PLANS
                    /// </summary>
                    public const int PLANS = 11;

                    /// <summary>
                    /// Defines the PLAN_DEVICES
                    /// </summary>
                    public const int PLAN_DEVICES = 12;

                    /// <summary>
                    /// Defines the LOG
                    /// </summary>
                    public const int LOG = 13;

                    /// <summary>
                    /// Defines the SWITCHLOG
                    /// </summary>
                    public const int SWITCHLOG = 14;

                    /// <summary>
                    /// Defines the SWITCHTIMER
                    /// </summary>
                    public const int SWITCHTIMER = 15;

                    /// <summary>
                    /// Defines the UPDATE
                    /// </summary>
                    public const int UPDATE = 16;

                    /// <summary>
                    /// Defines the USERVARIABLES
                    /// </summary>
                    public const int USERVARIABLES = 17;

                    /// <summary>
                    /// Defines the EVENTS
                    /// </summary>
                    public const int EVENTS = 18;

                    /// <summary>
                    /// Defines the GRAPH
                    /// </summary>
                    public const int GRAPH = 20;

                    /// <summary>
                    /// Defines the SETTINGS
                    /// </summary>
                    public const int SETTINGS = 22;

                    /// <summary>
                    /// Defines the SETSECURITY
                    /// </summary>
                    public const int SETSECURITY = 23;

                    /// <summary>
                    /// Defines the TEXTLOG
                    /// </summary>
                    public const int TEXTLOG = 24;

                    /// <summary>
                    /// Defines the CONFIG
                    /// </summary>
                    public const int CONFIG = 25;

                    /// <summary>
                    /// Defines the SET_DEVICE_USED
                    /// </summary>
                    public const int SET_DEVICE_USED = 26;

                    /// <summary>
                    /// Defines the UPDATE_DOWNLOAD_READY
                    /// </summary>
                    public const int UPDATE_DOWNLOAD_READY = 27;

                    /// <summary>
                    /// Defines the UPDATE_DOMOTICZ_SERVER
                    /// </summary>
                    public const int UPDATE_DOMOTICZ_SERVER = 28;

                    /// <summary>
                    /// Defines the ADD_MOBILE_DEVICE
                    /// </summary>
                    public const int ADD_MOBILE_DEVICE = 29;

                    /// <summary>
                    /// Defines the CLEAN_MOBILE_DEVICE
                    /// </summary>
                    public const int CLEAN_MOBILE_DEVICE = 30;

                    /// <summary>
                    /// Defines the NOTIFICATIONS
                    /// </summary>
                    public const int NOTIFICATIONS = 31;

                    /// <summary>
                    /// Defines the LANGUAGE
                    /// </summary>
                    public const int LANGUAGE = 32;

                    /// <summary>
                    /// Defines the SCENELOG
                    /// </summary>
                    public const int SCENELOG = 33;

                    /// <summary>
                    /// Defines the USERS
                    /// </summary>
                    public const int USERS = 34;

                    /// <summary>
                    /// Defines the LOGOFF
                    /// </summary>
                    public const int LOGOFF = 35;

                    /// <summary>
                    /// Defines the AUTH
                    /// </summary>
                    public const int AUTH = 36;

                    /// <summary>
                    /// Defines the FAVORITES
                    /// </summary>
                    public const int FAVORITES = 37;

                    /// <summary>
                    /// Defines the UPDATEVAR
                    /// </summary>
                    public const int UPDATEVAR = 40;

                    #endregion
                }

                /// <summary>
                /// Defines the <see cref="Set" />
                /// </summary>
                public static class Set
                {
                    #region Constants

                    /// <summary>
                    /// Defines the SCENES
                    /// </summary>
                    public const int SCENES = 101;

                    /// <summary>
                    /// Defines the SWITCHES
                    /// </summary>
                    public const int SWITCHES = 102;

                    /// <summary>
                    /// Defines the TEMP
                    /// </summary>
                    public const int TEMP = 103;

                    /// <summary>
                    /// Defines the FAVORITE
                    /// </summary>
                    public const int FAVORITE = 104;

                    /// <summary>
                    /// Defines the SCENEFAVORITE
                    /// </summary>
                    public const int SCENEFAVORITE = 106;

                    /// <summary>
                    /// Defines the EVENT
                    /// </summary>
                    public const int EVENT = 105;

                    /// <summary>
                    /// Defines the RGBCOLOR
                    /// </summary>
                    public const int RGBCOLOR = 107;

                    /// <summary>
                    /// Defines the MODAL_SWITCHES
                    /// </summary>
                    public const int MODAL_SWITCHES = 108;

                    /// <summary>
                    /// Defines the EVENTS_UPDATE_STATUS
                    /// </summary>
                    public const int EVENTS_UPDATE_STATUS = 109;

                    #endregion
                }
            }

            /// <summary>
            /// Defines the <see cref="Get" />
            /// </summary>
            public static class Get
            {
                #region Constants

                /// <summary>
                /// Defines the STATUS
                /// </summary>
                public const int STATUS = 301;

                #endregion
            }
        }

        /// <summary>
        /// Defines the <see cref="Favorite" />
        /// </summary>
        public static class Favorite
        {
            /// <summary>
            /// Defines the <see cref="Action" />
            /// </summary>
            public static class Action
            {
                #region Constants

                /// <summary>
                /// Defines the ON
                /// </summary>
                public const string ON = "1";

                /// <summary>
                /// Defines the OFF
                /// </summary>
                public const string OFF = "0";

                #endregion
            }
        }
    }
}
