using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Models
{
    /// <summary>
    /// Server settings object
    /// </summary>
    public class ServerSettings
    {
        private string _SERVER_UNIQUE_ID;
        private string _SERVER_NAME = "Default";
        private string _REMOTE_SERVER_USERNAME = "";
        private string _REMOTE_SERVER_PASSWORD = "";
        private string _REMOTE_SERVER_ADDRESS = "";
        private string _REMOTE_SERVER_PORT = "";
        private string _REMOTE_SERVER_DIRECTORY = "";
        private bool _REMOTE_SERVER_SECURE = true;
        private bool _REMOTE_SERVER_AUTHENTICATION_METHOD = false;
        private bool _IS_LOCAL_SERVER_ADDRESS_DIFFERENT = false;
        private bool _USE_ONLY_LOCAL = false;
        private string _LOCAL_SERVER_USERNAME = "";
        private string _LOCAL_SERVER_PASSWORD = "";
        private string _LOCAL_SERVER_ADDRESS = "";
        private string _LOCAL_SERVER_PORT = "";
        private string _LOCAL_SERVER_DIRECTORY = "";
        private bool _LOCAL_SERVER_SECURE = false;
        private bool _ENABLED = true;
        private bool _LOCAL_SERVER_AUTHENTICATION_METHOD = false;
        private string _LOCAL_SERVER_SSID;
        private int _REMOTE_SERVER_PROTOCOL = 0;
        private int _LOCAL_SERVER_PROTOCOL = 0;

        public string SERVER_UNIQUE_ID { get => _SERVER_UNIQUE_ID; set => _SERVER_UNIQUE_ID = value; }
        public string SERVER_NAME { get => _SERVER_NAME; set => _SERVER_NAME = value; }
        public string REMOTE_SERVER_USERNAME { get => _REMOTE_SERVER_USERNAME; set => _REMOTE_SERVER_USERNAME = value.Trim(); }
        public string REMOTE_SERVER_PASSWORD { get => _REMOTE_SERVER_PASSWORD; set => _REMOTE_SERVER_PASSWORD = value; }
        public string REMOTE_SERVER_URL { get => _REMOTE_SERVER_ADDRESS; set => _REMOTE_SERVER_ADDRESS = value; }
        public string REMOTE_SERVER_PORT { get => _REMOTE_SERVER_PORT; set => _REMOTE_SERVER_PORT = value; }
        public string REMOTE_SERVER_DIRECTORY { get => _REMOTE_SERVER_DIRECTORY; set => _REMOTE_SERVER_DIRECTORY = value; }
        public bool REMOTE_SERVER_SECURE { get => _REMOTE_SERVER_SECURE; set => _REMOTE_SERVER_SECURE = value; }
        public bool REMOTE_SERVER_AUTHENTICATION_METHOD { get => _REMOTE_SERVER_AUTHENTICATION_METHOD; set => _REMOTE_SERVER_AUTHENTICATION_METHOD = value; }
        public bool IS_LOCAL_SERVER_ADDRESS_DIFFERENT { get => _IS_LOCAL_SERVER_ADDRESS_DIFFERENT; set => _IS_LOCAL_SERVER_ADDRESS_DIFFERENT = value; }
        public bool USE_ONLY_LOCAL { get => _USE_ONLY_LOCAL; set => _USE_ONLY_LOCAL = value; }
        public string LOCAL_SERVER_USERNAME { get => _LOCAL_SERVER_USERNAME; set => _LOCAL_SERVER_USERNAME = value.Trim(); }
        public string LOCAL_SERVER_PASSWORD { get => _LOCAL_SERVER_PASSWORD; set => _LOCAL_SERVER_PASSWORD = value; }
        public string LOCAL_SERVER_URL { get => _LOCAL_SERVER_ADDRESS; set => _LOCAL_SERVER_ADDRESS = value; }
        public string LOCAL_SERVER_PORT { get => _LOCAL_SERVER_PORT; set => _LOCAL_SERVER_PORT = value; }
        public string LOCAL_SERVER_DIRECTORY { get => _LOCAL_SERVER_DIRECTORY; set => _LOCAL_SERVER_DIRECTORY = value; }
        public bool LOCAL_SERVER_SECURE { get => _LOCAL_SERVER_SECURE; set => _LOCAL_SERVER_SECURE = value; }
        public bool ENABLED { get => _ENABLED; set => _ENABLED = value; }
        public bool LOCAL_SERVER_AUTHENTICATION_METHOD { get => _LOCAL_SERVER_AUTHENTICATION_METHOD; set => _LOCAL_SERVER_AUTHENTICATION_METHOD = value; }
        public String LOCAL_SERVER_SSID { get => _LOCAL_SERVER_SSID; set => _LOCAL_SERVER_SSID = value; }
        public int REMOTE_SERVER_PROTOCOL { get => _REMOTE_SERVER_PROTOCOL; set => _REMOTE_SERVER_PROTOCOL = value; }
        public int LOCAL_SERVER_PROTOCOL { get => _LOCAL_SERVER_PROTOCOL; set => _LOCAL_SERVER_PROTOCOL = value; }
    }
}
