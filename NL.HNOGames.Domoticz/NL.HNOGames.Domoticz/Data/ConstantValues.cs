using System;
using System.Collections.Generic;
using System.Linq;

namespace NL.HNOGames.Domoticz.Data
{
    public static class ConstantValues
    {
        public enum GraphRange
        {
            Day,
            Month,
            Year
        }

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

        public static class Url
        {
            public static class Action
            {
                public const string ON = "On";
                public const string OFF = "Off";
                public const string UP = "Up";
                public const string STOP = "Stop";
                public const string DOWN = "Down";
                public const string PLUS = "Plus";
                public const string MIN = "Min";
            }

            public static class ModalAction
            {
                public const string AUTO = "Auto";
                public const string ECONOMY = "AutoWithEco";
                public const string AWAY = "Away";
                public const string DAY_OFF = "DayOff";
                public const string CUSTOM = "Custom";
                public const string HEATING_OFF = "HeatingOff";
            }

            public static class Category
            {
                public const string ALLDEVICES = "/json.htm?type=devices";
                public const string DEVICES = "/json.htm?type=devices&filter=all&used=true";
                public const string FAVORITES = "/json.htm?type=devices&filter=all&used=true&favorite=1";
                public const string VERSION = "/json.htm?type=command&param=getversion";
                public const string DASHBOARD = ALLDEVICES + "&filter=all";
                public const string SCENES = "/json.htm?type=scenes";
                public const string SWITCHES = "/json.htm?type=command&param=getlightswitches";
                public const string WEATHER = ALLDEVICES + "&filter=weather&used=true";
                public const string CAMERAS = "/json.htm?type=cameras";
                public const string CAMERA = "/camsnapshot.jpg?idx=";
                public const string UTILITIES = ALLDEVICES + "&filter=utility&used=true";
                public const string PLANS = "/json.htm?type=plans";
                public const string TEMPERATURE = ALLDEVICES + "&filter=temp&used=true";
                public const string SWITCHLOG = "/json.htm?type=lightlog&idx=";
                public const string TEXTLOG = "/json.htm?type=textlog&idx=";
                public const string SCENELOG = "/json.htm?type=scenelog&idx=";
                public const string SWITCHTIMER = "/json.htm?type=timers&idx=";
            }

            public static class Switch
            {
                public const string DIM_LEVEL = "Set%20Level&level=";
                public const string COLOR = "&hue=%hue%&brightness=%bright%&iswhite=false";
                public const string GET = "/json.htm?type=command&param=switchlight&idx=";
                public const string CMD = "&switchcmd=";
                public const string LEVEL = "&level=";
            }

            public static class ModalSwitch
            {
                public const string GET = "/json.htm?type=command&param=switchmodal&idx=";
                public const string STATUS = "&status=";
            }

            public static class Scene
            {
                public const string GET = "/json.htm?type=command&param=switchscene&idx=";
            }

            public static class Temp
            {
                public const string GET = "/json.htm?type=command&param=udevice&idx=";
                public const string VALUE = "&nvalue=0&svalue=";
            }

            public static class Favorite
            {
                public const string GET = "/json.htm?type=command&param=makefavorite&idx=";
                public const string SCENE = "/json.htm?type=command&param=makescenefavorite&idx=";
                public const string VALUE = "&isfavorite=";
            }

            public static class Protocol
            {
                public const string HTTP = "http://";
                public const string HTTPS = "https://";
            }

            public static class Device
            {
                public const string STATUS = "/json.htm?type=devices&rid=";
                public const string SET_USED = "/json.htm?type=setused&idx=";
            }

            public static class Sunrise
            {
                public const string GET = "/json.htm?type=command&param=getSunRiseSet";
            }

            public static class Plan
            {
                public const string GET = "/json.htm?type=plans";
                public const string DEVICES = "/json.htm?type=command&param=getplandevices&idx=";
            }

            public static class Log
            {
                public const string GRAPH = "/json.htm?type=graph&idx=";
                public const string GRAPH_RANGE = "&range=";
                public const string GRAPH_TYPE = "&sensor=";

                public const string GET_LOG = "/json.htm?type=command&param=getlog";
                public const string GET_FROMLASTLOGTIME = "/json.htm?type=command&param=getlog&lastlogtime=";
            }

            public static class Notification
            {
                public const string NOTIFICATION = "/json.htm?type=notifications&idx=";
            }

            public static class Security
            {
                public const string GET = "/json.htm?type=command&param=getsecstatus";
            }

            public static class UserVariable
            {
                public const string UPDATE = "/json.htm?type=command&param=updateuservariable";
            }

