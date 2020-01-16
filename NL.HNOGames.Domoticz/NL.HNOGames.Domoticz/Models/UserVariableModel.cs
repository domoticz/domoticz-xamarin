namespace NL.HNOGames.Domoticz.Models
{
    /// <summary>
    /// Defines the <see cref="UserVariableModel" />
    /// </summary>
    public class UserVariableModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the result
        /// </summary>
        public UserVariable[] result { get; set; }

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
    /// Defines the <see cref="UserVariable" />
    /// </summary>
    public class UserVariable
    {
        #region Properties

        /// <summary>
        /// Gets the Data
        /// </summary>
        public string Data
        {
            get
            {
                return string.Format("{0} ({1})", Value, TypeValue);
            }
        }

        /// <summary>
        /// Gets the TypeValue
        /// </summary>
        public string TypeValue
        {
            get
            {
                switch (Type)
                {
                    case "0":
                        return "Integer";
                    case "1":
                        return "Float";
                    case "2":
                        return "String";
                    case "3":
                        return "Date";
                    case "4":
                        return "Time";
                }
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the LastUpdate
        /// </summary>
        public string LastUpdate { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the idx
        /// </summary>
        public string idx { get; set; }

        #endregion
    }
}
