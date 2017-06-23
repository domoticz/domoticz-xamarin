using NL.HNOGames.Domoticz.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Models
{
    public class TimerModel
    {
        public Timer[] result { get; set; }
        public string status { get; set; }
        public string title { get; set; }

    }

    public class Timer
    {
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

        public String Description
        {
            get
            {
                String returnvalue = Active;

                if (Date != null && Date.Length > 0)
                    returnvalue = Active + " | " +Date;
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

        public string Active { get; set; }
        public string Date { get; set; }
        public int Days { get; set; }
        public int MDay { get; set; }
        public int Month { get; set; }
        public int Occurence { get; set; }
        public float Temperature { get; set; }
        public string Time { get; set; }
        public int Type { get; set; }
        public int Cmd { get; set; }
        public string idx { get; set; }
    }
}
