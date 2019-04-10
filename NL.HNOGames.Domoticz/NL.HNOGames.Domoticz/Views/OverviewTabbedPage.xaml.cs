using NL.HNOGames.Domoticz.Resources;
using NL.HNOGames.Domoticz.ViewModels;
using Plugin.SpeechRecognition;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
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
               ToolbarItems.Remove(tiPlans);
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
            ToolbarItems.Remove(tiQRCode);
         else if (!ToolbarItems.Contains(tiQRCode))
            ToolbarItems.Insert(1, tiQRCode);
         if (!App.AppSettings.SpeechEnabled)
            ToolbarItems.Remove(tiSpeechCode);
         else if (!ToolbarItems.Contains(tiSpeechCode))
            ToolbarItems.Insert(1, tiSpeechCode);
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
      private async void OnShowPlansClick(object o, EventArgs e)
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
      /// Speech Recognition
      /// </summary>
      private void tiSpeechCode_Activated(object sender, EventArgs e)
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
      private async void tiQRCode_Activated(object sender, EventArgs e)
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