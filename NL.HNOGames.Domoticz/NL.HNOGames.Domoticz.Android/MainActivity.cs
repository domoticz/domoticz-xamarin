using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using FFImageLoading.Forms.Droid;
using System;
using ZXing.Mobile;
using Plugin.Fingerprint;
using Plugin.InAppBilling;
using Plugin.CurrentActivity;
using Android.Content;
using System.Net;
using NL.HNOGames.Domoticz.Droid.Helpers;

#if NETFX_CORE
[assembly: Xamarin.Forms.Platform.WinRT.ExportRenderer(typeof(Xamarin.RangeSlider.Forms.RangeSlider), typeof(Xamarin.RangeSlider.Forms.RangeSliderRenderer))]
#else
[assembly: Xamarin.Forms.ExportRenderer(typeof(Xamarin.RangeSlider.Forms.RangeSlider), typeof(Xamarin.RangeSlider.Forms.RangeSliderRenderer))]
#endif

namespace NL.HNOGames.Domoticz.Droid
{
   [Activity(Label = "@string/app_name", Icon = "@mipmap/ic_launcher", RoundIcon = "@mipmap/ic_launcher_round", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
   public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
   {
      protected override void OnCreate(Bundle bundle)
      {
         System.Net.ServicePointManager.ServerCertificateValidationCallback +=
             (sender, cert, chain, sslPolicyErrors) =>
             {
                System.Diagnostics.Debug.WriteLine(cert.GetSerialNumberString());
                System.Diagnostics.Debug.WriteLine(cert.Issuer);
                System.Diagnostics.Debug.WriteLine(cert.Subject);
                return true;
             };

         CachedImageRenderer.Init(true);
         UserDialogs.Init(this);
         Rg.Plugins.Popup.Popup.Init(this, bundle);

         CrossCurrentActivity.Current.Init(this, bundle);
         CrossFingerprint.SetCurrentActivityResolver(() => CrossCurrentActivity.Current.Activity);
         CrossFingerprint.SetDialogFragmentType<CustomFingerprintDialogFragment>();

         OxyPlot.Xamarin.Forms.Platform.Android.PlotViewRenderer.Init();
         ZXing.Net.Mobile.Forms.Android.Platform.Init();
         MobileBarcodeScanner.Initialize(Application);

         TabLayoutResource = Resource.Layout.Tabbar;
         ToolbarResource = Resource.Layout.Toolbar;

         base.OnCreate(bundle);

         global::Xamarin.Forms.Forms.Init(this, bundle);

         LoadApplication(new App(null));
      }

      public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
      {
         base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
         global::ZXing.Net.Mobile.Forms.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
         Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
      }

      protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
      {
         base.OnActivityResult(requestCode, resultCode, data);
         InAppBillingImplementation.HandleActivityResult(requestCode, resultCode, data);
      }
   }
}