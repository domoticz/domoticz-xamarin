using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Models
{
    public class ConfigModel
    {
        public bool AllowWidgetOrdering { get; set; }
        public int DashboardType { get; set; }
        public float DegreeDaysBaseTemperature { get; set; }
        public int FiveMinuteHistoryDays { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int MobileType { get; set; }
        public float TempScale { get; set; }
        public string TempSign { get; set; }
        public float WindScale { get; set; }
        public string WindSign { get; set; }
        public string language { get; set; }
        public Result result { get; set; }
        public string status { get; set; }
        public string title { get; set; }
    }

    public class Result
    {
        public bool EnableTabCustom { get; set; }
        public bool EnableTabDashboard { get; set; }
        public bool EnableTabFloorplans { get; set; }
        public bool EnableTabLights { get; set; }
        public bool EnableTabProxy { get; set; }
        public bool EnableTabScenes { get; set; }
        public bool EnableTabTemp { get; set; }
        public bool EnableTabUtility { get; set; }
        public bool EnableTabWeather { get; set; }
        public bool ShowUpdatedEffect { get; set; }
    }

}
