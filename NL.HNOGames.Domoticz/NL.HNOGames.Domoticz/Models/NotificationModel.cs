using NL.HNOGames.Domoticz.Resources;
using System;

namespace NL.HNOGames.Domoticz.Models
{
    /// <summary>
    /// Defines the <see cref="NotificationModel" />
    /// </summary>
    public class NotificationModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the notifiers
        /// </summary>
        public Notifier[] notifiers { get; set; }

        /// <summary>
        /// Gets or sets the result
        /// </summary>
        public Notification[] result { get; set; }

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
    /// Defines the <see cref="Notifier" />
    /// </summary>
    public class Notifier
    {
        #region Properties

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string name { get; set; }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="Notification" />
    /// </summary>
    public class Notification
    {
        #region Properties

        /// <summary>
        /// Gets the PriorityDescription
        /// </summary>
        public String PriorityDescription
        {
            get
            {
                String priority = "";
                if (Priority == 0)
                    priority = AppResources.priority + ": " + AppResources.normal;
                else if (Priority == 1)
                    priority = AppResources.priority + ": " + AppResources.high;
                else if (Priority == 2)
                    priority = AppResources.priority + ": " + AppResources.emergency;
                else if (Priority == -1)
                    priority = AppResources.priority + ": " + AppResources.low;
                else if (Priority == -2)
                    priority = AppResources.priority + ": " + AppResources.verylow;

                return priority;
            }
        }

        /// <summary>
        /// Gets the SystemsDescription
        /// </summary>
        public String SystemsDescription
        {
            get
            {
                String type = "";
                if (string.IsNullOrEmpty(ActiveSystems))
                    type = AppResources.allsystems;
                else
                    type += AppResources.systems + ": " + ActiveSystems.Replace(";", ", ");

                return type;
            }
        }

        /// <summary>
        /// Gets or sets the ActiveSystems
        /// </summary>
        public string ActiveSystems { get; set; }

        /// <summary>
        /// Gets or sets the CustomMessage
        /// </summary>
        public string CustomMessage { get; set; }

        /// <summary>
        /// Gets or sets the Params
        /// </summary>
        public string Params { get; set; }

        /// <summary>
        /// Gets or sets the Priority
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether SendAlways
        /// </summary>
        public bool SendAlways { get; set; }

        /// <summary>
        /// Gets or sets the idx
        /// </summary>
        public int idx { get; set; }

        #endregion
    }
}
