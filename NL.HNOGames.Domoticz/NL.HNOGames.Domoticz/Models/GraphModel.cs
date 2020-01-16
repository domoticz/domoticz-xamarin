using System;
using System.Globalization;

namespace NL.HNOGames.Domoticz.Models
{
    /// <summary>
    /// Defines the <see cref="GraphModel" />
    /// </summary>
    public class GraphModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the result
        /// </summary>
        public Data[] result { get; set; }

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
    /// Defines the <see cref="Data" />
    /// </summary>
    public class Data
    {
        #region Properties

        /// <summary>
        /// Gets the dDateTime
        /// </summary>
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

        /// <summary>
        /// Gets or sets the d
        /// </summary>
        public string d { get; set; }

        /// <summary>
        /// Gets or sets the te
        /// </summary>
        public double? te { get; set; }

        /// <summary>
        /// Gets or sets the hu
        /// </summary>
        public String hu { get; set; }

        /// <summary>
        /// Gets or sets the ba
        /// </summary>
        public String ba { get; set; }

        /// <summary>
        /// Gets or sets the se
        /// </summary>
        public double? se { get; set; }

        /// <summary>
        /// Gets or sets the ta
        /// </summary>
        public double? ta { get; set; }

        /// <summary>
        /// Gets or sets the tm
        /// </summary>
        public double? tm { get; set; }

        /// <summary>
        /// Gets or sets the v
        /// </summary>
        public String v { get; set; }

        /// <summary>
        /// Gets or sets the v2
        /// </summary>
        public String v2 { get; set; }

        /// <summary>
        /// Gets or sets the r1
        /// </summary>
        public String r1 { get; set; }

        /// <summary>
        /// Gets or sets the r2
        /// </summary>
        public String r2 { get; set; }

        /// <summary>
        /// Gets or sets the eg
        /// </summary>
        public String eg { get; set; }

        /// <summary>
        /// Gets or sets the eu
        /// </summary>
        public String eu { get; set; }

        /// <summary>
        /// Gets or sets the v_min
        /// </summary>
        public String v_min { get; set; }

        /// <summary>
        /// Gets or sets the v_max
        /// </summary>
        public String v_max { get; set; }

        /// <summary>
        /// Gets or sets the v_avg
        /// </summary>
        public String v_avg { get; set; }

        /// <summary>
        /// Gets or sets the c
        /// </summary>
        public String c { get; set; }

        /// <summary>
        /// Gets or sets the di
        /// </summary>
        public String di { get; set; }

        /// <summary>
        /// Gets or sets the gu
        /// </summary>
        public String gu { get; set; }

        /// <summary>
        /// Gets or sets the sp
        /// </summary>
        public String sp { get; set; }

        /// <summary>
        /// Gets or sets the uvi
        /// </summary>
        public String uvi { get; set; }

        /// <summary>
        /// Gets or sets the mm
        /// </summary>
        public String mm { get; set; }

        /// <summary>
        /// Gets or sets the u
        /// </summary>
        public String u { get; set; }

        /// <summary>
        /// Gets or sets the u_max
        /// </summary>
        public String u_max { get; set; }

        /// <summary>
        /// Gets or sets the co2_min
        /// </summary>
        public String co2_min { get; set; }

        /// <summary>
        /// Gets or sets the co2_max
        /// </summary>
        public String co2_max { get; set; }

        /// <summary>
        /// Gets or sets the co2
        /// </summary>
        public String co2 { get; set; }

        /// <summary>
        /// Gets or sets the lux
        /// </summary>
        public String lux { get; set; }

        /// <summary>
        /// Gets or sets the lux_min
        /// </summary>
        public String lux_min { get; set; }

        /// <summary>
        /// Gets or sets the lux_max
        /// </summary>
        public String lux_max { get; set; }

        /// <summary>
        /// Gets or sets the lux_avg
        /// </summary>
        public String lux_avg { get; set; }

        #endregion

        #region Public

        /// <summary>
        /// The getValue
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getValue()
        {
            if (!double.TryParse(v, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        /// <summary>
        /// The getSecondValue
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getSecondValue()
        {
            if (!double.TryParse(v2, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        /// <summary>
        /// The getValueMin
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getValueMin()
        {
            if (!double.TryParse(v_min, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        /// <summary>
        /// The getValueMax
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getValueMax()
        {
            if (!double.TryParse(v_max, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        /// <summary>
        /// The getValueAvg
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getValueAvg()
        {
            if (!double.TryParse(v_avg, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        /// <summary>
        /// The getPowerReturn
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getPowerReturn()
        {
            if (!double.TryParse(r1, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        /// <summary>
        /// The getSecondPowerReturn
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getSecondPowerReturn()
        {
            if (!double.TryParse(r2, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        /// <summary>
        /// The getPowerUsage
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getPowerUsage()
        {
            if (!double.TryParse(eu, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        /// <summary>
        /// The getPowerDelivery
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getPowerDelivery()
        {
            if (!double.TryParse(eg, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        /// <summary>
        /// The getTemperature
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getTemperature()
        {
            if (!ta.HasValue)
                return te;
            else
                return ta;
        }

        /// <summary>
        /// The getTemperatureMin
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getTemperatureMin()
        {
            return tm;
        }

        /// <summary>
        /// The getTemperatureMax
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getTemperatureMax()
        {
            return te;
        }

        /// <summary>
        /// The hasTemperatureRange
        /// </summary>
        /// <returns>The <see cref="bool"/></returns>
        public bool hasTemperatureRange()
        {
            return ta.HasValue;
        }

        /// <summary>
        /// The getSetPoint
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getSetPoint()
        {
            return se;
        }

        /// <summary>
        /// The getHumidity
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getHumidity()
        {
            if (!double.TryParse(hu, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        /// <summary>
        /// The getSunPower
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getSunPower()
        {
            if (!double.TryParse(uvi, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        /// <summary>
        /// The getCounter
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getCounter()
        {
            if (!double.TryParse(c, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        /// <summary>
        /// The getSpeed
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getSpeed()
        {
            if (!double.TryParse(sp, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        /// <summary>
        /// The getDirection
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getDirection()
        {
            if (!double.TryParse(di, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        /// <summary>
        /// The getRain
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getRain()
        {
            if (!double.TryParse(mm, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        /// <summary>
        /// The getUsage
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getUsage()
        {
            if (!double.TryParse(u, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        /// <summary>
        /// The getBarometer
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getBarometer()
        {
            if (!double.TryParse(ba, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        /// <summary>
        /// The getDateTime
        /// </summary>
        /// <returns>The <see cref="DateTime?"/></returns>
        public DateTime? getDateTime()
        {
            return dDateTime;
        }

        /// <summary>
        /// The getCo2
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getCo2()
        {
            if (!double.TryParse(co2, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        /// <summary>
        /// The getCo2Max
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getCo2Max()
        {
            if (!double.TryParse(co2_max, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        /// <summary>
        /// The getCo2Min
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getCo2Min()
        {
            if (!double.TryParse(co2_min, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        /// <summary>
        /// The getLux
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getLux()
        {
            if (!double.TryParse(lux, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        /// <summary>
        /// The getLuxMax
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getLuxMax()
        {
            if (!double.TryParse(lux_max, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        /// <summary>
        /// The getLuxMin
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getLuxMin()
        {
            if (!double.TryParse(lux_min, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        /// <summary>
        /// The getLuxAvg
        /// </summary>
        /// <returns>The <see cref="double?"/></returns>
        public double? getLuxAvg()
        {
            if (!double.TryParse(lux_avg, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                return null;
            return result;
        }

        #endregion
    }
}
