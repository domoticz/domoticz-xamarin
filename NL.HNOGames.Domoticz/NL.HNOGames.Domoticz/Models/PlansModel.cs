namespace NL.HNOGames.Domoticz.Models
{
    /// <summary>
    /// Defines the <see cref="PlansModel" />
    /// </summary>
    public class PlansModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the result
        /// </summary>
        public Plan[] result { get; set; }

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
    /// Defines the <see cref="Plan" />
    /// </summary>
    public class Plan
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Devices
        /// </summary>
        public int Devices { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Order
        /// </summary>
        public string Order { get; set; }

        /// <summary>
        /// Gets or sets the idx
        /// </summary>
        public string idx { get; set; }

        #endregion
    }
}
