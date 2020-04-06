namespace NL.HNOGames.Domoticz.Models
{
    /// <summary>
    /// Defines the <see cref="VersionModel" />
    /// </summary>
    public class VersionModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the DomoticzUpdateURL
        /// </summary>
        public string DomoticzUpdateURL { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether HaveUpdate
        /// </summary>
        public bool HaveUpdate { get; set; }

        /// <summary>
        /// Gets or sets the Revision
        /// </summary>
        public int Revision { get; set; }

        /// <summary>
        /// Gets or sets the SystemName
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the build_time
        /// </summary>
        public string build_time { get; set; }

        /// <summary>
        /// Gets or sets the hash
        /// </summary>
        public string hash { get; set; }

        /// <summary>
        /// Gets or sets the status
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// Gets or sets the version
        /// </summary>
        public string version { get; set; }

        #endregion
    }
}
