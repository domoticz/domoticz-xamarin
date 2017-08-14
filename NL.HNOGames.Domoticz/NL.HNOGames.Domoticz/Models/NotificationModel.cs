using NL.HNOGames.Domoticz.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Models
{
    public class NotificationModel
    {
        public Notifier[] notifiers { get; set; }
        public Notification[] result { get; set; }
        public string status { get; set; }
        public string title { get; set; }
    }

    public class Notifier
    {
        public string description { get; set; }
        public string name { get; set; }
    }

    public class Notification
    {
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

        public string ActiveSystems { get; set; }
        public string CustomMessage { get; set; }
        public string Params { get; set; }
        public int Priority { get; set; }
        public bool SendAlways { get; set; }
        public int idx { get; set; }
    }

}
