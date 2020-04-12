using NL.HNOGames.Domoticz.Resources;
using System;

namespace NL.HNOGames.Domoticz.Models
{
    /// <summary>
    /// Defines the <see cref="GeofenceModel" />
    /// </summary>
    public class GeofenceModel
    {
        #region Variables

        /// <summary>
        /// Defines the Radius
        /// </summary>
        public int Radius = 400;

        /// <summary>
        /// Defines the Latitude
        /// </summary>
        public double Latitude;

        /// <summary>
        /// Defines the Longitude
        /// </summary>
        public double Longitude;

        /// <summary>
        /// Defines the Address
        /// </summary>
        public String Address;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether Enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsScene
        /// </summary>
        public bool IsScene { get; set; }

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the SwitchIDX
        /// </summary>
        public string SwitchIDX { get; set; }

        /// <summary>
        /// Gets or sets the SwitchName
        /// </summary>
        public string SwitchName { get; set; }

        /// <summary>
        /// Gets or sets the SwitchPassword
        /// </summary>
        public string SwitchPassword { get; set; }

        /// <summary>
        /// Gets or sets the Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets the SwitchDescription
        /// </summary>
        public string SwitchDescription
        {
            get
            {
                return string.IsNullOrEmpty(SwitchName) ? AppResources.connectedSwitch + ": -" : AppResources.connectedSwitch + ": " + SwitchName;
            }
        }

        /// <summary>
        /// Gets the RadiusDescription
        /// </summary>
        public string RadiusDescription
        {
            get
            {
                return AppResources.radius + ": " + Radius;
            }
        }

        #endregion
    }
}
