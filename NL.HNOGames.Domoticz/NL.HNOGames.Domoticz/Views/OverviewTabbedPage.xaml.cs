using NL.HNOGames.Domoticz.Resources;
using NL.HNOGames.Domoticz.ViewModels;
using Plugin.SpeechRecognition;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace NL.HNOGames.Domoticz.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OverviewTabbedPage
    {
        private readonly OverviewViewModel _viewModel;
        private static IDisposable listener = null;
        public static bool EmptyDialogShown = false;
        private bool _settingsOpened;

        private bool _showPlans = true;
        private bool _showQRCode = true;
        private bool _showSpeech = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public OverviewTabbedPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new OverviewViewModel();
            _viewModel.PlansLoadedMethod += () =>
            {
                if (_viewModel.Plans == null || _viewModel.Plans.Count <= 0)
                    _showPlans = false;
            };
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
                _showQRCode = false;
            if (!App.AppSettings.SpeechEnabled)
                _showSpeech = false;
        }

        /// <summary>
        /// Disappearing
        /// </summary>
        protected override void OnDisappearing()
        {
            if (App.ConnectionService != null)
                App.ConnectionService.CleanClient();
            base.OnDisappearing();
        }

        /// <summary>
        /// Show action sheet with plans
        /// </summary>
        private async void OnShowPlans()
        {
            if (_viewModel.Plans == null || _viewModel.Plans.Count <= 0) return;
            var selectedPlanName = await DisplayActionSheet(AppResources.title_plans, AppResources.cancel, null,
                _viewModel.Plans.Select(p => p.Name).ToArray());
            var selectedPlan = _viewModel.Plans.FirstOrDefault(q => q.Name == selectedPlanName);
            if (selectedPlan != null)
                await Navigation.PushAsync(new DashboardPage(DashboardViewModel.ScreenTypeEnum.Switches, selectedPlan));
        }

        /// <summary>
        /// Show all settings
        /// </summary>
        private async void OnSettingsClick()
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
        }

        /// <summary>
        /// Speech Recognition
        /// </summary>
        private void tiSpeechCode_Activated()
        {
            try
            {
                App.ShowLoading(AppResources.Speech);
                listener = CrossSpeechRecognition
                    .Current
                    .ListenUntilPause()
                    .Subscribe(async phrase =>
                    {
                        App.HideLoading();
                        App.ShowToast(phrase);
                        if (listener != null)
                            listener.Dispose();
                        try
                        {
                            var speechID = phrase.GetHashCode() + "";
                            var speechCommand = App.AppSettings.SpeechCommands.FirstOrDefault(o => o.Id == speechID);
                            if (speechCommand != null && speechCommand.Enabled)
                            {
                                App.AddLog("Speech Command Found: " + speechCommand);
                                App.ShowToast(AppResources.Speech + " " + speechCommand.Name + ": " + AppResources.switch_toggled + " " + speechCommand.SwitchName);
                                await App.ApiService.HandleSwitch(speechCommand.SwitchIDX, speechCommand.SwitchPassword, -1, speechCommand.Value,
                                    speechCommand.IsScene);
                                App.SetMainPage();
                            }
                            else
                            {
                                App.AddLog("Speech Command not registered: " + speechID);
                                App.ShowToast(
                                    speechCommand == null ? AppResources.Speech_found : AppResources.Speech_disabled);
                            }
                        }
                        catch (Exception ex)
                        {
                            App.AddLog(ex.Message);
                        }
                    });
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
                App.ShowToast(ex.Message);
                if (listener != null)
                    listener.Dispose();
            }
        }

        /// <summary>
        /// Scan QR Code
        /// </summary>
        private async void tiQRCode_Activated()
        {
            if (!App.AppSettings.QRCodeEnabled)
                return;

            var expectedFormat = BarcodeFormat.QR_CODE;
            var opts = new ZXing.Mobile.MobileBarcodeScanningOptions
            {
                PossibleFormats = new List<ZXing.BarcodeFormat> { expectedFormat }
            };
            System.Diagnostics.Debug.WriteLine("Scanning " + expectedFormat);

            if (Device.RuntimePlatform == Device.iOS)
            {
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                var result = await scanner.Scan();

                if (result == null) return;
                try
                {
                    var qrCodeId = result.Text.GetHashCode() + "";
                    processQrId(qrCodeId);
                }
                catch (Exception ex)
                {
                    App.AddLog(ex.Message);
                }
            }
            else
            {
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
                            processQrId(qrCodeId);
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

        private async void processQrId(string qrCodeId)
        {
            var qrCode = App.AppSettings.QRCodes.FirstOrDefault(o => o.Id == qrCodeId);
            if (qrCode != null && qrCode.Enabled)
            {
                App.AddLog("QR Code ID Found: " + qrCodeId);
                App.ShowToast(AppResources.qrcode + " " + qrCode.Name);
                _ = await App.ApiService.HandleSwitch(qrCode.SwitchIDX, qrCode.SwitchPassword, -1, qrCode.Value,
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

        /// <summary>
        /// Show a dialog with all options
        /// </summary>
        private async void TiMore_Clicked(object sender, EventArgs e)
        {
            var actions = new List<string>();
            if (_showPlans)
                actions.Add(AppResources.title_plans);
            if (_showQRCode)
                actions.Add(AppResources.qrcode);
            if (_showSpeech)
                actions.Add(AppResources.Speech);
            actions.Add(AppResources.wizard_button_settings);
            var result = await DisplayActionSheet("", AppResources.cancel, null, actions.ToArray());
            if (result == AppResources.title_plans)
                OnShowPlans();
            else if (result == AppResources.qrcode)
                tiQRCode_Activated();
            else if (result == AppResources.Speech)
                tiSpeechCode_Activated();
            else if (result == AppResources.wizard_button_settings)
                OnSettingsClick();
        }
    }
}