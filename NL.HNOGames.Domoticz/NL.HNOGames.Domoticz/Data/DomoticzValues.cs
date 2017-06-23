using System;

namespace NL.HNOGames.Domoticz.Data
{
    public static class DomoticzValues
    {
        public static class Url
        {
            public static class Action
            {
                public static String ON = "On";
                public static String OFF = "Off";
                public static String UP = "Up";
                public static String STOP = "Stop";
                public static String DOWN = "Down";
                public static String PLUS = "Plus";
                public static String MIN = "Min";
            }

            public static class ModalAction
            {
                public static String AUTO = "Auto";
                public static String ECONOMY = "AutoWithEco";
                public static String AWAY = "Away";
                public static String DAY_OFF = "DayOff";
                public static String CUSTOM = "Custom";
                public static String HEATING_OFF = "HeatingOff";
            }

            public static class Category
            {
                public static String ALLDEVICES = "/json.htm?type=devices";
                public static String DEVICES = "/json.htm?type=devices&filter=all&used=true";
                public static String FAVORITES = "/json.htm?type=devices&filter=all&used=true&favorite=1";
                public static String VERSION = "/json.htm?type=command&param=getversion";
                public static String DASHBOARD = ALLDEVICES + "&filter=all";
                public static String SCENES = "/json.htm?type=scenes";
                public static String SWITCHES = "/json.htm?type=command&param=getlightswitches";
                public static String WEATHER = ALLDEVICES + "&filter=weather&used=true";
                public static String CAMERAS = "/json.htm?type=cameras";
                public static String CAMERA = "/camsnapshot.jpg?idx=";
                public static String UTILITIES = ALLDEVICES + "&filter=utility&used=true";
                public static String PLANS = "/json.htm?type=plans";
                public static String TEMPERATURE = ALLDEVICES + "&filter=temp&used=true";
                public static String SWITCHLOG = "/json.htm?type=lightlog&idx=";
                public static String TEXTLOG = "/json.htm?type=textlog&idx=";
                public static String SCENELOG = "/json.htm?type=scenelog&idx=";
                public static String SWITCHTIMER = "/json.htm?type=timers&idx=";
            }

            public static class Switch
            {
                public static String DIM_LEVEL = "Set%20Level&level=";
                public static String COLOR = "&hue=%hue%&brightness=%bright%&iswhite=false";
                public static String GET = "/json.htm?type=command&param=switchlight&idx=";
                public static String CMD = "&switchcmd=";
                public static String LEVEL = "&level=";
            }

            public static class ModalSwitch
            {
                public static String GET = "/json.htm?type=command&param=switchmodal&idx=";
                public static String STATUS = "&status=";
            }

            public static class Scene
            {
                public static String GET = "/json.htm?type=command&param=switchscene&idx=";
            }

            public static class Temp
            {
                public static String GET = "/json.htm?type=command&param=udevice&idx=";
                public static String VALUE = "&nvalue=0&svalue=";
            }

            public static class Favorite
            {
                public static String GET = "/json.htm?type=command&param=makefavorite&idx=";
                public static String SCENE = "/json.htm?type=command&param=makescenefavorite&idx=";
                public static String VALUE = "&isfavorite=";
            }

            public static class Protocol
            {
                public static String HTTP = "http://";
                public static String HTTPS = "https://";
            }

            public static class Device
            {
                public static String STATUS = "/json.htm?type=devices&rid=";
                public static String SET_USED = "/json.htm?type=setused&idx=";
            }

            public static class Sunrise
            {
                public static String GET = "/json.htm?type=command&param=getSunRiseSet";
            }

            public static class Plan
            {
                public static String GET = "/json.htm?type=plans";
                public static String DEVICES = "/json.htm?type=command&param=getplandevices&idx=";
            }

            public static class Log
            {
                public static String GRAPH = "/json.htm?type=graph&idx=";
                public static String GRAPH_RANGE = "&range=";
                public static String GRAPH_TYPE = "&sensor=";

                public static String GET_LOG = "/json.htm?type=command&param=getlog";
                public static String GET_FROMLASTLOGTIME = "/json.htm?type=command&param=getlog&lastlogtime=";
            }

            public static class Notification
            {
                public static String NOTIFICATION = "/json.htm?type=notifications&idx=";
            }

            public static class Security
            {
                public static String GET = "/json.htm?type=command&param=getsecstatus";
            }

            public static class UserVariable
            {
                public static String UPDATE = "/json.htm?type=command&param=updateuservariable";
            }

            public static class System
            {
                public static String UPDATE = "/json.htm?type=command&param=checkforupdate&forced=true";
                public static String USERVARIABLES = "/json.htm?type=command&param=getuservariables";
                public static String EVENTS = "/json.htm?type=events&param=list";
                public static String EVENTS_UPDATE_STATUS = "/json.htm?type=events&param=updatestatus&eventid=";
                public static String RGBCOLOR = "/json.htm?type=command&param=setcolbrightnessvalue&idx=";
                public static String SETTINGS = "/json.htm?type=settings";
                public static String CONFIG = "/json.htm?type=command&param=getconfig";
                public static String SETSECURITY = "/json.htm?type=command&param=setsecstatus";
                public static String DOWNLOAD_READY = "/json.htm?type=command&param=downloadready";
                public static String UPDATE_DOMOTICZ_SERVER = "/json.htm?type=command&param=execute_script&scriptname=update_domoticz&direct=true";
                public static String ADD_MOBILE_DEVICE = "/json.htm?type=command&param=addmobiledevice";
                public static String CLEAN_MOBILE_DEVICE = "/json.htm?type=command&param=deletemobiledevice";
                public static String LANGUAGE_TRANSLATIONS = "/i18n/domoticz-";
                public static String USERS = "/json.htm?type=users";
                public static String AUTH = "/json.htm?type=command&param=getauth";
                public static String LOGOFF = "/json.htm?type=command&param=dologout";
            }

            public static class Event
            {
                public static String ON = "&eventstatus=1";
                public static String OFF = "&eventstatus=0";
            }
        }
    }
}