using System;
using System.Collections.Generic;
using System.Text;

namespace NL.HNOGames.Domoticz.Models
{
    /// <summary>
    /// Login model
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Gets or sets the rights
        /// </summary>
        public int rights { get; set; }

        /// <summary>
        /// Gets or sets the status
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// Gets or sets the user
        /// </summary>
        public string user { get; set; }

        /// <summary>
        /// Gets or sets the version
        /// </summary>
        public string version { get; set; }
    }
}
