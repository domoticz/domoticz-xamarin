using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Models
{
    public class ServerLogsModel
    {
        public string LastLogTime { get; set; }
        public ServerLog[] result { get; set; }
        public string status { get; set; }
        public string title { get; set; }
    }

    public class ServerLog
    {
        public string Date
        {
            get
            {
                if (message.Contains("  "))
                {
                    return message.Substring(0, message.IndexOf("  "));
                }
                return "";
            }
        }
        public string Data
        {
            get
            {
                if (message.Contains("  "))
                {
                    return message.Substring(message.IndexOf("  ") + 2);
                }
                return "";
            }
        }

        public int level { get; set; }
        public string message { get; set; }
    }
}
