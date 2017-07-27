
using FFImageLoading.Forms.Touch;
using Foundation;
using UIKit;

using System;
using MTiRate;
using PushNotification.Plugin;
using NL.HNOGames.Domoticz.Helpers;
using UserNotifications;
using Firebase.CloudMessaging;
using System.Diagnostics;

namespace NL.HNOGames.Domoticz.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
            UIApplication.SharedApplication.SetStatusBarHidden(false, false);

            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes()
            {
                Font = UIFont.FromName("HelveticaNeue-Light", (nfloat)20f),
                TextColor = UIColor.White
            });

            iRate.SharedInstance.DaysUntilPrompt = 10;
            iRate.SharedInstance.UsesUntilPrompt = 20;

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;

            CachedImageRenderer.Init();
            SlideOverKit.iOS.SlideOverKit.Init();
            OxyPlot.Xamarin.Forms.Platform.iOS.PlotViewRenderer.Init();

            LoadApplication(new App());
            CrossPushNotification.Initialize<CrossPushNotificationListener>();

            // get permission for notification
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // iOS 10
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
                {
                    Console.WriteLine(granted);
                });

                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = this;
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            // Firebase component initialize
            Firebase.Analytics.App.Configure();
            /* Firebase.InstanceID.InstanceId.Notifications.ObserveTokenRefresh((sender, e) => {
                var newToken = Firebase.InstanceID.InstanceId.SharedInstance.Token;
                if (newToken != null)
                {
                    System.Diagnostics.App.AddLog("Token received: " + newToken);
                    if (CrossPushNotification.Current is IPushNotificationHandler)
                        ((IPushNotificationHandler)CrossPushNotification.Current).OnRegisteredSuccess(newToken);
                }
                else
                    System.Diagnostics.App.AddLog("No new token received.");
            });*/

            connectFCM();

            var token = Firebase.InstanceID.InstanceId.SharedInstance.Token;
            if (token != null)
                registerAsync(token);

            return base.FinishedLaunching(app, options);
        }

        private async void registerAsync(String token)
        {
            App.AddLog(string.Format("GCM: Push Notification - Device Registered - Token : {0}", token));
            String Id = Helpers.UsefulBits.GetDeviceID();
            bool bSuccess = await App.ApiService.CleanRegisteredDevice(Id);
            if (bSuccess)
            {
                bSuccess = await App.ApiService.RegisterDevice(Id, token);
                if (bSuccess)
                    App.AddLog("GCM: Device registered on Domoticz");
                else
                    App.AddLog("GCM: Device not registered on Domoticz");
            }
        }

        public override void DidEnterBackground(UIApplication uiApplication)
        {
            Messaging.SharedInstance.Disconnect();
        }

        private void connectFCM()
        {
            Messaging.SharedInstance.Connect((error) =>
            {
                if (error == null)
                {
                    //TODO: Change Topic to what is required
                    Messaging.SharedInstance.Subscribe("/topics/all");
                }
                App.AddLog(error != null ? "GCM: error occured: " + error.Description : "GCM: connect success");
            });
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
#if DEBUG
            Firebase.InstanceID.InstanceId.SharedInstance.SetApnsToken(deviceToken, Firebase.InstanceID.ApnsTokenType.Sandbox);
#endif
#if RELEASE
			Firebase.InstanceID.InstanceId.SharedInstance.SetApnsToken(deviceToken, Firebase.InstanceID.ApnsTokenType.Prod);
#endif
        }

        public override void OnActivated(UIApplication uiApplication)
        {
            connectFCM();
            base.OnActivated(uiApplication);
        }

        // iOS 9 <=, fire when recieve notification foreground
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            if (App.AppSettings.EnableNotifications)
            {
                Messaging.SharedInstance.AppDidReceiveMessage(userInfo);

                // Generate custom event
                NSString[] keys = { new NSString("Event_type") };
                NSObject[] values = { new NSString("Recieve_Notification") };
                var parameters = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(keys, values, keys.Length);

                // Send custom event
                Firebase.Analytics.Analytics.LogEvent("CustomEvent", parameters);

                if (application.ApplicationState == UIApplicationState.Active)
                {
                    App.AddLog(userInfo.ToString());
                    var aps_d = userInfo["aps"] as NSDictionary;
                    var alert_d = aps_d["alert"] as NSDictionary;
                    var body = alert_d["body"] as NSString;
                    var title = alert_d["title"] as NSString;
                    if (String.IsNullOrEmpty(title))
                        title = alert_d["subject"] as NSString;
                    debugAlert(title, body);
                }
            }
        }

        // iOS 10, fire when recieve notification foreground
        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            if (App.AppSettings.EnableNotifications)
            {
                var title = notification.Request.Content.Title;
                var body = notification.Request.Content.Body;
                debugAlert(title, body);
            }
        }

        private void debugAlert(string title, string message)
        {
            var alert = new UIAlertView(title ?? "Title", message ?? "Message", null, "Cancel", "OK");
            alert.Show();
        }
    }


    public partial class PushNotificationApplicationDelegate : UIApplicationDelegate
    {
        const string TAG = "PushNotification-APN";

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            if (CrossPushNotification.Current is IPushNotificationHandler)
            {
                ((IPushNotificationHandler)CrossPushNotification.Current).OnErrorReceived(error);
            }
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            if (CrossPushNotification.Current is IPushNotificationHandler)
            {
                ((IPushNotificationHandler)CrossPushNotification.Current).OnRegisteredSuccess(deviceToken);
            }
        }

        public override void DidRegisterUserNotificationSettings(UIApplication application, UIUserNotificationSettings notificationSettings)
        {
            application.RegisterForRemoteNotifications();
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
			if (CrossPushNotification.Current is IPushNotificationHandler) 
			{
				((IPushNotificationHandler)CrossPushNotification.Current).OnMessageReceived(userInfo);
			}
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            if (CrossPushNotification.Current is IPushNotificationHandler)
            {
                ((IPushNotificationHandler)CrossPushNotification.Current).OnMessageReceived(userInfo);

            }
        }
    }
}
