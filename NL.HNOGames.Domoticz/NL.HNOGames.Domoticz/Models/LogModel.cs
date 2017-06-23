using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Models
{
    public class LogModel
    {
        public bool HaveDimmer { get; set; }
        public bool HaveGroupCmd { get; set; }
        public bool HaveSelector { get; set; }
        public Log[] result { get; set; }
        public string status { get; set; }
        public string title { get; set; }
    }

    public class Log
    {
        public string Data { get; set; }
        public string Date { get; set; }
        public int Level { get; set; }
        public int MaxDimLevel { get; set; }
        public string Status { get; set; }
        public string idx { get; set; }
    }
}

