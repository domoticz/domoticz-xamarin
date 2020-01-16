namespace NL.HNOGames.Domoticz.Models
{
    /// <summary>
    /// Defines the <see cref="LogModel" />
    /// </summary>
    public class LogModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether HaveDimmer
        /// </summary>
        public bool HaveDimmer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether HaveGroupCmd
        /// </summary>
        public bool HaveGroupCmd { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether HaveSelector
        /// </summary>
        public bool HaveSelector { get; set; }

        /// <summary>
        /// Gets or sets the result
        /// </summary>
        public Log[] result { get; set; }

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
    /// Defines the <see cref="Log" />
    /// </summary>
    public class Log
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Data
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the Date
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the Level
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the MaxDimLevel
        /// </summary>
        public int MaxDimLevel { get; set; }

        /// <summary>
        /// Gets or sets the Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the idx
        /// </summary>
        public string idx { get; set; }

        #endregion
    }
}
