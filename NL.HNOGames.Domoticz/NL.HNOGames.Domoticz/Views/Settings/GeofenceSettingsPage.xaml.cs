using System;
using Xamarin.Forms;
using System.Linq;
using System.Reactive.Linq;
using System.Collections.Generic;
using NL.HNOGames.Domoticz.Resources;
using Rg.Plugins.Popup.Services;
using NL.HNOGames.Domoticz.Views.Dialog;
using Acr.UserDialogs;
using System.Threading.Tasks;
using Plugin.CrossPlacePicker;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    public partial class GeofenceSettingsPage
    {
        private readonly List<Models.GeofenceModel> _oListSource;
        private Models.GeofenceModel _oSelectedGeofenceCommand;

        /// <summary>
        /// Constructor of Geofence page
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

        /// <summary>
        /// Check if this feature is supported for your device
        /// </summary>
        /// <returns></returns>
        private bool ValidateGeofenceSupported()
        {
            /*
            if (!this.Geofence.IsSupported)
            {
                App.ShowToast("Geofence recognition is not supported for this device at this moment..");
                return false;
            }
            else
            {
                var status = await Geofence.RequestPermission();
                if (status != GeofenceRecognizerStatus.Available)
                {
                    App.AddLog("Permission denied for Geofence recognition");
                    App.ShowToast("Don't have the permission for the mic");
                    return false;
                }
            }
            */
            return true;
        }

        /// <summary>
        /// Add new Geofence Command to system
        /// </summary>
        private async void ToolbarItem_Activated(object sender, EventArgs e)
        {
            if (!ValidateGeofenceSupported())
            {
                swEnableGeofence.IsToggled = false;
                return;
            }

            try
            {
                var result = await CrossPlacePicker.Current.Display();
                if (result != null)
                {
                    await AddNewRecordAsync(result.PlaceId, result);
                }
            }
            catch (Exception ex)
            {
                if (ex != null &&
                    !String.IsNullOrEmpty(ex.Message))
                {
                    App.ShowToast(ex.ToString());
                    App.AddLog(ex.Message);
                }
            }
        }

        /// <summary>
        /// Create new Geofence object
        /// </summary>
        private async Task AddNewRecordAsync(string GeofenceID, Plugin.CrossPlacePicker.Abstractions.Places location)
        {
            if (location == null)
                return;

            var r = await UserDialogs.Instance.PromptAsync(AppResources.radius,
                    inputType: InputType.Number);

            int radius = 500;
            await Task.Delay(500);
            if (r.Ok)
                radius = Convert.ToInt32(r.Text);
            else
                return;

            var GeofenceObject = new Models.GeofenceModel()
            {
                Id = GeofenceID,
                Name = location.Name,
                Latitude = location.Coordinates.Latitude,
                Longitude = location.Coordinates.Longitude,
                Address = location.Address,
                Radius = radius,
                Enabled = true,
            };

            _oListSource.Add(GeofenceObject);
            SaveAndRefresh();
        }

        /// <summary>
        /// Delete a Geofence Command from the list
        /// </summary>
        private void btnDeleteButton_Clicked(object sender, EventArgs e)
        {
            var oGeofenceCommand = (Models.GeofenceModel)((Button)sender).BindingContext;
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
        private async void btnConnect_Clicked(object sender, EventArgs e)
        {
            _oSelectedGeofenceCommand = (Models.GeofenceModel)((Button)sender).BindingContext;
            var oSwitchPopup = new SwitchPopup();
            oSwitchPopup.DeviceSelectedMethod += DelegateMethod;
            await PopupNavigation.PushAsync(oSwitchPopup);
        }

        /// <summary>
        /// Connect device to Geofence Command
        /// </summary>
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
    }
}