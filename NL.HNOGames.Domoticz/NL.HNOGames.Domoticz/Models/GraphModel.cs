using NL.HNOGames.Domoticz.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Models
{
    public class GraphModel
    {
        public Data[] result { get; set; }
        public string status { get; set; }
        public string title { get; set; }
    }

    public class Data
    {
        public DateTime? dDateTime
        {
            get
            {
                if (DateTime.TryParseExact(d, "yyyy-MM-dd HH:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dResult))
                    return dResult;
                else if (DateTime.TryParseExact(d, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dYearResult))
                    return dYearResult;
                return null;
            }
        }

        public string d { get; set; }
        public double? te { get; set; }
        public String hu { get; set; }
        public String ba { get; set; }
        public double? se { get; set; }
        public double? ta { get; set; }
        public double? tm { get; set; }
        public String v { get; set; }
        public String v2 { get; set; }
        public String r1 { get; set; }
        public String r2 { get; set; }
        public String eg { get; set; }
        public String eu { get; set; }
        public String vMin { get; set; }
        public String vMax { get; set; }
        public String c { get; set; }
        public String di { get; set; }
        public String gu { get; set; }
        public String sp { get; set; }
        public String uvi { get; set; }
        public String mm { get; set; }
        public String u { get; set; }
        public String u_max { get; set; }
        public String co2_min { get; set; }
        public String co2_max { get; set; }
        public String co2 { get; set; }

        public double? getValue()
        {
            if (!double.TryParse(v, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        public double? getSecondValue()
        {
            if (!double.TryParse(v2, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        public double? getPowerReturn()
        {
            if (!double.TryParse(r1, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        public double? getSecondPowerReturn()
        {
            if (!double.TryParse(r2, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        public double? getPowerUsage()
        {
            if (!double.TryParse(eu, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        public double? getPowerDelivery()
        {
            if (!double.TryParse(eg, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        public double? getValueMin()
        {
            if (!double.TryParse(vMin, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        public double? getValueMax()
        {
            if (!double.TryParse(vMax, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        public double? getTemperature()
        {
            if (!ta.HasValue)
                return te;
            else
                return ta;
        }

        public double? getTemperatureMin()
        {
            return tm;
        }

        public double? getTemperatureMax()
        {
            return te;
        }

        public bool hasTemperatureRange()
        {
            return ta.HasValue;
        }

        public double? getSetPoint()
        {
            return se;
        }

        public double? getHumidity()
        {
            if (!double.TryParse(hu, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        public double? getSunPower()
        {
            if (!double.TryParse(uvi, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        public double? getCounter()
        {
            if (!double.TryParse(c, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        public double? getSpeed()
        {
            if (!double.TryParse(sp, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        public double? getDirection()
        {
            if (!double.TryParse(di, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        public double? getRain()
        {
            if (!double.TryParse(mm, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        public double? getUsage()
        {
            if (!double.TryParse(u, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        public double? getBarometer()
        {
            if (!double.TryParse(ba, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        public DateTime? getDateTime()
        {
            return dDateTime;
        }

        public double? getCo2()
        {
            if (!double.TryParse(co2, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        public double? getCo2Max()
        {
            if (!double.TryParse(co2_max, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        public double? getCo2Min()
        {
            if (!double.TryParse(co2_min, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }
    }
}