            public static class System
            {
                public const string UPDATE = "/json.htm?type=command&param=checkforupdate&forced=true";
                public const string USERVARIABLES = "/json.htm?type=command&param=getuservariables";
                public const string EVENTS = "/json.htm?type=events&param=list";
                public const string EVENTS_UPDATE_STATUS = "/json.htm?type=events&param=updatestatus&eventid=";
                public const string RGBCOLOR = "/json.htm?type=command&param=setcolbrightnessvalue&idx=";
                public const string SETTINGS = "/json.htm?type=settings";
                public const string CONFIG = "/json.htm?type=command&param=getconfig";
                public const string SETSECURITY = "/json.htm?type=command&param=setsecstatus";
                public const string DOWNLOAD_READY = "/json.htm?type=command&param=downloadready";

                public const string UPDATE_DOMOTICZ_SERVER =
                    "/json.htm?type=command&param=execute_script&scriptname=update_domoticz&direct=true";

                public const string ADD_MOBILE_DEVICE = "/json.htm?type=command&param=addmobiledevice";
                public const string CLEAN_MOBILE_DEVICE = "/json.htm?type=command&param=deletemobiledevice";
                public const string LANGUAGE_TRANSLATIONS = "/i18n/domoticz-";
                public const string USERS = "/json.htm?type=users";
                public const string AUTH = "/json.htm?type=command&param=getauth";
                public const string LOGOFF = "/json.htm?type=command&param=dologout";
            }

            public static class Event
            {
                public const string ON = "&eventstatus=1";
                public const string OFF = "&eventstatus=0";
            }
        }

        public static class Event
        {
            public static class Type
            {
                public const string EVENT = "Event";
            }

            public static class Action
            {
                public const int ON = 55;
                public const int OFF = 56;
            }
        }

        public static class Security
        {
            public static class Status
            {
                public const int ARMHOME = 1;
                public const int ARMAWAY = 2;
                public const int DISARM = 0;
            }
        }

        public static class Device
        {
            public static class Scene
            {
                public static class Type
                {
                    public const string GROUP = "Group";
                    public const string SCENE = "Scene";
                }

                public static class Action
                {
                    public const int ON = 40;
                    public const int OFF = 41;
                }
            }


            public static class Switch
            {
                public static class Action
                {
                    public const int ON = 10;
                    public const int OFF = 11;
                }
            }

            public static class Hardware
            {
                public const string EVOHOME = "evohome";
            }

            public static class Dimmer
            {
                public static class Action
                {
                    public const int DIM_LEVEL = 20;
                    public const int COLOR = 21;
                }
            }

            public static class Utility
            {
                public static class Type
                {
                    public const string HEATING = "Heating";
                    public const string THERMOSTAT = "Thermostat";
                }

                public static class SubType
                {
                    public const string TEXT = "Text";
                    public const string PERCENTAGE = "Percentage";
                    public const string ENERGY = "Energy";
                    public const string KWH = "kWh";
                    public const string GAS = "Gas";
                    public const string ELECTRIC = "Electric";
                    public const string VOLTCRAFT = "Voltcraft";
                    public const string SETPOINT = "SetPoint";
                    public const string YOULESS = "YouLess";
                    public const string SMARTWARES = "Smartwares";
                }
            }

            public static class Blind
            {
                public static class State
                {
                    public const string CLOSED = "Closed";
                    public const string OPEN = "Open";
                    public const string STOPPED = "Stopped";
                    public const string ON = "On";
                    public const string OFF = "Off";
                }

                public static class Action
                {
                    public const int UP = 30;
                    public const int STOP = 31;
                    public const int ON = 33;
                    public const int OFF = 34;
                    public const int DOWN = 32;
                }
            }

            public static class Thermostat
            {
                public static class Action
                {
                    public const int MIN = 50;
                    public const int PLUS = 51;
                }
            }

            public static class ModalSwitch
            {
                public static class Action
                {
                    public const int AUTO = 60;
                    public const int ECONOMY = 61;
                    public const int AWAY = 62;
                    public const int DAY_OFF = 63;
                    public const int CUSTOM = 64;
                    public const int HEATING_OFF = 65;
                }
            }

            public static class Type
            {
                public static class Value
                {
                    public const int DOORBELL = 1;
                    public const int CONTACT = 2;
                    public const int BLINDS = 3;
                    public const int SMOKE_DETECTOR = 5;
                    public const int DIMMER = 7;
                    public const int MOTION = 8;
                    public const int PUSH_ON_BUTTON = 9;
                    public const int PUSH_OFF_BUTTON = 10;
                    public const int ON_OFF = 0;
                    public const int SECURITY = 0;
                    public const int X10SIREN = 4;
                    public const int MEDIAPLAYER = 17;
                    public const int DUSKSENSOR = 12;
                    public const int DOORCONTACT = 11;
                    public const int BLINDPERCENTAGE = 13;
                    public const int BLINDVENETIAN = 15;
                    public const int BLINDVENETIANUS = 14;
                    public const int BLINDINVERTED = 6;
                    public const int BLINDPERCENTAGEINVERTED = 16;
                    public const int SELECTOR = 18;
                    public const int DOORLOCK = 19;
                }

