using System;

namespace NL.HNOGames.Domoticz.Models
{
    /// <summary>
    /// Server settings object
    /// </summary>
    public class ServerSettings
    {
        #region Variables

        /// <summary>
        /// Defines the _SERVER_UNIQUE_ID
        /// </summary>
        private string _SERVER_UNIQUE_ID;

        /// <summary>
        /// Defines the _SERVER_NAME
        /// </summary>
        private string _SERVER_NAME = "Default";

        /// <summary>
        /// Defines the _REMOTE_SERVER_USERNAME
        /// </summary>
        private string _REMOTE_SERVER_USERNAME = "";

        /// <summary>
        /// Defines the _REMOTE_SERVER_PASSWORD
        /// </summary>
        private string _REMOTE_SERVER_PASSWORD = "";

        /// <summary>
        /// Defines the _REMOTE_SERVER_ADDRESS
        /// </summary>
        private string _REMOTE_SERVER_ADDRESS = "";

        /// <summary>
        /// Defines the _REMOTE_SERVER_PORT
        /// </summary>
        private string _REMOTE_SERVER_PORT = "";

        /// <summary>
        /// Defines the _REMOTE_SERVER_DIRECTORY
        /// </summary>
        private string _REMOTE_SERVER_DIRECTORY = "";

        /// <summary>
        /// Defines the _REMOTE_SERVER_SECURE
        /// </summary>
        private bool _REMOTE_SERVER_SECURE = true;

        /// <summary>
        /// Defines the _REMOTE_SERVER_AUTHENTICATION_METHOD
        /// </summary>
        private bool _REMOTE_SERVER_AUTHENTICATION_METHOD = false;

        /// <summary>
        /// Defines the _IS_LOCAL_SERVER_ADDRESS_DIFFERENT
        /// </summary>
        private bool _IS_LOCAL_SERVER_ADDRESS_DIFFERENT = false;

        /// <summary>
        /// Defines the _USE_ONLY_LOCAL
        /// </summary>
        private bool _USE_ONLY_LOCAL = false;

        /// <summary>
        /// Defines the _LOCAL_SERVER_USERNAME
        /// </summary>
        private string _LOCAL_SERVER_USERNAME = "";

        /// <summary>
        /// Defines the _LOCAL_SERVER_PASSWORD
        /// </summary>
        private string _LOCAL_SERVER_PASSWORD = "";

        /// <summary>
        /// Defines the _LOCAL_SERVER_ADDRESS
        /// </summary>
        private string _LOCAL_SERVER_ADDRESS = "";

        /// <summary>
        /// Defines the _LOCAL_SERVER_PORT
        /// </summary>
        private string _LOCAL_SERVER_PORT = "";

        /// <summary>
        /// Defines the _LOCAL_SERVER_DIRECTORY
        /// </summary>
        private string _LOCAL_SERVER_DIRECTORY = "";

        /// <summary>
        /// Defines the _LOCAL_SERVER_SECURE
        /// </summary>
        private bool _LOCAL_SERVER_SECURE = false;

        /// <summary>
        /// Defines the _ENABLED
        /// </summary>
        private bool _ENABLED = true;

        /// <summary>
        /// Defines the _LOCAL_SERVER_AUTHENTICATION_METHOD
        /// </summary>
        private bool _LOCAL_SERVER_AUTHENTICATION_METHOD = false;

        /// <summary>
        /// Defines the _LOCAL_SERVER_SSID
        /// </summary>
        private string _LOCAL_SERVER_SSID;

        /// <summary>
        /// Defines the _REMOTE_SERVER_PROTOCOL
        /// </summary>
        private int _REMOTE_SERVER_PROTOCOL = 0;

        /// <summary>
        /// Defines the _LOCAL_SERVER_PROTOCOL
        /// </summary>
        private int _LOCAL_SERVER_PROTOCOL = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the SERVER_UNIQUE_ID
        /// </summary>
        public string SERVER_UNIQUE_ID { get => _SERVER_UNIQUE_ID; set => _SERVER_UNIQUE_ID = value; }

        /// <summary>
        /// Gets or sets the SERVER_NAME
        /// </summary>
        public string SERVER_NAME { get => _SERVER_NAME; set => _SERVER_NAME = value; }

        /// <summary>
        /// Gets or sets the REMOTE_SERVER_USERNAME
        /// </summary>
        public string REMOTE_SERVER_USERNAME { get => _REMOTE_SERVER_USERNAME; set => _REMOTE_SERVER_USERNAME = value.Trim(); }

        /// <summary>
        /// Gets or sets the REMOTE_SERVER_PASSWORD
        /// </summary>
        public string REMOTE_SERVER_PASSWORD { get => _REMOTE_SERVER_PASSWORD; set => _REMOTE_SERVER_PASSWORD = value; }

        /// <summary>
        /// Gets or sets the REMOTE_SERVER_URL
        /// </summary>
        public string REMOTE_SERVER_URL { get => _REMOTE_SERVER_ADDRESS; set => _REMOTE_SERVER_ADDRESS = value; }

