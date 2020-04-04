using Foundation;
using UIKit;
using System;
using UserNotifications;

namespace NL.HNOGames.Domoticz.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate
    {
        #region Variables

        /// <summary>
        /// Object reference to the application that we loaded
        /// </summary>
        private App application;

        #endregion

        /// <summary>
        /// This method is invoked when the application has loaded and is ready to run. In this
        /// method you should instantiate the window, load the UI into it and then make the window
        /// visible.
        /// </summary>
        /// <param name="uiApplication"></param>
        /// <param name="launchOptions"></param>
        /// <returns></returns>
        /// <remarks>You have 17 seconds to return from this method, or iOS will terminate your application.</remarks>
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {

#if DEBUG
            System.AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                // Set a breakpoint here to catch the unhandled exceptions
                if (e.ExceptionObject is System.Exception ex)
                {
                    System.Console.WriteLine($"UNHANDLEDEXCEPTION: {ex.Message}");
                    System.Console.WriteLine($"UNHANDLEDEXCEPTION: {ex.StackTrace}");
                }
            };
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                // Set a breakpoint here to catch the unhandled exceptions
                System.Console.WriteLine($"UNHANDLEDEXCEPTION: {e.Exception.Message}");
                System.Console.WriteLine($"UNHANDLEDEXCEPTION: {e.Exception.StackTrace}");
            };
#endif

            SetStatusBar();
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;

            global::Xamarin.Forms.Forms.Init();

            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            Plugin.InputKit.Platforms.iOS.Config.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();

            SlideOverKit.iOS.SlideOverKit.Init();
            Plugin.InputKit.Platforms.iOS.Config.Init();
            Rg.Plugins.Popup.Popup.Init();
            OxyPlot.Xamarin.Forms.Platform.iOS.PlotViewRenderer.Init();
            XamEffects.iOS.Effects.Init();
            //FirebasePushNotificationManager.Initialize(launchOptions, true);

            application = new App();
            LoadApplication(application);
            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        /// <summary>
        /// Set status bar
        /// </summary>
        private static void SetStatusBar()
        {
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
            UIApplication.SharedApplication.SetStatusBarHidden(false, false);
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes()
            {
                Font = UIFont.FromName("HelveticaNeue-Light", (nfloat)20f),
                TextColor = UIColor.White
            });
        }
    }
}
