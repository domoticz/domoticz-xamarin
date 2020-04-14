using Acr.UserDialogs;
using NL.HNOGames.Domoticz.Controls;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using NL.HNOGames.Domoticz.Views.Dialog;
using Plugin.NFC;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZXing;
using ZXing.Net.Mobile.Forms;
using static System.Net.Mime.MediaTypeNames;
using Device = Xamarin.Forms.Device;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    /// <summary>
    /// Defines the <see cref="NFCSettingsPage" />
    /// </summary>
    public partial class NFCSettingsPage
    {
        #region Variables

        /// <summary>
        /// Defines the _oListSource
        /// </summary>
        private readonly List<NFCModel> _oListSource;

        /// <summary>
        /// Defines the _oSelectedNFC
        /// </summary>
        private NFCModel _oSelectedNFC;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NFCSettingsPage"/> class.
        /// </summary>
        public NFCSettingsPage()
        {
            InitializeComponent();

            App.ShowToast(AppResources.nfc_register);
            swEnableNFC.IsToggled = App.AppSettings.NFCEnabled;
            swEnableNFC.Toggled += (sender, args) => { App.AppSettings.NFCEnabled = swEnableNFC.IsToggled; };

            _oListSource = App.AppSettings.NFCTags;
            if (_oListSource != null)
                listView.ItemsSource = _oListSource;
        }

        #endregion

        #region Private

        /// <summary>
        /// Add new qr code to system
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void ToolbarItem_Activated(object sender, EventArgs e)
        {
            if (!App.AppSettings.NFCEnabled)
                return;
            if (ValidatePermissions())
            {
                App.ShowLoading(AppResources.nfc_register);
                CrossNFC.Current.OnMessageReceived += OnNFCMessageReceived;
                CrossNFC.Current.OnTagDiscovered += OnNFCDiscovered;
                CrossNFC.Current.StartListening();
            }
        }

        private void OnNFCDiscovered(ITagInfo tagInfo, bool format)
        {
            OnNFCMessageReceived(tagInfo);
        }

        private void OnNFCMessageReceived(ITagInfo tagInfo)
        {
            App.HideLoading();

            try
            {
                var textId = tagInfo.SerialNumber;
                if (_oListSource.Any(o => string.Compare(o.Id, textId, StringComparison.OrdinalIgnoreCase) == 0))
                    App.ShowToast(AppResources.nfc_exists);
                else
                {
                    AddNewRecord(textId);
                    App.ShowToast(AppResources.nfc_tag_found);
                }
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }
        }

        /// <summary>
        /// Check if this feature is supported for your device
        /// </summary>
        /// <returns></returns>
        private bool ValidatePermissions()
        {
            try
            {
                if (!CrossNFC.Current.IsAvailable)
                {

                    App.ShowToast(AppResources.nfc_not_supported);
                    return false;
                }

                if (!CrossNFC.Current.IsEnabled)
                {
                    App.ShowToast("NFC Reader not enabled. Please turn it on in the settings.");
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
        /// Create new NFC object
        /// </summary>
        /// <param name="NFCId">The NFCId<see cref="string"/></param>
        private void AddNewRecord(string NFCId)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var r = await UserDialogs.Instance.PromptAsync(AppResources.nfc_tag_name, inputType: InputType.Name);
                await Task.Delay(500);

                if (!r.Ok) return;
                var name = r.Text;
                if (string.IsNullOrEmpty(name)) return;
                App.ShowToast(AppResources.nfc_saved + " " + name);
                var NFC = new NFCModel()
                {
                    Id = NFCId,
                    Name = name,
                    Enabled = true,
                };
                _oListSource.Add(NFC);
                SaveAndRefresh();
            });
        }

        /// <summary>
        /// Delete a QR Code from the list
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnDeleteButton_Clicked(object sender, EventArgs e)
        {
            var oNFC = (NFCModel)((TintedCachedImage)sender).BindingContext;
            App.ShowToast(AppResources.something_deleted.Replace("%1$s", oNFC.Name));
            _oListSource.Remove(oNFC);
            SaveAndRefresh();
        }

        /// <summary>
        /// Save and refresh the list of QR Codes
        /// </summary>
        private void SaveAndRefresh()
        {
            App.AppSettings.NFCTags = _oListSource;
            listView.ItemsSource = null;
            listView.ItemsSource = _oListSource;
        }

        /// <summary>
        /// Connect device to QR Code
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void btnConnect_Clicked(object sender, EventArgs e)
        {
            _oSelectedNFC = (NFCModel)((TintedCachedImage)sender).BindingContext;
            var oSwitchPopup = new SwitchPopup();
            oSwitchPopup.DeviceSelectedMethod += DelegateMethod;
            await Navigation.PushAsync(oSwitchPopup);
        }

        /// <summary>
        /// Connect device to QR Code
        /// </summary>
        /// <param name="device">The device<see cref="Models.Device"/></param>
        /// <param name="password">The password<see cref="string"/></param>
        /// <param name="value">The value<see cref="string"/></param>
        private void DelegateMethod(Models.Device device, string password, string value)
        {
            App.ShowToast("Connecting " + _oSelectedNFC.Name + " with switch " + device.Name);
            _oSelectedNFC.SwitchIDX = device.idx;
            _oSelectedNFC.SwitchName = device.Name;
            _oSelectedNFC.Value = value;
            _oSelectedNFC.SwitchPassword = password;
            _oSelectedNFC.IsScene = device.IsScene;
            SaveAndRefresh();
        }

        /// <summary>
        /// On disappearing
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            try
            {
                CrossNFC.Current.StopListening();
            }
            catch (Exception) { }
        }

        #endregion
    }
}
