using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using NL.HNOGames.Domoticz.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace NL.HNOGames.Domoticz.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OverviewTabbedPage : TabbedPage
    {
        OverviewViewModel viewModel;
        public static bool EmptyDialogShown = false;

        public OverviewTabbedPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new OverviewViewModel();
        }

        /// <summary>
        /// On Appearing of this screen
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (settingsOpened)
            {
                settingsOpened = false;
                BreakingSettingsChanged();
            }
            else
                viewModel.RefreshPlansCommand.Execute(null);

            if (!App.AppSettings.QRCodeEnabled)
                ToolbarItems.Remove(tiQRCode);
            else if(!ToolbarItems.Contains(tiQRCode))
                ToolbarItems.Insert(1, tiQRCode);
        }

        /// <summary>
        /// Show action sheet with plans
        /// </summary>
        public async void OnShowPlansClick(object o, EventArgs e)
        {
            if (viewModel.Plans != null && viewModel.Plans.Count > 0)
            {
                List<String> plans = new List<string>();
                foreach (Plan p in viewModel.Plans)
                    plans.Add(p.Name);

                var selectedPlanName = await DisplayActionSheet(AppResources.title_plans, AppResources.cancel, null, plans.ToArray());
                var selectedPlan = viewModel.Plans.Where(q => q.Name == selectedPlanName).FirstOrDefault();
                if (selectedPlan != null)
                {
                    await Navigation.PushAsync(new DashboardPage(DashboardViewModel.ScreenType.Switches, selectedPlan));
                }
            }
        }

        private bool settingsOpened = false;
        /// <summary>
        /// Show all settings
        /// </summary>
        public async void OnSettingsClick(object o, EventArgs e)
        {
            settingsOpened = true;
            await Navigation.PushAsync(new Settings.SettingsPage(new Command(async () => await BreakingSettingsChanged())));
        }

        /// <summary>
        /// Refresh mainscreen
        /// </summary>
        async Task BreakingSettingsChanged()
        {
            App.SetMainPage();
            App.RestartFirebase();
        }

        /// <summary>
        /// Scan QR Code
        /// </summary>
        private async Task tiQRCode_Activated(object sender, EventArgs e)
        {
            if (!App.AppSettings.QRCodeEnabled)
                return;

            var expectedFormat = ZXing.BarcodeFormat.QR_CODE;
            var opts = new ZXing.Mobile.MobileBarcodeScanningOptions
            {
                PossibleFormats = new List<ZXing.BarcodeFormat> { expectedFormat }
            };
            System.Diagnostics.Debug.WriteLine("Scanning " + expectedFormat);

            var scanPage = new ZXingScannerPage(opts);
            scanPage.OnScanResult += (result) =>
            {
                scanPage.IsScanning = false;

                Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    try
                    {
                        String QRCodeID = result.Text.ToString().GetHashCode() + "";
                        var QRCode = App.AppSettings.QRCodes.Where(o => o.Id == QRCodeID).FirstOrDefault();
                        if(QRCode != null && QRCode.Enabled)
                        {
                            App.AddLog("QR Code ID Found: " + QRCodeID);
                            App.ShowToast(AppResources.qrcode + " " + QRCode.Name);
                            await App.ApiService.HandleSwitch(QRCode.SwitchIDX, QRCode.SwitchPassword, -1, QRCode.Value, QRCode.IsScene);
                        }
                        else
                        {
                            App.AddLog("QR Code ID not registered: " + QRCodeID);
                            if (QRCode == null)
                                App.ShowToast(AppResources.qrcode_new_found);
                            else
                                App.ShowToast(AppResources.qr_code_disabled);
                        }
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
}
