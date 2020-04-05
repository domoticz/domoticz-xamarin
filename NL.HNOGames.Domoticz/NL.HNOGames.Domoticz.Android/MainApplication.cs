using System;
using System.Collections.Generic;
using Firebase;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;

namespace NL.HNOGames.Domoticz.Droid
{
    //You can specify additional application information in this attribute
#if DEBUG
    [Application(Debuggable = true)]
#else
   [Application(Debuggable = false)]
#endif
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        public static Context AppContext;

        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          : base(handle, transer)
        {}

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);

            //FirebaseApp.InitializeApp(this);
            AppContext = ApplicationContext;
            CrossCurrentActivity.Current.Init(this);

//            //Set the default notification channel for your app when running Android Oreo
//            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
//            {
//                FirebasePushNotificationManager.DefaultNotificationChannelId = "Default";
//                FirebasePushNotificationManager.DefaultNotificationChannelName = "General";
//            }

//            //If debug you should reset the token each time.
//#if DEBUG
//            FirebasePushNotificationManager.Initialize(this, true);
//#else
//              FirebasePushNotificationManager.Initialize(this,false);
//#endif

            //CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            //{
            //    System.Diagnostics.Debug.WriteLine("NOTIFICATION RECEIVED", p.Data);
            //};
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }
    }
}