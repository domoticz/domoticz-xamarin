using System;

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
                public const String ON = "On";
                public const String OFF = "Off";
                public const String UP = "Up";
                public const String STOP = "Stop";
                public const String DOWN = "Down";
                public const String PLUS = "Plus";
                public const String MIN = "Min";
            }

            public static class ModalAction
            {
                public const String AUTO = "Auto";
                public const String ECONOMY = "AutoWithEco";
                public const String AWAY = "Away";
                public const String DAY_OFF = "DayOff";
                public const String CUSTOM = "Custom";
                public const String HEATING_OFF = "HeatingOff";
            }

            public static class Category
            {
                public const String ALLDEVICES = "/json.htm?type=devices";
                public const String DEVICES = "/json.htm?type=devices&filter=all&used=true";
                public const String FAVORITES = "/json.htm?type=devices&filter=all&used=true&favorite=1";
                public const String VERSION = "/json.htm?type=command&param=getversion";
                public const String DASHBOARD = ALLDEVICES + "&filter=all";
                public const String SCENES = "/json.htm?type=scenes";
                public const String SWITCHES = "/json.htm?type=command&param=getlightswitches";
                public const String WEATHER = ALLDEVICES + "&filter=weather&used=true";
                public const String CAMERAS = "/json.htm?type=cameras";
                public const String CAMERA = "/camsnapshot.jpg?idx=";
                public const String UTILITIES = ALLDEVICES + "&filter=utility&used=true";
                public const String PLANS = "/json.htm?type=plans";
                public const String TEMPERATURE = ALLDEVICES + "&filter=temp&used=true";
                public const String SWITCHLOG = "/json.htm?type=lightlog&idx=";
                public const String TEXTLOG = "/json.htm?type=textlog&idx=";
                public const String SCENELOG = "/json.htm?type=scenelog&idx=";
                public const String SWITCHTIMER = "/json.htm?type=timers&idx=";
            }

            public static class Switch
            {
                public const String DIM_LEVEL = "Set%20Level&level=";
                public const String COLOR = "&hue=%hue%&brightness=%bright%&iswhite=false";
                public const String GET = "/json.htm?type=command&param=switchlight&idx=";
                public const String CMD = "&switchcmd=";
                public const String LEVEL = "&level=";
            }

            public static class ModalSwitch
            {
                public const String GET = "/json.htm?type=command&param=switchmodal&idx=";
                public const String STATUS = "&status=";
            }

            public static class Scene
            {
                public const String GET = "/json.htm?type=command&param=switchscene&idx=";
            }

            public static class Temp
            {
                public const String GET = "/json.htm?type=command&param=udevice&idx=";
                public const String VALUE = "&nvalue=0&svalue=";
            }

            public static class Favorite
            {
                public const String GET = "/json.htm?type=command&param=makefavorite&idx=";
                public const String SCENE = "/json.htm?type=command&param=makescenefavorite&idx=";
                public const String VALUE = "&isfavorite=";
            }

            public static class Protocol
            {
                public const String HTTP = "http://";
                public const String HTTPS = "https://";
            }

            public static class Device
            {
                public const String STATUS = "/json.htm?type=devices&rid=";
                public const String SET_USED = "/json.htm?type=setused&idx=";
            }

            public static class Sunrise
            {
                public const String GET = "/json.htm?type=command&param=getSunRiseSet";
            }

            public static class Plan
            {
                public const String GET = "/json.htm?type=plans";
                public const String DEVICES = "/json.htm?type=command&param=getplandevices&idx=";
            }

            public static class Log
            {
                public const String GRAPH = "/json.htm?type=graph&idx=";
                public const String GRAPH_RANGE = "&range=";
                public const String GRAPH_TYPE = "&sensor=";

                public const String GET_LOG = "/json.htm?type=command&param=getlog";
                public const String GET_FROMLASTLOGTIME = "/json.htm?type=command&param=getlog&lastlogtime=";
            }

            public static class Notification
            {
                public const String NOTIFICATION = "/json.htm?type=notifications&idx=";
            }

            public static class Security
            {
                public const String GET = "/json.htm?type=command&param=getsecstatus";
            }

            public static class UserVariable
            {
                public const String UPDATE = "/json.htm?type=command&param=updateuservariable";
            }

            public static class System
            {
                public const String UPDATE = "/json.htm?type=command&param=checkforupdate&forced=true";
                public const String USERVARIABLES = "/json.htm?type=command&param=getuservariables";
                public const String EVENTS = "/json.htm?type=events&param=list";
                public const String EVENTS_UPDATE_STATUS = "/json.htm?type=events&param=updatestatus&eventid=";
                public const String RGBCOLOR = "/json.htm?type=command&param=setcolbrightnessvalue&idx=";
                public const String SETTINGS = "/json.htm?type=settings";
                public const String CONFIG = "/json.htm?type=command&param=getconfig";
                public const String SETSECURITY = "/json.htm?type=command&param=setsecstatus";
                public const String DOWNLOAD_READY = "/json.htm?type=command&param=downloadready";
                public const String UPDATE_DOMOTICZ_SERVER = "/json.htm?type=command&param=execute_script&scriptname=update_domoticz&direct=true";
                public const String ADD_MOBILE_DEVICE = "/json.htm?type=command&param=addmobiledevice";
                public const String CLEAN_MOBILE_DEVICE = "/json.htm?type=command&param=deletemobiledevice";
                public const String LANGUAGE_TRANSLATIONS = "/i18n/domoticz-";
                public const String USERS = "/json.htm?type=users";
                public const String AUTH = "/json.htm?type=command&param=getauth";
                public const String LOGOFF = "/json.htm?type=command&param=dologout";
            }

            public static class Event
            {
                public const String ON = "&eventstatus=1";
                public const String OFF = "&eventstatus=0";
            }
        }

        public static class Event
        {
            public static class Type
            {
                public const String EVENT = "Event";
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
                    public const String GROUP = "Group";
                    public const String SCENE = "Scene";
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
                public const String EVOHOME = "evohome";
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
                    public const String HEATING = "Heating";
                    public const String THERMOSTAT = "Thermostat";
                }

                public static class SubType
                {
                    public const String TEXT = "Text";
                    public const String PERCENTAGE = "Percentage";
                    public const String ENERGY = "Energy";
                    public const String KWH = "kWh";
                    public const String GAS = "Gas";
                    public const String ELECTRIC = "Electric";
                    public const String VOLTCRAFT = "Voltcraft";
                    public const String SETPOINT = "SetPoint";
                    public const String YOULESS = "YouLess";
                    public const String SMARTWARES = "Smartwares";
                }
            }

            public static class Blind
            {
                public static class State
                {
                    public const String CLOSED = "Closed";
                    public const String OPEN = "Open";
                    public const String STOPPED = "Stopped";
                    public const String ON = "On";
                    public const String OFF = "Off";
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
                    public const String DOORBELL = "Doorbell";
                    public const String CONTACT = "Contact";
                    public const String BLINDS = "Blinds";
                    public const String SMOKE_DETECTOR = "Smoke Detector";
                    public const String DIMMER = "Dimmer";
                    public const String MOTION = "Motion Sensor";
                    public const String PUSH_ON_BUTTON = "Push On Button";
                    public const String PUSH_OFF_BUTTON = "Push Off Button";
                    public const String ON_OFF = "On/Off";
                    public const String SECURITY = "Security";
                    public const String X10SIREN = "X10 Siren";
                    public const String MEDIAPLAYER = "Media Player";
                    public const String DUSKSENSOR = "Dusk Sensor";
                    public const String DOORLOCK = "Door Lock";
                    public const String DOORCONTACT = "Door Contact";
                    public const String BLINDPERCENTAGE = "Blinds Percentage";
                    public const String BLINDVENETIAN = "Venetian Blinds EU";
                    public const String BLINDVENETIANUS = "Venetian Blinds US";
                    public const String BLINDINVERTED = "Blinds Inverted";
                    public const String BLINDPERCENTAGEINVERTED = "Blinds Percentage Inverted";
                    public const String TEMPHUMIDITYBARO = "Temp + Humidity + Baro";
                    public const String WIND = "Wind";
                    public const String SELECTOR = "Selector";
                    public const String EVOHOME = "evohome";
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
                    public const String RGB = "RGB";
                    public const String SECURITYPANEL = "Security Panel";
                    public const String EVOHOME = "Evohome";
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
                public const String RESULT = "result";
                public const String STATUS = "status";
                public const String VERSION = "version";
                public const String MESSAGE = "message";
                public const String ERROR = "ERROR";
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
                public const String ON = "1";
                public const String OFF = "0";
            }
        }
    }
}