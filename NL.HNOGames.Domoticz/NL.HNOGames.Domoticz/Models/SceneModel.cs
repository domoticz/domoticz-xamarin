using Humanizer;
using Humanizer.Configuration;
using Humanizer.DateTimeHumanizeStrategy;
using NL.HNOGames.Domoticz.Data;
using NL.HNOGames.Domoticz.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Models
{
    public class SceneModel
    {
        public int ActTime { get; set; }
        public bool AllowWidgetOrdering { get; set; }
        public string ServerTime { get; set; }
        public string Sunrise { get; set; }
        public string Sunset { get; set; }
        public Scene[] result { get; set; }
        public string status { get; set; }
        public string title { get; set; }
    }

    public class Scene
    {
        public String Icon
        {
            get
            {
                String selectedIcon = IconService.getDrawableIcon(ConstantValues.Device.Scene.Type.SCENE.ToLower(),
                        null,
                        null,
                        false,
                        false,
                        null);
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

        public String FavoriteIcon
        {
            get
            {
                return Favorite > 0 ? "ic_star.png" : "ic_star_border.png";
            }
        }
        public String FavoriteIconTintColor
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
                    if (String.Compare(Status, ConstantValues.Device.Blind.State.OFF, StringComparison.OrdinalIgnoreCase) == 0 ||
                        String.Compare(Status, ConstantValues.Device.Blind.State.CLOSED, StringComparison.OrdinalIgnoreCase) == 0)
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
        public String LastUpdateDescription
        {
            get
            {
                DateTime d = DateTime.ParseExact(LastUpdate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                //System.Diagnostics.Debug.WriteLine("LastUpdateDescription" + (DateTime.Now - d).Humanize(2) + " | " + d.ToString() + " | " + DateTime.Now.ToString());
                return String.Format("{0}: {1} ago", AppResources.last_update, (DateTime.Now - d).Humanize(2));
            }
        }

        public String TypeDescription
        {
            get
            {
                return String.Format("{0}: {1}", AppResources.type, Type);
            }
        }

        public string Description { get; set; }
        public int Favorite { get; set; }
        public string LastUpdate { get; set; }
        public string Name { get; set; }
        public string OffAction { get; set; }
        public string OnAction { get; set; }
        public bool Protected { get; set; }
        public string Status { get; set; }
        public string Timers { get; set; }
        public string Type { get; set; }
        public bool UsedByCamera { get; set; }
        public string idx { get; set; }
    }

}
