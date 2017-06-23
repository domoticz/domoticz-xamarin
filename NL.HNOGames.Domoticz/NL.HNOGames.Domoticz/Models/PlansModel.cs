using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Models
{
    public class PlansModel
    {
        public Plan[] result { get; set; }
        public string status { get; set; }
        public string title { get; set; }
    }

    public class Plan
    {
        public int Devices { get; set; }
        public string Name { get; set; }
        public string Order { get; set; }
        public string idx { get; set; }
    }
}
