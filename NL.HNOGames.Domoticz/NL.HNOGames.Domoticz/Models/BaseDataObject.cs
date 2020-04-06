using NL.HNOGames.Domoticz.Helpers;
using System;

namespace NL.HNOGames.Domoticz.Models
{
    /// <summary>
    /// Defines the <see cref="BaseDataObject" />
    /// </summary>
    public class BaseDataObject : ObservableObject
    {
        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDataObject"/> class.
        /// </summary>
        public BaseDataObject()
        {
            Id = Guid.NewGuid().ToString();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Id
        /// Id for item
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the CreatedAt
        /// Azure created at time stamp
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the UpdatedAt
        /// Azure UpdateAt timestamp for online/offline sync
        /// </summary>
        public DateTimeOffset UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the AzureVersion
        /// Azure version for online/offline sync
        /// </summary>
        public string AzureVersion { get; set; }

        #endregion
    }
}
