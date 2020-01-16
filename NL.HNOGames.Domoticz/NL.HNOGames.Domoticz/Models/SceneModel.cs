using Humanizer;
using NL.HNOGames.Domoticz.Data;
using NL.HNOGames.Domoticz.Resources;
using System;
using System.Globalization;

namespace NL.HNOGames.Domoticz.Models
{
    /// <summary>
    /// Defines the <see cref="SceneModel" />
    /// </summary>
    public class SceneModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the ActTime
        /// </summary>
        public int ActTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether AllowWidgetOrdering
        /// </summary>
        public bool AllowWidgetOrdering { get; set; }

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
        public Scene[] result { get; set; }

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
    /// Defines the <see cref="Scene" />
    /// </summary>
    public class Scene
    {
        #region Properties

        /// <summary>
        /// Gets the Icon
        /// </summary>
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
        /// Gets the FavoriteIcon
        /// </summary>
        public String FavoriteIcon
        {
            get
            {
                return Favorite > 0 ? "ic_star.png" : "ic_star_border.png";
            }
        }

        /// <summary>
        /// Gets the FavoriteIconTintColor
        /// </summary>
        public String FavoriteIconTintColor
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
        public String LastUpdateDescription
        {
            get
            {
                DateTime d = DateTime.ParseExact(LastUpdate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                //App.AddLog("LastUpdateDescription" + (DateTime.Now - d).Humanize(2) + " | " + d.ToString() + " | " + DateTime.Now.ToString());
                return string.Format("{0}: {1} ago", AppResources.last_update, (DateTime.Now - d).Humanize(2));
            }
        }

        /// <summary>
        /// Gets the TypeDescription
        /// </summary>
        public String TypeDescription
        {
            get
            {
                return string.Format("{0}: {1}", AppResources.type, Type);
            }
        }

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Favorite
        /// </summary>
        public int Favorite { get; set; }

        /// <summary>
        /// Gets or sets the LastUpdate
        /// </summary>
        public string LastUpdate { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the OffAction
        /// </summary>
        public string OffAction { get; set; }

        /// <summary>
        /// Gets or sets the OnAction
        /// </summary>
        public string OnAction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Protected
        /// </summary>
        public bool Protected { get; set; }

        /// <summary>
        /// Gets or sets the Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the Timers
        /// </summary>
        public string Timers { get; set; }

        /// <summary>
        /// Gets or sets the Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether UsedByCamera
        /// </summary>
        public bool UsedByCamera { get; set; }

        /// <summary>
        /// Gets or sets the idx
        /// </summary>
        public string idx { get; set; }

        #endregion
    }
}
