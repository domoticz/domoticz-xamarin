using NL.HNOGames.Domoticz.Controls;
using NL.HNOGames.Domoticz.Resources;
using NL.HNOGames.Domoticz.Views.Dialog;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    /// <summary>
    /// Defines the <see cref="GeofenceSettingsPage" />
    /// </summary>
    public partial class GeofenceSettingsPage
    {
        #region Variables

        /// <summary>
        /// Defines the _oListSource
        /// </summary>
        private readonly List<Models.GeofenceModel> _oListSource;

        /// <summary>
        /// Defines the _oSelectedGeofenceCommand
        /// </summary>
        private Models.GeofenceModel _oSelectedGeofenceCommand;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GeofenceSettingsPage"/> class.
        /// </summary>
        public GeofenceSettingsPage()
        {
            _oSelectedGeofenceCommand = null;
            InitializeComponent();

            App.ShowToast(AppResources.title_add_location);
            swEnableGeofence.IsToggled = App.AppSettings.GeofenceEnabled;
            swEnableGeofence.Toggled += (sender, args) =>
            {
                App.AppSettings.GeofenceEnabled = swEnableGeofence.IsToggled;
                if (swEnableGeofence.IsToggled)
                {
                    if (!ValidateGeofenceSupported())
                        swEnableGeofence.IsToggled = false;
                }
            };

            _oListSource = App.AppSettings.GeofenceCommands;
            if (_oListSource != null)
                listView.ItemsSource = _oListSource;
        }

        #endregion

        #region Public

        /// <summary>
        /// Connect device to Geofence Command
        /// </summary>
        /// <param name="device">The device<see cref="Models.Device"/></param>
        /// <param name="password">The password<see cref="String"/></param>
        /// <param name="value">The value<see cref="String"/></param>
        public void DelegateMethod(Models.Device device, String password, String value)
        {
            App.ShowToast("Connecting " + _oSelectedGeofenceCommand.Name + " with switch " + device.Name);
            _oSelectedGeofenceCommand.SwitchIDX = device.idx;
            _oSelectedGeofenceCommand.SwitchName = device.Name;
            _oSelectedGeofenceCommand.Value = value;
            _oSelectedGeofenceCommand.SwitchPassword = password;
            _oSelectedGeofenceCommand.IsScene = device.IsScene;
            _oSelectedGeofenceCommand.IsScene = device.IsScene;
            SaveAndRefresh();
        }

        #endregion

        #region Private

        /// <summary>
        /// Check if this feature is supported for your device
        /// </summary>
        /// <returns></returns>
        private bool ValidateGeofenceSupported()
        {
            return true;
        }

        /// <summary>
        /// Add new Geofence Command to system
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void ToolbarItem_Activated(object sender, EventArgs e)
        {
            if (!ValidateGeofenceSupported())
            {
                swEnableGeofence.IsToggled = false;
                return;
            }
        }

        /// <summary>
        /// Create new Geofence object
        /// </summary>
        /// <param name="GeofenceID">The GeofenceID<see cref="string"/></param>
        private void AddNewRecord(string GeofenceID)
        {
        }

        /// <summary>
        /// Delete a Geofence Command from the list
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnDeleteButton_Clicked(object sender, EventArgs e)
        {
            var oGeofenceCommand = (Models.GeofenceModel)((TintedCachedImage)sender).BindingContext;
            App.ShowToast(AppResources.something_deleted.Replace("%1$s", oGeofenceCommand.Name));
            _oListSource.Remove(oGeofenceCommand);
            SaveAndRefresh();
        }

        /// <summary>
        /// Save and refresh the list of Geofence Commands
        /// </summary>
        private void SaveAndRefresh()
        {
            App.AppSettings.GeofenceCommands = _oListSource;
            listView.ItemsSource = null;
            listView.ItemsSource = _oListSource;
        }

        /// <summary>
        /// Connect device to Geofence Command
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void btnConnect_Clicked(object sender, EventArgs e)
        {
            _oSelectedGeofenceCommand = (Models.GeofenceModel)((TintedCachedImage)sender).BindingContext;
            var oSwitchPopup = new SwitchPopup();
            oSwitchPopup.DeviceSelectedMethod += DelegateMethod;
            await PopupNavigation.Instance.PushAsync(oSwitchPopup);
        }

        #endregion
    }
}
