using NL.HNOGames.Domoticz.Controls;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using NL.HNOGames.Domoticz.Views.Dialog;
using Rg.Plugins.Popup.Services;
using Shiny;
using Shiny.Locations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        private List<Models.GeofenceModel> _oListSource;

        /// <summary>
        /// Defines the _oSelectedGeofenceCommand
        /// </summary>
        private GeofenceModel _oSelectedGeofenceCommand;

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
            };

            swEnableGeofenceNotifications.IsToggled = App.AppSettings.GeofenceNotificationsEnabled;
            swEnableGeofenceNotifications.Toggled += (sender, args) =>
            {
                App.AppSettings.GeofenceNotificationsEnabled = swEnableGeofenceNotifications.IsToggled;
            };
            
            _oListSource = App.AppSettings.Geofences;
            if (_oListSource == null)
                _oListSource = new List<GeofenceModel>();
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
        private async Task<bool> ValidateGeofenceSupportedAsync()
        {
            try
            {
                var geofences = ShinyHost.Resolve<IGeofenceManager>();
                var status = await geofences.RequestAccess();
                if (status != AccessState.Available)
                {
                    App.AddLog("Permission denied for getting your location");
                    App.ShowToast("Don't have the permission for getting your location, check your app permission settings.");
                    return false;
                }
            }
            catch (Exception)
            {
                //Something went wrong
            }
            return true;
        }

        /// <summary>
        /// Add new Geofence Command to system
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void ToolbarItem_Activated(object sender, EventArgs e)
        {
            if (!await ValidateGeofenceSupportedAsync())
            {
                swEnableGeofence.IsToggled = false;
                return;
            }
            await Navigation.PushAsync(new LocationPickerPage(OnLocationChoosen));
        }

        /// <summary>
        /// On location choosen
        /// </summary>
        private void OnLocationChoosen(int radius, string address, Xamarin.Forms.Maps.Position location)
        {
            if (location == null)
                return;
            if (string.IsNullOrEmpty(address))
                address = $"{location.Latitude} {location.Longitude}";
            var geofence = new GeofenceModel()
            {
                Id = address.GetHashCode().ToString(),
                Name = address,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Enabled = true,
            };

            _oListSource.Add(geofence);

            SaveAndRefresh();
            App.ShowToast(AppResources.noSwitchSelected_explanation_Geofences);
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
            App.AppSettings.Geofences = _oListSource;
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
