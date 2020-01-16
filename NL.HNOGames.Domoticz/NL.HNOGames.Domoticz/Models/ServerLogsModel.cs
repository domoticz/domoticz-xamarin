namespace NL.HNOGames.Domoticz.Models
{
    /// <summary>
    /// Defines the <see cref="ServerLogsModel" />
    /// </summary>
    public class ServerLogsModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the LastLogTime
        /// </summary>
        public string LastLogTime { get; set; }

        /// <summary>
        /// Gets or sets the result
        /// </summary>
        public ServerLog[] result { get; set; }

        /// <summary>
        /// Gets or sets the status
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string title { get; set; }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="ServerLog" />
    /// </summary>
    public class ServerLog
    {
        #region Properties

        /// <summary>
        /// Gets the Date
        /// </summary>
        public string Date
        {
            get
            {
                if (message.Contains("  "))
                {
                    return message.Substring(0, message.IndexOf("  "));
                }
                return "";
            }
        }

        /// <summary>
        /// Gets the Data
        /// </summary>
        public string Data
        {
            get
            {
                if (message.Contains("  "))
                {
                    return message.Substring(message.IndexOf("  ") + 2);
                }
                return "";
            }
        }

        /// <summary>
        /// Gets or sets the level
        /// </summary>
        public int level { get; set; }

        /// <summary>
        /// Gets or sets the message
        /// </summary>
        public string message { get; set; }

        #endregion
    }
}
