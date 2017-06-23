using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Models
{

    public class VersionModel
    {
        public string DomoticzUpdateURL { get; set; }
        public bool HaveUpdate { get; set; }
        public int Revision { get; set; }
        public string SystemName { get; set; }
        public string build_time { get; set; }
        public string hash { get; set; }
        public string status { get; set; }
        public string title { get; set; }
        public string version { get; set; }
    }
}
