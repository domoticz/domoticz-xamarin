using NL.HNOGames.Domoticz.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Models
{
    public class EventModel
    {
        public string interpreters { get; set; }
        public Event[] result { get; set; }
        public string status { get; set; }
        public string title { get; set; }
    }

    public class Event
    {
        public string Data
        {
            get
            {
                return eventstatus == "1" ? AppResources.button_state_on : AppResources.button_state_off;
            }
        }

        public string eventstatus { get; set; }

        public bool Enabled
        {
            get
            {
                return eventstatus == "1" ? true : false;
            }
        }

        public string id { get; set; }
        public string Name { get; set; }
    }
}
