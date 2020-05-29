using NL.HNOGames.Domoticz.Controls;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using NL.HNOGames.Domoticz.Views.Dialog;
using Shiny;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    /// <summary>
    /// Defines the <see cref="BluetoothSettingsPage" />
    /// </summary>
    public partial class BluetoothSettingsPage
    {
        #region Variables

        /// <summary>
        /// Defines the _oListSource
        /// </summary>
        private List<Models.BluetoothModel> _oListSource;

        /// <summary>
        /// Defines the _oSelectedBluetoothCommand
        /// </summary>
        private BluetoothModel _oSelectedBluetoothCommand;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BluetoothSettingsPage"/> class.
        /// </summary>
        public BluetoothSettingsPage()
        {
            _oSelectedBluetoothCommand = null;
            InitializeComponent();

            App.ShowToast(AppResources.bluetooth);
            swEnableBluetooth.IsToggled = App.AppSettings.BluetoothEnabled;
            swEnableBluetooth.Toggled += (sender, args) =>
            {
                App.AppSettings.BluetoothEnabled = swEnableBluetooth.IsToggled;
            };

            swEnableBluetoothNotifications.IsToggled = App.AppSettings.BluetoothNotificationsEnabled;
            swEnableBluetoothNotifications.Toggled += (sender, args) =>
            {
                App.AppSettings.BluetoothNotificationsEnabled = swEnableBluetoothNotifications.IsToggled;
            };

            _oListSource = App.AppSettings.Bluetooths;
            if (_oListSource == null)
                _oListSource = new List<BluetoothModel>();
            listView.ItemsSource = _oListSource;
        }

        #endregion

        #region Public

        /// <summary>
        /// Connect device to Bluetooth Command
        /// </summary>
        /// <param name="device">The device<see cref="Models.Device"/></param>
        /// <param name="password">The password<see cref="String"/></param>
        /// <param name="value">The value<see cref="String"/></param>
        public void DelegateMethod(Models.Device device, String password, String value)
        {
            App.ShowToast("Connecting " + _oSelectedBluetoothCommand.Name + " with switch " + device.Name);
            _oSelectedBluetoothCommand.SwitchIDX = device.idx;
            _oSelectedBluetoothCommand.SwitchName = device.Name;
            _oSelectedBluetoothCommand.Value = value;
            _oSelectedBluetoothCommand.SwitchPassword = password;
            _oSelectedBluetoothCommand.IsScene = device.IsScene;
            _oSelectedBluetoothCommand.IsScene = device.IsScene;
            SaveAndRefresh();
        }

        #endregion

        #region Private

        /// <summary>
        /// Check if this feature is supported for your device
        /// </summary>
        /// <returns></returns>
        private async Task<bool> ValidateBluetoothSupportedAsync()
        {
            try
            {
                //var bluetooths = ShinyHost.Resolve<IBluetoothManager>();
                //var status = await bluetooths.RequestAccess(true);
                //if (status != AccessState.Available)
                //{
                //    App.AddLog("Permission denied for getting your Bluetooths");
                //    App.ShowToast("Don't have the permission for getting your Bluetooths, check your app permission settings.");
                //    return false;
                //}
            }
            catch (Exception)
            {
                //Something went wrong
            }
            return true;
        }

        /// <summary>
        /// Add new Bluetooth Command to system
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void ToolbarItem_Activated(object sender, EventArgs e)
        {
            if (!await ValidateBluetoothSupportedAsync())
            {
                swEnableBluetooth.IsToggled = false;
                return;
            }

            // TODO: Show available bluetooth devices
        }

        /// <summary>
        /// On Bluetooth configed
        /// </summary>
        private void OnBluetoothConfigged(BluetoothModel newBluetooth)
        {
            if (newBluetooth == null)
                return;

            _oListSource.Add(newBluetooth);
            App.ShowToast(AppResources.noSwitchSelected_explanation_bluetooth);
            SaveAndRefresh();
        }

        /// <summary>
        /// Delete a Bluetooth Command from the list
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnDeleteButton_Clicked(object sender, EventArgs e)
        {
            var oBluetoothCommand = (Models.BluetoothModel)((TintedCachedImage)sender).BindingContext;
            App.ShowToast(AppResources.something_deleted.Replace("%1$s", oBluetoothCommand.Name));
            _oListSource.Remove(oBluetoothCommand);
            SaveAndRefresh();
        }

        /// <summary>
        /// Save and refresh the list of Bluetooth Commands
        /// </summary>
        private void SaveAndRefresh()
        {
            App.AppSettings.Bluetooths = _oListSource;
            listView.ItemsSource = null;
            listView.ItemsSource = _oListSource;
        }

        /// <summary>
        /// Connect device to Bluetooth Command
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void btnConnect_Clicked(object sender, EventArgs e)
        {
            _oSelectedBluetoothCommand = (Models.BluetoothModel)((TintedCachedImage)sender).BindingContext;
            var oSwitchPopup = new SwitchPopup();
            oSwitchPopup.DeviceSelectedMethod += DelegateMethod;
            await Navigation.PushAsync(oSwitchPopup);
        }

        #endregion
    }
}
