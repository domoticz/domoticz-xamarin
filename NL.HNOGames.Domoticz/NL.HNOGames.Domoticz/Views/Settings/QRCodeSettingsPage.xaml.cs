using System;

using NL.HNOGames.Domoticz.ViewModels;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Plugin.Share;
using NL.HNOGames.Domoticz.Resources;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    public partial class QRCodeSettingsPage : ContentPage
    {
        public QRCodeSettingsPage()
        {
            InitializeComponent();

            swEnableQRCode.IsToggled = App.AppSettings.QRCodeEnabled;
            swEnableQRCode.Toggled += (sender, args) =>
            {
                App.AppSettings.QRCodeEnabled = swEnableQRCode.IsToggled;
            };
        }

        private async Task ToolbarItem_Activated(object sender, EventArgs e)
        {
            var scanPage = new ZXingScannerPage();
            scanPage.OnScanResult += (result) =>
            {
                scanPage.IsScanning = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    await DisplayAlert("Scanned Barcode", result.Text, "OK");
                });
            };
            await Navigation.PushAsync(scanPage);
        }
    }
}