using System;

using NL.HNOGames.Domoticz.ViewModels;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Plugin.Share;
using NL.HNOGames.Domoticz.Resources;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;
using System.Linq;
using NL.HNOGames.Domoticz.Models;
using Acr.UserDialogs;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    public partial class QRCodeSettingsPage : ContentPage
    {
        private List<QRCodeModel> oListSource = new List<QRCodeModel>();

        public QRCodeSettingsPage()
        {
            InitializeComponent();

            App.ShowToast(AppResources.qrcode_register);
            swEnableQRCode.IsToggled = App.AppSettings.QRCodeEnabled;
            swEnableQRCode.Toggled += (sender, args) =>
            {
                App.AppSettings.QRCodeEnabled = swEnableQRCode.IsToggled;
            };

            oListSource = App.AppSettings.QRCodes;
            if (oListSource != null)
                listView.ItemsSource = oListSource;
        }

        /// <summary>
        /// QRCode selected (select connected switch)
        /// </summary>
        async Task OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
        }

        /// <summary>
        /// Add new qr code to system
        /// </summary>
        private async Task ToolbarItem_Activated(object sender, EventArgs e)
        {
            var scanPage = new ZXingScannerPage();
            scanPage.OnScanResult += (result) =>
            {
                scanPage.IsScanning = false;
                Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    String QRCodeID = result.Text.ToString().GetHashCode()+"";
                    if (oListSource.Any(o => String.Compare(o.Id, QRCodeID, StringComparison.OrdinalIgnoreCase) == 0))
                    {
                        App.ShowToast(AppResources.qrcode_exists);
                    }
                    else
                    {
                        var r = await UserDialogs.Instance.PromptAsync(AppResources.qrcode_name, inputType: InputType.Name);
                        await Task.Delay(500);
                        if (r.Ok)
                        {
                            var name = r.Text;
                            if (!String.IsNullOrEmpty(name))
                            {
                                App.ShowToast(AppResources.qrcode_saved + " " + result.Text);
                                QRCodeModel QRCode = new QRCodeModel()
                                {
                                    Id = QRCodeID,
                                    Name = name,
                                };

                                oListSource.Add(QRCode);
                                SaveAndRefresh();
                            }
                        }
                    }
                });
            };
            await Navigation.PushAsync(scanPage);
        }

        /// <summary>
        /// Delete a QR Code from the list
        /// </summary>
        private void btnDeleteButton_Clicked(object sender, EventArgs e)
        {
            QRCodeModel oQRCode = (QRCodeModel)((Button)sender).BindingContext;
            App.ShowToast(AppResources.something_deleted + " " + oQRCode.Name);
            oListSource.Remove(oQRCode);
            SaveAndRefresh();
        }

        /// <summary>
        /// Save and refresh the list of QR Codes
        /// </summary>
        private void SaveAndRefresh()
        {
            App.AppSettings.QRCodes = oListSource;
            listView.ItemsSource = null;
            listView.ItemsSource = oListSource;
        }

        //QR Code enabled/disabled
        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            Switch swEnabled = ((Switch)sender);
            QRCodeModel oQRCode = (QRCodeModel)swEnabled.Parent.BindingContext;
            oQRCode.Enabled = swEnabled.IsToggled;
            SaveAndRefresh();
        }
    }
}