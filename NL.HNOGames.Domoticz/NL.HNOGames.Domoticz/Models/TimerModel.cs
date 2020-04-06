using NL.HNOGames.Domoticz.Resources;
using System;

namespace NL.HNOGames.Domoticz.Models
{
    /// <summary>
    /// Defines the <see cref="TimerModel" />
    /// </summary>
    public class TimerModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the result
        /// </summary>
        public Timer[] result { get; set; }

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
    /// Defines the <see cref="Timer" />
    /// </summary>
    public class Timer
    {
        #region Properties

        /// <summary>
        /// Gets the TypeDescription
        /// </summary>
        public String TypeDescription
        {
            get
            {
                String type = "";
                if (Type == 0)
                    type += AppResources.type + ": " + AppResources.timer_before_sunrise;
                else if (Type == 1)
                    type += AppResources.type + ": " + AppResources.timer_after_sunrise;
                else if (Type == 2)
                    type += AppResources.type + ": " + AppResources.timer_ontime;
                else if (Type == 3)
                    type += AppResources.type + ": " + AppResources.timer_before_sunset;
                else if (Type == 4)
                    type += AppResources.type + ": " + AppResources.timer_after_sunset;
                else if (Type == 5)
                    type += AppResources.type + ": " + AppResources.timer_fixed;
                else if (Type == 6)
                    type += AppResources.type + ": " + AppResources.odd_day_numbers;
                else if (Type == 7)
                    type += AppResources.type + ": " + AppResources.even_day_numbers;
                else if (Type == 8)
                    type += AppResources.type + ": " + AppResources.odd_week_numbers;
                else if (Type == 9)
                    type += AppResources.type + ": " + AppResources.even_week_numbers;
                else if (Type == 10)
                    type += AppResources.type + ": " + AppResources.monthly;
                else if (Type == 11)
                    type += AppResources.type + ": " + AppResources.monthly_weekday;
                else if (Type == 12)
                    type += AppResources.type + ": " + AppResources.yearly;
                else if (Type == 13)
                    type += AppResources.type + ": " + AppResources.yearly_weekday;
                else
                    type += AppResources.type + ": " + AppResources.notapplicable;
                return type;
            }
        }

        /// <summary>
        /// Gets the CommandDescription
        /// </summary>
        public String CommandDescription
        {
            get
            {
                String commando = "";
                if (Cmd == 0)
                    commando += AppResources.command + ": " + AppResources.button_state_on;
                else
                    commando += AppResources.command + ": " + AppResources.button_state_off;
                return commando;
            }
        }

        /// <summary>
        /// Gets the Description
        /// </summary>
        public String Description
        {
            get
            {
                String returnvalue = Active;

                if (Date != null && Date.Length > 0)
                    returnvalue = Active + " | " + Date;
                else
                {
                    if (Days == 128)
                        returnvalue = Active + " | " + AppResources.timer_every_days;
                    else if (Days == 512)
                        returnvalue = Active + " | " + AppResources.timer_weekend;
                    else if (Days == 256)
                        returnvalue = Active + " | " + AppResources.timer_working_days;
                    else if (Days == 512)
                        returnvalue = Active + " | " + AppResources.timer_weekend;
                    else
                        returnvalue = Active + " | " + AppResources.timer_other;
                }

                return returnvalue;
            }
        }

        /// <summary>
        /// Gets or sets the Active
        /// </summary>
        public string Active { get; set; }

        /// <summary>
        /// Gets or sets the Date
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the Days
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// Gets or sets the MDay
        /// </summary>
        public int MDay { get; set; }

        /// <summary>
        /// Gets or sets the Month
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Gets or sets the Occurence
        /// </summary>
        public int Occurence { get; set; }

        /// <summary>
        /// Gets or sets the Temperature
        /// </summary>
        public float Temperature { get; set; }

        /// <summary>
        /// Gets or sets the Time
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Gets or sets the Type
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets the Cmd
        /// </summary>
        public int Cmd { get; set; }

        /// <summary>
        /// Gets or sets the idx
        /// </summary>
        public string idx { get; set; }

        #endregion
    }
}