        /// <summary>
        /// Gets or sets the REMOTE_SERVER_PORT
        /// </summary>
        public string REMOTE_SERVER_PORT { get => _REMOTE_SERVER_PORT; set => _REMOTE_SERVER_PORT = value; }

        /// <summary>
        /// Gets or sets the REMOTE_SERVER_DIRECTORY
        /// </summary>
        public string REMOTE_SERVER_DIRECTORY { get => _REMOTE_SERVER_DIRECTORY; set => _REMOTE_SERVER_DIRECTORY = value; }

        /// <summary>
        /// Gets or sets a value indicating whether REMOTE_SERVER_SECURE
        /// </summary>
        public bool REMOTE_SERVER_SECURE { get => _REMOTE_SERVER_SECURE; set => _REMOTE_SERVER_SECURE = value; }

        /// <summary>
        /// Gets or sets a value indicating whether REMOTE_SERVER_AUTHENTICATION_METHOD
        /// </summary>
        public bool REMOTE_SERVER_AUTHENTICATION_METHOD { get => _REMOTE_SERVER_AUTHENTICATION_METHOD; set => _REMOTE_SERVER_AUTHENTICATION_METHOD = value; }

        /// <summary>
        /// Gets or sets a value indicating whether IS_LOCAL_SERVER_ADDRESS_DIFFERENT
        /// </summary>
        public bool IS_LOCAL_SERVER_ADDRESS_DIFFERENT { get => _IS_LOCAL_SERVER_ADDRESS_DIFFERENT; set => _IS_LOCAL_SERVER_ADDRESS_DIFFERENT = value; }

        /// <summary>
        /// Gets or sets a value indicating whether USE_ONLY_LOCAL
        /// </summary>
        public bool USE_ONLY_LOCAL { get => _USE_ONLY_LOCAL; set => _USE_ONLY_LOCAL = value; }

        /// <summary>
        /// Gets or sets the LOCAL_SERVER_USERNAME
        /// </summary>
        public string LOCAL_SERVER_USERNAME { get => _LOCAL_SERVER_USERNAME; set => _LOCAL_SERVER_USERNAME = value.Trim(); }

        /// <summary>
        /// Gets or sets the LOCAL_SERVER_PASSWORD
        /// </summary>
        public string LOCAL_SERVER_PASSWORD { get => _LOCAL_SERVER_PASSWORD; set => _LOCAL_SERVER_PASSWORD = value; }

        /// <summary>
        /// Gets or sets the LOCAL_SERVER_URL
        /// </summary>
        public string LOCAL_SERVER_URL { get => _LOCAL_SERVER_ADDRESS; set => _LOCAL_SERVER_ADDRESS = value; }

        /// <summary>
        /// Gets or sets the LOCAL_SERVER_PORT
        /// </summary>
        public string LOCAL_SERVER_PORT { get => _LOCAL_SERVER_PORT; set => _LOCAL_SERVER_PORT = value; }

        /// <summary>
        /// Gets or sets the LOCAL_SERVER_DIRECTORY
        /// </summary>
        public string LOCAL_SERVER_DIRECTORY { get => _LOCAL_SERVER_DIRECTORY; set => _LOCAL_SERVER_DIRECTORY = value; }

        /// <summary>
        /// Gets or sets a value indicating whether LOCAL_SERVER_SECURE
        /// </summary>
        public bool LOCAL_SERVER_SECURE { get => _LOCAL_SERVER_SECURE; set => _LOCAL_SERVER_SECURE = value; }

        /// <summary>
        /// Gets or sets a value indicating whether ENABLED
        /// </summary>
        public bool ENABLED { get => _ENABLED; set => _ENABLED = value; }

        /// <summary>
        /// Gets or sets a value indicating whether LOCAL_SERVER_AUTHENTICATION_METHOD
        /// </summary>
        public bool LOCAL_SERVER_AUTHENTICATION_METHOD { get => _LOCAL_SERVER_AUTHENTICATION_METHOD; set => _LOCAL_SERVER_AUTHENTICATION_METHOD = value; }

        /// <summary>
        /// Gets or sets the LOCAL_SERVER_SSID
        /// </summary>
        public String LOCAL_SERVER_SSID { get => _LOCAL_SERVER_SSID; set => _LOCAL_SERVER_SSID = value; }

        /// <summary>
        /// Gets or sets the REMOTE_SERVER_PROTOCOL
        /// </summary>
        public int REMOTE_SERVER_PROTOCOL { get => _REMOTE_SERVER_PROTOCOL; set => _REMOTE_SERVER_PROTOCOL = value; }

        /// <summary>
        /// Gets or sets the LOCAL_SERVER_PROTOCOL
        /// </summary>
        public int LOCAL_SERVER_PROTOCOL { get => _LOCAL_SERVER_PROTOCOL; set => _LOCAL_SERVER_PROTOCOL = value; }

        #endregion
    }
}
