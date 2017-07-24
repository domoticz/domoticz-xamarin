
using FFImageLoading.Forms.Touch;
using Foundation;
using UIKit;

using System;
using MTiRate;
using PushNotification.Plugin;
using NL.HNOGames.Domoticz.Helpers;
using UserNotifications;
using Firebase.CloudMessaging;

namespace NL.HNOGames.Domoticz.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            iRate.SharedInstance.DaysUntilPrompt = 10;
            iRate.SharedInstance.UsesUntilPrompt = 20;

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;

            CachedImageRenderer.Init();
            SlideOverKit.iOS.SlideOverKit.Init();
            OxyPlot.Xamarin.Forms.Platform.iOS.PlotViewRenderer.Init();

            CrossPushNotification.Initialize<CrossPushNotificationListener>();
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // Ask the user for permission to get notifications on iOS 10.0+
                UNUserNotificationCenter.Current.RequestAuthorization(
                        UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
                        (approved, error) => { });
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                // Ask the user for permission to get notifications on iOS 8.0+
                var settings = UIUserNotificationSettings.GetSettingsForTypes(
                        UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                        new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            UIApplication.SharedApplication.RegisterForRemoteNotifications();
            
            // Firebase component initialize
            Firebase.Analytics.App.Configure();
            Firebase.InstanceID.InstanceId.Notifications.ObserveTokenRefresh((sender, e) => {
                var newToken = Firebase.InstanceID.InstanceId.SharedInstance.Token;
                // if you want to send notification per user, use this token
                System.Diagnostics.Debug.WriteLine(newToken);
                connectFCM();

                if (CrossPushNotification.Current is IPushNotificationHandler)
                {
                    ((IPushNotificationHandler)CrossPushNotification.Current).OnRegisteredSuccess(newToken);
                }
            });

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        private void connectFCM()
        {
            Messaging.SharedInstance.Connect((error) =>
            {
                if (error == null)
                {
                    Messaging.SharedInstance.Subscribe("/topics/all");
                }
                System.Diagnostics.Debug.WriteLine(error != null ? "error occured" : "connect success");
            });
        }

        public override void OnActivated(UIApplication uiApplication)
        {
            connectFCM();
            base.OnActivated(uiApplication);
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
