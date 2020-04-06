using System;

namespace NL.HNOGames.Domoticz.Helpers
{
    /// <summary>
    /// Defines the <see cref="MessagingCenterAlert" />
    /// </summary>
    public class MessagingCenterAlert
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the Cancel
        /// Gets or sets a value indicating whether this instance cancel/OK text.
        /// </summary>
        public string Cancel { get; set; }

        /// <summary>
        /// Gets or sets the OnCompleted Action.
        /// </summary>
        public Action OnCompleted { get; set; }

        #endregion

        #region Public

        /// <summary>
        /// Init this instance.
        /// </summary>
        public static void Init()
        {
            var time = DateTime.UtcNow;
        }

        #endregion
    }
}
