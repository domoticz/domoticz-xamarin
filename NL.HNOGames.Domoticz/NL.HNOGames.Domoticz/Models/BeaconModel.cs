using NL.HNOGames.Domoticz.Resources;
using Shiny.Beacons;
using System;

namespace NL.HNOGames.Domoticz.Models
{
    /// <summary>
    /// Defines the <see cref="BeaconModel" />
    /// </summary>
    public class BeaconModel
    {
        #region Variables

        /// <summary>
        /// Defines the beacon
        /// </summary>
        public Guid UUID;

        public ushort Minor;
        public ushort Major;

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

        #endregion
    }
}
