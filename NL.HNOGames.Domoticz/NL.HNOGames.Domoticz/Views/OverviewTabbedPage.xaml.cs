using NL.HNOGames.Domoticz.Resources;
using NL.HNOGames.Domoticz.ViewModels;
using Plugin.NFC;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.SpeechRecognition;
using Shiny;
using Shiny.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace NL.HNOGames.Domoticz.Views
{
    /// <summary>
    /// Defines the <see cref="OverviewTabbedPage" />
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OverviewTabbedPage
    {
        #region Variables

        /// <summary>
        /// Defines the _viewModel
        /// </summary>
        private readonly OverviewViewModel _viewModel;

        /// <summary>
        /// Defines the listener
        /// </summary>
        private static IDisposable listener = null;

        /// <summary>
        /// Defines the EmptyDialogShown
        /// </summary>
        public static bool EmptyDialogShown = false;

        /// <summary>
        /// Defines the _settingsOpened
        /// </summary>
        private bool _settingsOpened;

        /// <summary>
        /// Defines the _showPlans
        /// </summary>
        private bool _showPlans = true;

        /// <summary>
        /// Defines the _showQRCode
        /// </summary>
        private bool _showQRCode = true;

        /// <summary>
        /// Defines the _showNFC
        /// </summary>
        private bool _showNFC = true;

        /// <summary>
        /// Defines the _showSpeech
        /// </summary>
        private bool _showSpeech = true;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="OverviewTabbedPage"/> class.
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

        #endregion

        #region Private

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
        private async void tiSpeechCode_Activated()
        {
            try
            {
                if (await ValidateSpeechRecognition())
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
        /// Check if this feature is supported for your device
        /// </summary>
        /// <returns></returns>
        private bool ValidateNFCPermissions()
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
        /// Check if this feature is supported for your device
        /// </summary>
        /// <returns></returns>
        private async Task<bool> ValidateCameraPermissions()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                if (status != PermissionStatus.Granted)
                {
                    var newStatus = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                    if (!newStatus.ContainsKey(Permission.Camera))
                        return false;
                    status = newStatus[Permission.Camera];
                    if (status != PermissionStatus.Granted)
                    {
                        App.AddLog("Permission denied for camera");
                        App.ShowToast("Don't have the permission for the camera, check your app permission settings.");
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                //Something went wrong
            }
            return true;
        }

        /// <summary>
        /// Check if this feature is supported for your device
        /// </summary>
        /// <returns></returns>
        private async Task<bool> ValidateSpeechRecognition()
        {
            if (!CrossSpeechRecognition.Current.IsSupported)
            {
                App.ShowToast("Speech recognition is not supported for this device at this moment..");
                return false;
            }
            else
            {
                try
                {
                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Microphone);
                    if (status != PermissionStatus.Granted)
                    {
                        var newStatus = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Microphone);
                        if (!newStatus.ContainsKey(Permission.Microphone))
                            return false;
                        status = newStatus[Permission.Microphone];
                        if (status != PermissionStatus.Granted)
                        {
                            App.AddLog("Permission denied for speech recognition");
                            App.ShowToast("Don't have the permission for the microphone, check your app permission settings.");
                            return false;
                        }
                    }
                }
                catch (Exception)
                {
                    //Something went wrong
                }
            }
            return true;
        }

        /// <summary>
        /// Scan NFC
        /// </summary>
        private void tiNFC_Activated()
        {
            if (!App.AppSettings.NFCEnabled)
                return;
            if (ValidateNFCPermissions())
            {
                if (Device.RuntimePlatform == Device.Android)
                    App.ShowLoading(AppResources.nfc_register, AppResources.cancel, null);

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
            processNFC(tagInfo.SerialNumber);
        }

        /// <summary>
        /// Scan QR Code
        /// </summary>
        private async void tiQRCode_Activated()
        {
            if (!App.AppSettings.QRCodeEnabled)
                return;
            if (await ValidateCameraPermissions())
            {
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
                    const BarcodeFormat expectedFormat = BarcodeFormat.QR_CODE;
                    var opts = new ZXing.Mobile.MobileBarcodeScanningOptions
                    {
                        PossibleFormats = new List<BarcodeFormat> { expectedFormat }
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
        }

        /// <summary>
        /// The processNFC
        /// </summary>
        /// <param name="NFCId">The NFCId<see cref="string"/></param>
        private async void processNFC(string NFCId)
        {
            if (Device.RuntimePlatform == Device.Android)
                App.HideLoading();
            var nfcTag = App.AppSettings.NFCTags.FirstOrDefault(o => o.Id == NFCId);
            if (nfcTag != null && nfcTag.Enabled)
            {
                App.AddLog("NFC tag Found: " + NFCId);
                App.ShowToast(AppResources.nfc + " " + nfcTag.Name);
                _ = await App.ApiService.HandleSwitch(nfcTag.SwitchIDX, nfcTag.SwitchPassword, -1, nfcTag.Value,
                nfcTag.IsScene);
                App.SetMainPage();
            }
            else
            {
                App.AddLog("NFC tag not registered: " + NFCId);
                App.ShowToast(nfcTag == null ? AppResources.nfc_tag_found : AppResources.enable_nfc);
            }
        }

        /// <summary>
        /// The processQrId
        /// </summary>
        /// <param name="qrCodeId">The qrCodeId<see cref="string"/></param>
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
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void TiMore_Clicked(object sender, EventArgs e)
        {
            var actions = new List<string>();
            if (_showPlans)
                actions.Add(AppResources.title_plans);
            if (_showNFC)
                actions.Add(AppResources.nfc);
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
            else if (result == AppResources.nfc)
                tiNFC_Activated();
            else if (result == AppResources.Speech)
                tiSpeechCode_Activated();
            else if (result == AppResources.wizard_button_settings)
                OnSettingsClick();
        }

        #endregion

        /// <summary>
        /// On Appearing of this screen
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_settingsOpened)
            {
                _settingsOpened = false;
                BreakingSettingsChanged();
            }
            else
                _viewModel.RefreshPlansCommand.Execute(null);

            _showQRCode = App.AppSettings.QRCodeEnabled;
            _showNFC = App.AppSettings.NFCEnabled;
            _showSpeech = App.AppSettings.SpeechEnabled;
            await SetupGeofencesAsync();
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

        /// <summary>
        /// Setup geofences
        /// </summary>
        private async Task SetupGeofencesAsync()
        {
            if (App.AppSettings.GeofenceEnabled)
            {
                App.AddLog("Recreating all registed geofences");
                var geofences = ShinyHost.Resolve<IGeofenceManager>();
                await geofences.StopAllMonitoring();
                foreach (var geofence in App.AppSettings.Geofences)
                {
                    if (geofence.Enabled)
                    {
                        App.AddLog($"Started monitoring for Geofence {geofence.Name}");
                        await geofences.StartMonitoring(new GeofenceRegion(
                               geofence.Id,
                               new Position(geofence.Latitude, geofence.Longitude),
                               Distance.FromMeters(geofence.Radius)
                           )
                        {
                            NotifyOnEntry = true,
                            NotifyOnExit = true,
                            SingleUse = false
                        });
                    }
                }
            }
        }
    }
}
