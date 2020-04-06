using NL.HNOGames.Domoticz.Resources;

namespace NL.HNOGames.Domoticz.Models
{
    /// <summary>
    /// Defines the <see cref="EventModel" />
    /// </summary>
    public class EventModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the interpreters
        /// </summary>
        public string interpreters { get; set; }

        /// <summary>
        /// Gets or sets the result
        /// </summary>
        public Event[] result { get; set; }

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
    /// Defines the <see cref="Event" />
    /// </summary>
    public class Event
    {
        #region Properties

        /// <summary>
        /// Gets the Data
        /// </summary>
        public string Data
        {
            get
            {
                return eventstatus == "1" ? AppResources.button_state_on : AppResources.button_state_off;
            }
        }

        /// <summary>
        /// Gets or sets the eventstatus
        /// </summary>
        public string eventstatus { get; set; }

        /// <summary>
        /// Gets a value indicating whether Enabled
        /// </summary>
        public bool Enabled
        {
            get
            {
                return eventstatus == "1" ? true : false;
            }
        }

        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}
