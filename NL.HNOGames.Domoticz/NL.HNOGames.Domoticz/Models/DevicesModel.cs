using Humanizer;
using Newtonsoft.Json;
using NL.HNOGames.Domoticz.Data;
using NL.HNOGames.Domoticz.Helpers;
using NL.HNOGames.Domoticz.Resources;
using System;
using System.Globalization;

namespace NL.HNOGames.Domoticz.Models
{
    /// <summary>
    /// Defines the <see cref="DevicesModel" />
    /// </summary>
    public class DevicesModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the ActTime
        /// </summary>
        public int ActTime { get; set; }

        /// <summary>
        /// Gets or sets the ServerTime
        /// </summary>
        public string ServerTime { get; set; }

        /// <summary>
        /// Gets or sets the Sunrise
        /// </summary>
        public string Sunrise { get; set; }

        /// <summary>
        /// Gets or sets the Sunset
        /// </summary>
        public string Sunset { get; set; }

        /// <summary>
        /// Gets or sets the result
        /// </summary>
        public Device[] result { get; set; }

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
    /// Defines the <see cref="Device" />
    /// </summary>
    public class Device
    {
        #region Variables

        /// <summary>
        /// Defines the _levelNames
        /// </summary>
        private string _levelNames;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether ShowExtraData
        /// </summary>
        public bool ShowExtraData { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether IsDashboard
        /// </summary>
        public bool IsDashboard { get; set; } = false;

        /// <summary>
        /// Gets the RowHeight
        /// </summary>
        public int RowHeight
        {
            get
            {
                return ViewHelper.GetTemplateHeight(this, IsDashboard);
            }
        }

        /// <summary>
        /// Gets the Icon
        /// </summary>
        public string Icon
        {
            get
            {
                string selectedIcon = IconService.getDrawableIcon(TypeImg,
                Type,
                SubType,
                StatusBoolean,
                CustomImage != null && CustomImage.HasValue,
                Image);
                return selectedIcon;
            }
        }

        /// <summary>
        /// Gets the Opacity
        /// </summary>
        public double Opacity
        {
            get
            {
                double opacity = 1.0;
                if (!StatusBoolean)
                    opacity = 0.5;
                return opacity;
            }
        }

        /// <summary>
        /// Gets a value indicating whether IsScene
        /// </summary>
        public bool IsScene
        {
            get
            {
                if (!string.IsNullOrEmpty(Type) &&
                    (string.Compare(Type, ConstantValues.Device.Scene.Type.SCENE, StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(Type, ConstantValues.Device.Scene.Type.GROUP, StringComparison.OrdinalIgnoreCase) == 0))
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Gets the FavoriteIcon
        /// </summary>
        public string FavoriteIcon
        {
            get
            {
                return Favorite > 0 ? "ic_star.png" : "ic_star_border.png";
            }
        }

        /// <summary>
        /// Gets the FavoriteIconTintColor
        /// </summary>
        public string FavoriteIconTintColor
        {
            get
            {
                return Favorite > 0 ? "#FFEB3B" : "#999999";
            }
        }

        /// <summary>
        /// Gets a value indicating whether StatusBoolean
        /// </summary>
        public bool StatusBoolean
        {
            get
            {
                try
                {
                    bool statusBoolean = true;
                    if (string.Compare(Status, ConstantValues.Device.Blind.State.OFF, StringComparison.OrdinalIgnoreCase) == 0 ||
                        string.Compare(Status, ConstantValues.Device.Blind.State.CLOSED, StringComparison.OrdinalIgnoreCase) == 0 ||
                        string.Compare(Status, ConstantValues.Device.Door.State.UNLOCKED, StringComparison.OrdinalIgnoreCase) == 0)
                        statusBoolean = false;
                    return statusBoolean;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether FavoriteBoolean
        /// </summary>
        public bool FavoriteBoolean
        {
            get
            {
                return Favorite == 1;
            }
        }

        /// <summary>
        /// Gets the LastUpdateDescription
        /// </summary>
        public string LastUpdateDescription
        {
            get
            {
                var d = DateTime.ParseExact(LastUpdate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                return string.Format("{0}: {1}", AppResources.last_update, (DateTime.Now - d).Humanize(2));
            }
        }

        /// <summary>
        /// Gets the LevelNamesArray
        /// </summary>
        public string[] LevelNamesArray
        {
            get
            {
                if (string.IsNullOrWhiteSpace(LevelNames))
                    return null;
                return LevelNames.Split('|');
            }
        }

        /// <summary>
        /// Gets the LevelNamesIndex
        /// </summary>
        public int LevelNamesIndex
        {
            get
            {
                if (LevelInt > 0)
                    return LevelInt / 10;
                return 0;
            }
        }

        /// <summary>
        /// Gets the ParsedColor
        /// </summary>
        public ColorModel ParsedColor
        {
            get
            {
                if (string.IsNullOrEmpty(Color))
                    return null;
                else
                    return JsonConvert.DeserializeObject<ColorModel>(Color);
            }
        }

        /// <summary>
        /// Gets the DataDescription
        /// </summary>
        public string DataDescription
        {
            get
            {
                try
                {
                    var dataText = string.Empty;
                    if (!string.IsNullOrEmpty(this.Usage))
                        dataText = AppResources.usage + ": " + this.Usage;
                    if (!string.IsNullOrEmpty(this.CounterToday))
                        dataText += " " + AppResources.today + ": " + this.CounterToday;
                    if (!string.IsNullOrEmpty(this.Counter) &&
                        string.Compare(this.Counter, this.Data, StringComparison.OrdinalIgnoreCase) != 0)
                        dataText += " " + AppResources.total + ": " + this.Counter;
                    if (!string.IsNullOrEmpty(this.Type) && string.Compare(this.Type, ConstantValues.Device.Type.Name.WIND, StringComparison.OrdinalIgnoreCase) == 0)
                        dataText = AppResources.direction + ": " + this.Direction + " " + this.DirectionStr;
                    if (!string.IsNullOrEmpty(this.ForecastStr))
                        dataText = this.ForecastStr;

                    if (this.Temp.HasValue)
                        dataText += !string.IsNullOrEmpty(dataText) ? ", " + AppResources.temp + ": " + this.Temp + " " + App.GetServerConfig().TempSign : AppResources.temp + ": " + this.Temp + " " + App.GetServerConfig().TempSign;
                    if (!string.IsNullOrEmpty(this.HumidityStatus))
                    {
                        dataText += !string.IsNullOrEmpty(dataText) ? ", " + AppResources.humidity + ": " + this.HumidityStatus : AppResources.humidity + ": " + this.HumidityStatus;
                        if (this.Humidity > 0)
                            dataText += $" ({Humidity}%)";
                    }

                    if (!string.IsNullOrEmpty(this.Speed))
                        dataText += !string.IsNullOrEmpty(dataText) ? ", " + AppResources.speed + ": " + this.Speed + " " + App.GetServerConfig().WindSign : AppResources.speed + ": " + this.Speed + " " + App.GetServerConfig().WindSign;
                    if (!string.IsNullOrEmpty(this.DewPoint))
                        dataText += !string.IsNullOrEmpty(dataText) ? ", " + AppResources.dewPoint + ": " + this.DewPoint + " " + App.GetServerConfig().TempSign : AppResources.dewPoint + ": " + this.DewPoint + " " + App.GetServerConfig().TempSign;
                    if (this.Barometer.HasValue)
                        dataText += !string.IsNullOrEmpty(dataText) ? ", " + AppResources.pressure + ": " + this.Barometer : AppResources.pressure + ": " + this.Barometer;
                    if (this.Chill.HasValue)
                        dataText += !string.IsNullOrEmpty(dataText) ? ", " + AppResources.chill + ": " + this.Chill + " " + App.GetServerConfig().TempSign : AppResources.chill + ": " + this.Chill + " " + App.GetServerConfig().TempSign;

                    if (!string.IsNullOrEmpty(this.Rain))
                        dataText = AppResources.rain + ": " + this.Rain + " mm";
                    if (!string.IsNullOrEmpty(this.RainRate))
                        dataText += ", " + AppResources.rainrate + ": " + this.RainRate + " mm/h";
                    if (string.IsNullOrEmpty(dataText))
                        dataText = Data;

                    if (!dataText.StartsWith(AppResources.usage) && !dataText.StartsWith(AppResources.direction))
                        return string.Format("{0}: {1}", AppResources.status, dataText);
                    else
                        return dataText;
                }
                catch (Exception ex)
                {
                    App.AddLog(ex.Message);
                    return "";
                }
            }
        }

        /// <summary>
        /// Gets or sets the AddjMulti
        /// </summary>
        public float AddjMulti { get; set; }

        /// <summary>
        /// Gets or sets the AddjMulti2
        /// </summary>
        public float AddjMulti2 { get; set; }

        /// <summary>
        /// Gets or sets the AddjValue
        /// </summary>
        public float AddjValue { get; set; }

        /// <summary>
        /// Gets or sets the AddjValue2
        /// </summary>
        public float AddjValue2 { get; set; }

        /// <summary>
        /// Gets or sets the BatteryLevel
        /// </summary>
        public int BatteryLevel { get; set; }

        /// <summary>
        /// Gets or sets the CustomImage
        /// </summary>
        public int? CustomImage { get; set; }

        /// <summary>
        /// Gets or sets the Data
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Favorite
        /// </summary>
        public int Favorite { get; set; }

        /// <summary>
        /// Gets or sets the HardwareID
        /// </summary>
        public int HardwareID { get; set; }

        /// <summary>
        /// Gets or sets the HardwareName
        /// </summary>
        public string HardwareName { get; set; }

        /// <summary>
        /// Gets or sets the HardwareType
        /// </summary>
        public string HardwareType { get; set; }

        /// <summary>
        /// Gets or sets the HardwareTypeVal
        /// </summary>
        public int HardwareTypeVal { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether HaveTimeout
        /// </summary>
        public bool HaveTimeout { get; set; }

        /// <summary>
        /// Gets or sets the ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the Image
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the LastUpdate
        /// </summary>
        public string LastUpdate { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Notifications
        /// </summary>
        public string Notifications { get; set; }

        /// <summary>
        /// Gets or sets the PlanID
        /// </summary>
        public string PlanID { get; set; }

        /// <summary>
        /// Gets or sets the PlanIDs
        /// </summary>
        public int[] PlanIDs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Protected
        /// </summary>
        public bool Protected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ShowNotifications
        /// </summary>
        public bool ShowNotifications { get; set; }

        /// <summary>
        /// Gets or sets the SignalLevel
        /// </summary>
        public string SignalLevel { get; set; }

        /// <summary>
        /// Gets or sets the SubType
        /// </summary>
        public string SubType { get; set; }

        /// <summary>
        /// Gets or sets the Timers
        /// </summary>
        public string Timers { get; set; }

        /// <summary>
        /// Gets or sets the Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the TypeImg
        /// </summary>
        public string TypeImg { get; set; }

        /// <summary>
        /// Gets or sets the Unit
        /// </summary>
        public int Unit { get; set; }

        /// <summary>
        /// Gets or sets the Used
        /// </summary>
        public int Used { get; set; }

        /// <summary>
        /// Gets or sets the XOffset
        /// </summary>
        public string XOffset { get; set; }

        /// <summary>
        /// Gets or sets the YOffset
        /// </summary>
        public string YOffset { get; set; }

        /// <summary>
        /// Gets or sets the idx
        /// </summary>
        public string idx { get; set; }

        /// <summary>
        /// Gets or sets the DewPoint
        /// </summary>
        public string DewPoint { get; set; }

        /// <summary>
        /// Gets or sets the Humidity
        /// </summary>
        public int Humidity { get; set; }

        /// <summary>
        /// Gets or sets the HumidityStatus
        /// </summary>
        public string HumidityStatus { get; set; }

        /// <summary>
        /// Gets or sets the Temp
        /// </summary>
        public float? Temp { get; set; }

        /// <summary>
        /// Gets or sets the Barometer
        /// </summary>
        public float? Barometer { get; set; }

        /// <summary>
        /// Gets or sets the Forecast
        /// </summary>
        public int? Forecast { get; set; }

        /// <summary>
        /// Gets or sets the ForecastStr
        /// </summary>
        public string ForecastStr { get; set; }

        /// <summary>
        /// Gets or sets the forecast_url
        /// </summary>
        public string forecast_url { get; set; }

        /// <summary>
        /// Gets or sets the Chill
        /// </summary>
        public float? Chill { get; set; }

        /// <summary>
        /// Gets or sets the Direction
        /// </summary>
        public float? Direction { get; set; }

        /// <summary>
        /// Gets or sets the DirectionStr
        /// </summary>
        public string DirectionStr { get; set; }

        /// <summary>
        /// Gets or sets the Gust
        /// </summary>
        public string Gust { get; set; }

        /// <summary>
        /// Gets or sets the Speed
        /// </summary>
        public string Speed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether HaveDimmer
        /// </summary>
        public bool HaveDimmer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether HaveGroupCmd
        /// </summary>
        public bool HaveGroupCmd { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsSubDevice
        /// </summary>
        public bool IsSubDevice { get; set; }

        /// <summary>
        /// Gets or sets the Level
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the LevelInt
        /// </summary>
        public int LevelInt { get; set; }

        /// <summary>
        /// Gets or sets the MaxDimLevel
        /// </summary>
        public int MaxDimLevel { get; set; }

        /// <summary>
        /// Gets or sets the Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the StrParam1
        /// </summary>
        public string StrParam1 { get; set; }

        /// <summary>
        /// Gets or sets the StrParam2
        /// </summary>
        public string StrParam2 { get; set; }

        /// <summary>
        /// Gets or sets the SwitchType
        /// </summary>
        public string SwitchType { get; set; }

        /// <summary>
        /// Gets or sets the SwitchTypeVal
        /// </summary>
        public int SwitchTypeVal { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether UsedByCamera
        /// </summary>
        public bool UsedByCamera { get; set; }

        /// <summary>
        /// Gets or sets the LevelActions
        /// </summary>
        public string LevelActions { get; set; }

        /// <summary>
        /// Gets or sets the LevelNames
        /// </summary>
        public string LevelNames
        {
            get => _levelNames;
            set
            {
                _levelNames = value;
                if (UsefulBits.IsBase64Encoded(_levelNames))
                    _levelNames = UsefulBits.DecodeBase64String(_levelNames);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether LevelOffHidden
        /// </summary>
        public bool LevelOffHidden { get; set; }

        /// <summary>
        /// Gets or sets the SelectorStyle
        /// </summary>
        public int SelectorStyle { get; set; }

        /// <summary>
        /// Gets or sets the UVI
        /// </summary>
        public string UVI { get; set; }

        /// <summary>
        /// Gets or sets the Visibility
        /// </summary>
        public float Visibility { get; set; }

        /// <summary>
        /// Gets or sets the Counter
        /// </summary>
        public string Counter { get; set; }

        /// <summary>
        /// Gets or sets the Color
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the CounterToday
        /// </summary>
        public string CounterToday { get; set; }

        /// <summary>
        /// Gets or sets the Options
        /// </summary>
        public string Options { get; set; }

        /// <summary>
        /// Gets or sets the Usage
        /// </summary>
        public string Usage { get; set; }

        /// <summary>
        /// Gets or sets the SetPoint
        /// </summary>
        public string SetPoint { get; set; }

        /// <summary>
        /// Gets or sets the InternalState
        /// </summary>
        public string InternalState { get; set; }

        /// <summary>
        /// Gets or sets the Rain
        /// </summary>
        public string Rain { get; set; }

        /// <summary>
        /// Gets or sets the RainRate
        /// </summary>
        public string RainRate { get; set; }

        #endregion
    }
}
