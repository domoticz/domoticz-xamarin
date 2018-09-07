using Humanizer;
using Newtonsoft.Json;
using NL.HNOGames.Domoticz.Data;
using NL.HNOGames.Domoticz.Helpers;
using NL.HNOGames.Domoticz.Resources;
using System;
using System.Globalization;

namespace NL.HNOGames.Domoticz.Models
{
   public class DevicesModel
   {
      public int ActTime { get; set; }
      public string ServerTime { get; set; }
      public string Sunrise { get; set; }
      public string Sunset { get; set; }
      public Device[] result { get; set; }
      public string status { get; set; }
      public string title { get; set; }
   }

   public class Device
   {
      private string _levelNames;

      public bool ShowExtraData { get; set; } = true;

      public bool IsDashboard { get; set; } = false;

      public int RowHeight
      {
         get
         {
            return ViewHelper.GetTemplateHeight(this, IsDashboard);
         }
      }

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

      public string FavoriteIcon
      {
         get
         {
            return Favorite > 0 ? "ic_star.png" : "ic_star_border.png";
         }
      }
      public string FavoriteIconTintColor
      {
         get
         {
            return Favorite > 0 ? "#FFEB3B" : "#999999";
         }
      }

      public bool StatusBoolean
      {
         get
         {
            try
            {
               bool statusBoolean = true;
               if (string.Compare(Status, ConstantValues.Device.Blind.State.OFF, StringComparison.OrdinalIgnoreCase) == 0 ||
                   string.Compare(Status, ConstantValues.Device.Blind.State.CLOSED, StringComparison.OrdinalIgnoreCase) == 0)
                  statusBoolean = false;
               return statusBoolean;
            }
            catch (Exception)
            {
               return false;
            }
         }
      }

      public bool FavoriteBoolean
      {
         get
         {
            return Favorite == 1;
         }
      }

      public string LastUpdateDescription
      {
         get
         {
            var d = DateTime.ParseExact(LastUpdate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            return string.Format("{0}: {1}", AppResources.last_update, (DateTime.Now - d).Humanize(2));
         }
      }

      public string[] LevelNamesArray
      {
         get
         {
            if (string.IsNullOrWhiteSpace(LevelNames))
               return null;
            return LevelNames.Split('|');
         }
      }

      public int LevelNamesIndex
      {
         get
         {
            if (LevelInt > 0)
               return LevelInt / 10;
            return 0;
         }
      }

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
                  dataText += !string.IsNullOrEmpty(dataText) ? ", " + AppResources.humidity + ": " + this.HumidityStatus : AppResources.humidity + ": " + this.HumidityStatus;

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
               return string.Format("{0}: {1}", AppResources.status, dataText);
            }
            catch (Exception ex)
            {
               App.AddLog(ex.Message);
               return "";
            }
         }
      }

      public float AddjMulti { get; set; }
      public float AddjMulti2 { get; set; }
      public float AddjValue { get; set; }
      public float AddjValue2 { get; set; }
      public int BatteryLevel { get; set; }
      public int? CustomImage { get; set; }
      public string Data { get; set; }
      public string Description { get; set; }
      public int Favorite { get; set; }
      public int HardwareID { get; set; }
      public string HardwareName { get; set; }
      public string HardwareType { get; set; }
      public int HardwareTypeVal { get; set; }
      public bool HaveTimeout { get; set; }
      public string ID { get; set; }
      public string Image { get; set; }
      public string LastUpdate { get; set; }
      public string Name { get; set; }
      public string Notifications { get; set; }
      public string PlanID { get; set; }
      public int[] PlanIDs { get; set; }
      public bool Protected { get; set; }
      public bool ShowNotifications { get; set; }
      public string SignalLevel { get; set; }
      public string SubType { get; set; }
      public string Timers { get; set; }
      public string Type { get; set; }
      public string TypeImg { get; set; }
      public int Unit { get; set; }
      public int Used { get; set; }
      public string XOffset { get; set; }
      public string YOffset { get; set; }
      public string idx { get; set; }
      public string DewPoint { get; set; }
      public int Humidity { get; set; }
      public string HumidityStatus { get; set; }
      public float? Temp { get; set; }
      public float? Barometer { get; set; }
      public int? Forecast { get; set; }
      public string ForecastStr { get; set; }
      public string forecast_url { get; set; }
      public float? Chill { get; set; }
      public float? Direction { get; set; }
      public string DirectionStr { get; set; }
      public string Gust { get; set; }
      public string Speed { get; set; }
      public bool HaveDimmer { get; set; }
      public bool HaveGroupCmd { get; set; }
      public bool IsSubDevice { get; set; }
      public int Level { get; set; }
      public int LevelInt { get; set; }
      public int MaxDimLevel { get; set; }
      public string Status { get; set; }
      public string StrParam1 { get; set; }
      public string StrParam2 { get; set; }
      public string SwitchType { get; set; }
      public int SwitchTypeVal { get; set; }
      public bool UsedByCamera { get; set; }
      public string LevelActions { get; set; }
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
      public bool LevelOffHidden { get; set; }
      public int SelectorStyle { get; set; }
      public string UVI { get; set; }
      public float Visibility { get; set; }
      public string Counter { get; set; }
      public string Color { get; set; }
      public string CounterToday { get; set; }
      public string Options { get; set; }
      public string Usage { get; set; }
      public string SetPoint { get; set; }
      public string InternalState { get; set; }
      public string Rain { get; set; }
      public string RainRate { get; set; }
   }
}
