using NL.HNOGames.Domoticz.Resources;
using NL.HNOGames.Domoticz.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace NL.HNOGames.Domoticz.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OverviewTabbedPage
    {
        private readonly OverviewViewModel _viewModel;
        public static bool EmptyDialogShown = false;

        public OverviewTabbedPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new OverviewViewModel();
        }

        /// <summary>
        /// On Appearing of this screen
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_settingsOpened)
            {
                _settingsOpened = false;
                BreakingSettingsChanged();
            }
            else
                _viewModel.RefreshPlansCommand.Execute(null);

            if (!App.AppSettings.QRCodeEnabled)
                ToolbarItems.Remove(tiQRCode);
            else if (!ToolbarItems.Contains(tiQRCode))
                ToolbarItems.Insert(1, tiQRCode);
        }

        /// <summary>
        /// Show action sheet with plans
        /// </summary>
        private async void OnShowPlansClick(object o, EventArgs e)
        {
            if (_viewModel.Plans == null || _viewModel.Plans.Count <= 0) return;
            var selectedPlanName = await DisplayActionSheet(AppResources.title_plans, AppResources.cancel, null,
                _viewModel.Plans.Select(p => p.Name).ToArray());
            var selectedPlan = _viewModel.Plans.FirstOrDefault(q => q.Name == selectedPlanName);
            if (selectedPlan != null)
            {
                await Navigation.PushAsync(new DashboardPage(DashboardViewModel.ScreenTypeEnum.Switches, selectedPlan));
            }
        }

        private bool _settingsOpened;

        /// <summary>
        /// Show all settings
        /// </summary>
        private async void OnSettingsClick(object o, EventArgs e)
        {
            _settingsOpened = true;
            await Navigation.PushAsync(new Settings.SettingsPage(new Command(BreakingSettingsChanged)));
        }

        /// <summary>
        /// Refresh mainscreen
        /// </summary>
        private static void BreakingSettingsChanged()
        {
            App.SetMainPage();
            App.RestartFirebase();
        }

        /// <summary>
        /// Scan QR Code
        /// </summary>
        private async void tiQRCode_Activated(object sender, EventArgs e)
        {
            if (!App.AppSettings.QRCodeEnabled)
                return;

            var expectedFormat = ZXing.BarcodeFormat.QR_CODE;
            var opts = new ZXing.Mobile.MobileBarcodeScanningOptions
            {
                PossibleFormats = new List<ZXing.BarcodeFormat> {expectedFormat}
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
                        var qrCode = App.AppSettings.QRCodes.FirstOrDefault(o => o.Id == qrCodeId);
                        if (qrCode != null && qrCode.Enabled)
                        {
                            App.AddLog("QR Code ID Found: " + qrCodeId);
                            App.ShowToast(AppResources.qrcode + " " + qrCode.Name);
                            await App.ApiService.HandleSwitch(qrCode.SwitchIDX, qrCode.SwitchPassword, -1, qrCode.Value,
                                qrCode.IsScene);
                            App.SetMainPage();
                        }
                        else
                        {
                            App.AddLog("QR Code ID not registered: " + qrCodeId);
                            App.ShowToast(
                                qrCode == null ? AppResources.qrcode_new_found : AppResources.qr_code_disabled);
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