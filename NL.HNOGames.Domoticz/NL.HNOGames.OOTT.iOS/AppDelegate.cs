
using FFImageLoading.Forms;
using Foundation;
using UIKit;
using System;
using MTiRate;
using PushNotification.Plugin;
using NL.HNOGames.Domoticz.Helpers;
using UserNotifications;
using Firebase.CloudMessaging;
using Firebase.Core;
using System.Net;
using Plugin.Fingerprint;

namespace NL.HNOGames.OOTT.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) =>
                {
                    System.Diagnostics.Debug.WriteLine(cert.GetSerialNumberString());
                    System.Diagnostics.Debug.WriteLine(cert.Issuer);
                    System.Diagnostics.Debug.WriteLine(cert.Subject);
                    return true;
                };

            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
            UIApplication.SharedApplication.SetStatusBarHidden(false, false);

            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes()
            {
                Font = UIFont.FromName("HelveticaNeue-Light", (nfloat)20f),
                TextColor = UIColor.White
            });

            global::Xamarin.Forms.Forms.Init();

            Plugin.InputKit.Platforms.iOS.Config.Init();
            iRate.SharedInstance.DaysUntilPrompt = 10;
            iRate.SharedInstance.UsesUntilPrompt = 20;
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();

            SlideOverKit.iOS.SlideOverKit.Init();
            Plugin.InputKit.Platforms.iOS.Config.Init();
            Rg.Plugins.Popup.Popup.Init();
            OxyPlot.Xamarin.Forms.Platform.iOS.PlotViewRenderer.Init();

            LoadApplication(new Domoticz.App(SaveToken));
            CrossPushNotification.Initialize<CrossPushNotificationListener>();

            App.Configure();
            // Register your app for remote notifications.
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = this;

                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
                {
                    Console.WriteLine(granted);
                });
            }
            else
            {
                // iOS 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            UIApplication.SharedApplication.RegisterForRemoteNotifications();
            Messaging.SharedInstance.Delegate = this;
            
            // To connect with FCM. FCM manages the connection, closing it
            // when your app goes into the background and reopening it 
            // whenever the app is foregrounded.
            Messaging.SharedInstance.ShouldEstablishDirectChannel = true;
            return base.FinishedLaunching(app, options);
        }

        [Export("messaging:didReceiveRegistrationToken:")]
        public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
        {
            // Monitor token generation: To be notified whenever the token is updated.
            Domoticz.App.AddLog($"Firebase registration token: {fcmToken}");
            registerAsync(fcmToken);
                Domoticz.App.AddLog("GCM: No token available");
            tokenUploaded = true;
        }

        /// <summary>
        /// Save token to domoticz
        /// </summary>
        private void SaveToken()
        {
            var token = Messaging.SharedInstance.FcmToken;
            if (token != null)
                registerAsync(token);
            else
                Domoticz.App.AddLog("GCM: No token available");
            tokenUploaded = true;
        }

        private async void registerAsync(string token)
        {
            tokenUploaded = true;
            Domoticz.App.AddLog(string.Format("GCM: Push Notification - Device Registered - Token : {0}", token));
            string Id = UsefulBits.GetDeviceID();
            tokenUploaded = await Domoticz.App.ApiService.RegisterDevice(Id, token);
            if (tokenUploaded)
                Domoticz.App.AddLog("GCM: Device registered on Domoticz");
            else
                Domoticz.App.AddLog("GCM: Device not registered on Domoticz");
        }
        
        public override void RegisteredForRemoteNotifications (UIApplication application, NSData deviceToken)
        {
        	Messaging.SharedInstance.ApnsToken = deviceToken;
        }

        private bool tokenUploaded = false;

        // iOS 9 <=, fire when recieve notification foreground
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            if (Domoticz.App.AppSettings.EnableNotifications)
            {
                Domoticz.App.AddLog("GCM: Notification received");
                Domoticz.App.AddLog(userInfo.ToString());
                var body = WebUtility.UrlDecode(userInfo["gcm.notification.message"] as NSString);
                var title = WebUtility.UrlDecode(userInfo["gcm.notification.title"] as NSString);
                if (string.IsNullOrEmpty(title))
                    title = WebUtility.UrlDecode(userInfo["gcm.notification.subject"] as NSString);
                if (string.Compare(title, body, true) == 0)
                    title = "Domoticz";
                if (application.ApplicationState == UIApplicationState.Active)
                    debugAlert(title, body);
            }
        }

        [Export("messaging:didReceiveMessage:")]
        public void DidReceiveMessage(Messaging messaging, RemoteMessage remoteMessage)
        {
            Domoticz.App.AddLog("GCM: Notification received");
            Domoticz.App.AddLog(remoteMessage.AppData.ToString());
            //// Handle Data messages for iOS 10 and above.
            //if (Domoticz.App.AppSettings.EnableNotifications)
            //{
            //    Domoticz.App.AddLog("GCM: Notification received");
            //    Domoticz.App.AddLog(remoteMessage.AppData.ToString());
            //    var body = WebUtility.UrlDecode(remoteMessage.AppData["gcm.notification.message"] as NSString);
            //    var title = WebUtility.UrlDecode(remoteMessage.AppData["gcm.notification.title"] as NSString);
            //    if (string.IsNullOrEmpty(title))
            //        title = WebUtility.UrlDecode(remoteMessage.AppData["gcm.notification.subject"] as NSString);
            //    if (string.Compare(title, body, true) == 0)
            //        title = "Domoticz";
            //    debugAlert(title, body);
            //}
        }

        // iOS 10, fire when recieve notification foreground
        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            if (Domoticz.App.AppSettings.EnableNotifications)
            {
                Domoticz.App.AddLog("GCM: Notification received");
                var title = WebUtility.UrlDecode(notification.Request.Content.Title);
                var body = WebUtility.UrlDecode(notification.Request.Content.Body);
                debugAlert(title, body);
            }
        }

        private void debugAlert(string title, string message)
        {
            var alert = new UIAlertView(title ?? "Title", message ?? "Message", null, "Cancel", "OK");
            alert.Show();
        }

    }
}
