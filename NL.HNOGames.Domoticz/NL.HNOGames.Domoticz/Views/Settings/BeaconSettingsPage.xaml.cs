using NL.HNOGames.Domoticz.Controls;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using NL.HNOGames.Domoticz.Views.Dialog;
using Shiny;
using Shiny.Beacons;
using Shiny.Locations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    /// <summary>
    /// Defines the <see cref="BeaconSettingsPage" />
    /// </summary>
    public partial class BeaconSettingsPage
    {
        #region Variables

        /// <summary>
        /// Defines the _oListSource
        /// </summary>
        private List<Models.BeaconModel> _oListSource;

        /// <summary>
        /// Defines the _oSelectedBeaconCommand
        /// </summary>
        private BeaconModel _oSelectedBeaconCommand;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BeaconSettingsPage"/> class.
        /// </summary>
        public BeaconSettingsPage()
        {
            _oSelectedBeaconCommand = null;
            InitializeComponent();

            App.ShowToast(AppResources.title_add_beacon);
            swEnableBeacon.IsToggled = App.AppSettings.BeaconEnabled;
            swEnableBeacon.Toggled += (sender, args) =>
            {
                App.AppSettings.BeaconEnabled = swEnableBeacon.IsToggled;
            };

            swEnableBeaconNotifications.IsToggled = App.AppSettings.BeaconNotificationsEnabled;
            swEnableBeaconNotifications.Toggled += (sender, args) =>
            {
                App.AppSettings.BeaconNotificationsEnabled = swEnableBeaconNotifications.IsToggled;
            };

            _oListSource = App.AppSettings.Beacons;
            if (_oListSource == null)
                _oListSource = new List<BeaconModel>();
            listView.ItemsSource = _oListSource;
        }

        #endregion

        #region Public

        /// <summary>
        /// Connect device to Beacon Command
        /// </summary>
        /// <param name="device">The device<see cref="Models.Device"/></param>
        /// <param name="password">The password<see cref="String"/></param>
        /// <param name="value">The value<see cref="String"/></param>
        public void DelegateMethod(Models.Device device, String password, String value)
        {
            App.ShowToast("Connecting " + _oSelectedBeaconCommand.Name + " with switch " + device.Name);
            _oSelectedBeaconCommand.SwitchIDX = device.idx;
            _oSelectedBeaconCommand.SwitchName = device.Name;
            _oSelectedBeaconCommand.Value = value;
            _oSelectedBeaconCommand.SwitchPassword = password;
            _oSelectedBeaconCommand.IsScene = device.IsScene;
            _oSelectedBeaconCommand.IsScene = device.IsScene;
            SaveAndRefresh();
        }

        #endregion

        #region Private

        /// <summary>
        /// Check if this feature is supported for your device
        /// </summary>
        /// <returns></returns>
        private async Task<bool> ValidateBeaconSupportedAsync()
        {
            try
            {
                var Beacons = ShinyHost.Resolve<IBeaconManager>();
                var status = await Beacons.RequestAccess(true);
                if (status != AccessState.Available)
                {
                    App.AddLog("Permission denied for getting your beacons");
                    App.ShowToast("Don't have the permission for getting your beacons, check your app permission settings.");
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
        /// Add new Beacon Command to system
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void ToolbarItem_Activated(object sender, EventArgs e)
        {
            if (!await ValidateBeaconSupportedAsync())
            {
                swEnableBeacon.IsToggled = false;
                return;
            }
            if (_oListSource.Count >= 20)
                App.ShowToast(AppResources.beacon_max_error);
            else
                await Navigation.PushAsync(new BeaconConfigPage(OnBeaconConfigged));
        }

        /// <summary>
        /// On beacon configed
        /// </summary>
        private void OnBeaconConfigged(BeaconModel newBeacon)
        {
            if (newBeacon == null)
                return;

            _oListSource.Add(newBeacon);
            App.ShowToast(AppResources.noswitchselected_explanation_beacons);
            SaveAndRefresh();
        }

        /// <summary>
        /// Delete a Beacon Command from the list
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnDeleteButton_Clicked(object sender, EventArgs e)
        {
            var oBeaconCommand = (Models.BeaconModel)((TintedCachedImage)sender).BindingContext;
            App.ShowToast(AppResources.something_deleted.Replace("%1$s", oBeaconCommand.Name));
            _oListSource.Remove(oBeaconCommand);
            SaveAndRefresh();
        }

        /// <summary>
        /// Save and refresh the list of Beacon Commands
        /// </summary>
        private void SaveAndRefresh()
        {
            App.AppSettings.Beacons = _oListSource;
            listView.ItemsSource = null;
            listView.ItemsSource = _oListSource;
        }

        /// <summary>
        /// Connect device to Beacon Command
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void btnConnect_Clicked(object sender, EventArgs e)
        {
            _oSelectedBeaconCommand = (Models.BeaconModel)((TintedCachedImage)sender).BindingContext;
            var oSwitchPopup = new SwitchPopup();
            oSwitchPopup.DeviceSelectedMethod += DelegateMethod;
            await Navigation.PushAsync(oSwitchPopup);
        }

        #endregion
    }
}