                public static class Name
                {
                    public const string DOORBELL = "Doorbell";
                    public const string CONTACT = "Contact";
                    public const string BLINDS = "Blinds";
                    public const string SMOKE_DETECTOR = "Smoke Detector";
                    public const string DIMMER = "Dimmer";
                    public const string MOTION = "Motion Sensor";
                    public const string PUSH_ON_BUTTON = "Push On Button";
                    public const string PUSH_OFF_BUTTON = "Push Off Button";
                    public const string ON_OFF = "On/Off";
                    public const string SECURITY = "Security";
                    public const string X10SIREN = "X10 Siren";
                    public const string MEDIAPLAYER = "Media Player";
                    public const string DUSKSENSOR = "Dusk Sensor";
                    public const string DOORLOCK = "Door Lock";
                    public const string DOORCONTACT = "Door Contact";
                    public const string BLINDPERCENTAGE = "Blinds Percentage";
                    public const string BLINDVENETIAN = "Venetian Blinds EU";
                    public const string BLINDVENETIANUS = "Venetian Blinds US";
                    public const string BLINDINVERTED = "Blinds Inverted";
                    public const string BLINDPERCENTAGEINVERTED = "Blinds Percentage Inverted";
                    public const string TEMPHUMIDITYBARO = "Temp + Humidity + Baro";
                    public const string WIND = "Wind";
                    public const string SELECTOR = "Selector";
                    public const string EVOHOME = "evohome";
                }
            }

            public static class SubType
            {
                public static class Value
                {
                    public const int RGB = 1;
                    public const int SECURITYPANEL = 2;
                    public const int EVOHOME = 3;
                }

                public static class Name
                {
                    public const string RGB = "RGB";
                    public const string SECURITYPANEL = "Security Panel";
                    public const string EVOHOME = "Evohome";
                }
            }

            public static class Favorite
            {
                public const int ON = 208;
                public const int OFF = 209;
            }
        }

        public static class Json
        {
            public static class Field
            {
                public const string RESULT = "result";
                public const string STATUS = "status";
                public const string VERSION = "version";
                public const string MESSAGE = "message";
                public const string ERROR = "ERROR";
            }

            public static class Url
            {
                public static class Request
                {
                    public const int DASHBOARD = 1;
                    public const int SCENES = 2;
                    public const int SWITCHES = 3;
                    public const int UTILITIES = 4;
                    public const int TEMPERATURE = 5;
                    public const int WEATHER = 6;
                    public const int CAMERAS = 7;
                    public const int CAMERA = 21;
                    public const int SUNRISE_SUNSET = 8;
                    public const int VERSION = 9;
                    public const int DEVICES = 10;
                    public const int PLANS = 11;
                    public const int PLAN_DEVICES = 12;
                    public const int LOG = 13;
                    public const int SWITCHLOG = 14;
                    public const int SWITCHTIMER = 15;
                    public const int UPDATE = 16;
                    public const int USERVARIABLES = 17;
                    public const int EVENTS = 18;
                    public const int GRAPH = 20;
                    public const int SETTINGS = 22;
                    public const int SETSECURITY = 23;
                    public const int TEXTLOG = 24;
                    public const int CONFIG = 25;
                    public const int SET_DEVICE_USED = 26;
                    public const int UPDATE_DOWNLOAD_READY = 27;
                    public const int UPDATE_DOMOTICZ_SERVER = 28;
                    public const int ADD_MOBILE_DEVICE = 29;
                    public const int CLEAN_MOBILE_DEVICE = 30;
                    public const int NOTIFICATIONS = 31;
                    public const int LANGUAGE = 32;
                    public const int SCENELOG = 33;
                    public const int USERS = 34;
                    public const int LOGOFF = 35;
                    public const int AUTH = 36;
                    public const int FAVORITES = 37;
                    public const int UPDATEVAR = 40;
                }

                public static class Set
                {
                    public const int SCENES = 101;
                    public const int SWITCHES = 102;
                    public const int TEMP = 103;
                    public const int FAVORITE = 104;
                    public const int SCENEFAVORITE = 106;
                    public const int EVENT = 105;
                    public const int RGBCOLOR = 107;
                    public const int MODAL_SWITCHES = 108;
                    public const int EVENTS_UPDATE_STATUS = 109;
                }
            }

            public static class Get
            {
                public const int STATUS = 301;
            }
        }

        public static class Favorite
        {
            public static class Action
            {
                public const string ON = "1";
                public const string OFF = "0";
            }
        }
    }
}