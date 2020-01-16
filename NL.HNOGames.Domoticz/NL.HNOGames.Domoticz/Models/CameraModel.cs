namespace NL.HNOGames.Domoticz.Models
{
    /// <summary>
    /// Defines the <see cref="CameraModel" />
    /// </summary>
    public class CameraModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the result
        /// </summary>
        public Camera[] result { get; set; }

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
    /// Defines the <see cref="Camera" />
    /// </summary>
    public class Camera
    {
        #region Properties

        /// <summary>
        /// Gets or sets the ImageBytes
        /// </summary>
        public byte[] ImageBytes { get; set; }

        /// <summary>
        /// Gets or sets the Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the Enabled
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// Gets or sets the ImageURL
        /// </summary>
        public string ImageURL { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the Port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the Protocol
        /// </summary>
        public int Protocol { get; set; }

        /// <summary>
        /// Gets or sets the Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the idx
        /// </summary>
        public string idx { get; set; }

        #endregion
    }
}
