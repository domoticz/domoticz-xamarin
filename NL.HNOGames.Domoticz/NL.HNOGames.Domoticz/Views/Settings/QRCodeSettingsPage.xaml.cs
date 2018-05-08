using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using NL.HNOGames.Domoticz.Resources;
using System.Linq;
using NL.HNOGames.Domoticz.Models;
using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using NL.HNOGames.Domoticz.Views.Dialog;
using ZXing;
using ZXing.Net.Mobile.Forms;
using Device = Xamarin.Forms.Device;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    public partial class QrCodeSettingsPage
    {
        private readonly List<QRCodeModel> _oListSource;
        private QRCodeModel _oSelectedQrCode;

        /// <summary>
        /// Constructor of QRCode page
        /// </summary>
        public QrCodeSettingsPage()
        {
            InitializeComponent();

            App.ShowToast(AppResources.qrcode_register);
            swEnableQRCode.IsToggled = App.AppSettings.QRCodeEnabled;
            swEnableQRCode.Toggled += (sender, args) => { App.AppSettings.QRCodeEnabled = swEnableQRCode.IsToggled; };

            _oListSource = App.AppSettings.QRCodes;
            if (_oListSource != null)
                listView.ItemsSource = _oListSource;
        }

        /// <summary>
        /// Add new qr code to system
        /// </summary>
        private async void ToolbarItem_Activated(object sender, EventArgs e)
        {
            if (!App.AppSettings.QRCodeEnabled)
                return;

            if (Device.RuntimePlatform == Device.iOS)
            {
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                var result = await scanner.Scan();

                if (result == null) return;
                try
                {
                    var qrCodeId = result.Text.GetHashCode() + "";
                    if (_oListSource.Any(o => string.Compare(o.Id, qrCodeId, StringComparison.OrdinalIgnoreCase) == 0))
                        App.ShowToast(AppResources.qrcode_exists);
                    else
                        AddNewRecord(qrCodeId);
                }
                catch (Exception ex)
                {
                    App.AddLog(ex.Message);
                }
            }
            else
            {
                const BarcodeFormat expectedFormat = BarcodeFormat.QR_CODE;
                var opts = new ZXing.Mobile.MobileBarcodeScanningOptions
                {
                    PossibleFormats = new List<BarcodeFormat> {expectedFormat}
                };
                System.Diagnostics.Debug.WriteLine("Scanning " + expectedFormat);

                var scanPage = new ZXingScannerPage(opts);
                scanPage.OnScanResult += (result) =>
                {
                    scanPage.IsScanning = false;

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Navigation.PopAsync();
                        try
                        {
                            var qrCodeId = result.Text.GetHashCode() + "";
                            if (_oListSource.Any(
                                o => string.Compare(o.Id, qrCodeId, StringComparison.OrdinalIgnoreCase) == 0))
                                App.ShowToast(AppResources.qrcode_exists);
                            else
                                AddNewRecord(qrCodeId);
                        }
                        catch (Exception ex)
                        {
                            App.AddLog(ex.Message);
                        }
                    });
                };

                await Navigation.PushAsync(scanPage);
            }
        }

        /// <summary>
        /// Create new QR Code object
        /// </summary>
        private void AddNewRecord(string qrCodeId)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var r = await UserDialogs.Instance.PromptAsync(AppResources.qrcode_name, inputType: InputType.Name);
                await Task.Delay(500);

                if (!r.Ok) return;
                var name = r.Text;
                if (string.IsNullOrEmpty(name)) return;
                App.ShowToast(AppResources.qrcode_saved + " " + name);
                var qrCode = new QRCodeModel()
                {
                    Id = qrCodeId,
                    Name = name,
                    Enabled = true,
                };
                _oListSource.Add(qrCode);
                SaveAndRefresh();
            });
        }

        /// <summary>
        /// Delete a QR Code from the list
        /// </summary>
        private void btnDeleteButton_Clicked(object sender, EventArgs e)
        {
            var oQrCode = (QRCodeModel) ((Button) sender).BindingContext;
            App.ShowToast(AppResources.something_deleted.Replace("%1$s", oQrCode.Name));
            _oListSource.Remove(oQrCode);
            SaveAndRefresh();
        }

        /// <summary>
        /// Save and refresh the list of QR Codes
        /// </summary>
        private void SaveAndRefresh()
        {
            App.AppSettings.QRCodes = _oListSource;
            listView.ItemsSource = null;
            listView.ItemsSource = _oListSource;
        }

        /// <summary>
        /// Connect device to QR Code
        /// </summary>
        private async void btnConnect_Clicked(object sender, EventArgs e)
        {
            _oSelectedQrCode = (QRCodeModel) ((Button) sender).BindingContext;
            var oSwitchPopup = new SwitchPopup();
            oSwitchPopup.DeviceSelectedMethod += DelegateMethod;
            await PopupNavigation.Instance.PushAsync(oSwitchPopup);
        }

        /// <summary>
        /// Connect device to QR Code
        /// </summary>
        private void DelegateMethod(Models.Device device, string password, string value)
        {
            App.ShowToast("Connecting " + _oSelectedQrCode.Name + " with switch " + device.Name);
            _oSelectedQrCode.SwitchIDX = device.idx;
            _oSelectedQrCode.SwitchName = device.Name;
            _oSelectedQrCode.Value = value;
            _oSelectedQrCode.SwitchPassword = password;
            _oSelectedQrCode.IsScene = device.IsScene;
            _oSelectedQrCode.IsScene = device.IsScene;
            SaveAndRefresh();
        }
    }
}