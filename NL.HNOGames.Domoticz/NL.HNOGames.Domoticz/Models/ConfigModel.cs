namespace NL.HNOGames.Domoticz.Models
{
    /// <summary>
    /// Defines the <see cref="ConfigModel" />
    /// </summary>
    public class ConfigModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether AllowWidgetOrdering
        /// </summary>
        public bool AllowWidgetOrdering { get; set; }

        /// <summary>
        /// Gets or sets the DashboardType
        /// </summary>
        public int DashboardType { get; set; }

        /// <summary>
        /// Gets or sets the DegreeDaysBaseTemperature
        /// </summary>
        public float DegreeDaysBaseTemperature { get; set; }

        /// <summary>
        /// Gets or sets the FiveMinuteHistoryDays
        /// </summary>
        public int FiveMinuteHistoryDays { get; set; }

        /// <summary>
        /// Gets or sets the Latitude
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// Gets or sets the Longitude
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// Gets or sets the MobileType
        /// </summary>
        public int MobileType { get; set; }

        /// <summary>
        /// Gets or sets the TempScale
        /// </summary>
        public float TempScale { get; set; }

        /// <summary>
        /// Gets or sets the TempSign
        /// </summary>
        public string TempSign { get; set; }

        /// <summary>
        /// Gets or sets the WindScale
        /// </summary>
        public float WindScale { get; set; }

        /// <summary>
        /// Gets or sets the WindSign
        /// </summary>
        public string WindSign { get; set; }

        /// <summary>
        /// Gets or sets the language
        /// </summary>
        public string language { get; set; }

        /// <summary>
        /// Gets or sets the result
        /// </summary>
        public Result result { get; set; }

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
    /// Defines the <see cref="Result" />
    /// </summary>
    public class Result
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether EnableTabCustom
        /// </summary>
        public bool EnableTabCustom { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether EnableTabDashboard
        /// </summary>
        public bool EnableTabDashboard { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether EnableTabFloorplans
        /// </summary>
        public bool EnableTabFloorplans { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether EnableTabLights
        /// </summary>
        public bool EnableTabLights { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether EnableTabProxy
        /// </summary>
        public bool EnableTabProxy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether EnableTabScenes
        /// </summary>
        public bool EnableTabScenes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether EnableTabTemp
        /// </summary>
        public bool EnableTabTemp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether EnableTabUtility
        /// </summary>
        public bool EnableTabUtility { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether EnableTabWeather
        /// </summary>
        public bool EnableTabWeather { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ShowUpdatedEffect
        /// </summary>
        public bool ShowUpdatedEffect { get; set; }

        #endregion
    }
}
