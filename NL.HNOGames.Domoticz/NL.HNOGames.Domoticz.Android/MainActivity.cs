using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using ZXing.Mobile;
using Plugin.FirebasePushNotification;
using Plugin.Fingerprint;
using Plugin.InAppBilling;
using Plugin.CurrentActivity;
using Android.Content;
using NL.HNOGames.Domoticz.Droid.Helpers;
using Plugin.Permissions;
using Android.Runtime;
using Firebase;

namespace NL.HNOGames.Domoticz.Droid
{
    [Activity(Label = "@string/app_name", Icon = "@mipmap/ic_launcher", RoundIcon = "@mipmap/ic_launcher_round", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) =>
                {
                    System.Diagnostics.Debug.WriteLine(cert.GetSerialNumberString());
                    System.Diagnostics.Debug.WriteLine(cert.Issuer);
                    System.Diagnostics.Debug.WriteLine(cert.Subject);
                    return true;
                };

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            UserDialogs.Init(this);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            CrossFingerprint.SetCurrentActivityResolver(() => CrossCurrentActivity.Current.Activity);
            CrossFingerprint.SetDialogFragmentType<CustomFingerprintDialogFragment>();
            Plugin.InputKit.Platforms.Droid.Config.Init(this, savedInstanceState);

            FirebasePushNotificationManager.ProcessIntent(this, Intent);
            OxyPlot.Xamarin.Forms.Platform.Android.PlotViewRenderer.Init();
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            MobileBarcodeScanner.Initialize(Application);
            XamEffects.Droid.Effects.Init();

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);

            LoadApplication(new App());
            FirebaseApp.InitializeApp(this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            global::ZXing.Net.Mobile.Forms.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            InAppBillingImplementation.HandleActivityResult(requestCode, resultCode, data);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            FirebasePushNotificationManager.ProcessIntent(this, intent);
        }
    }
}