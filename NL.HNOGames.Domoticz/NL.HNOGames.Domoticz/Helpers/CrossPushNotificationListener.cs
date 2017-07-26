using PushNotification.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PushNotification.Plugin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Acr.UserDialogs;
using Plugin.LocalNotifications;

namespace NL.HNOGames.Domoticz.Helpers
{
    /// <summary>
    /// Class to handle push notifications listens to events such as registration, unregistration, message arrival and errors.
    /// </summary>
    public class CrossPushNotificationListener : IPushNotificationListener
    {
        public void OnMessage(JObject values, DeviceType deviceType)
        {
            Debug.WriteLine("Message Arrived" + values.ToString());
            String subject = System.Net.WebUtility.UrlDecode(values["subject"].ToString());
            String message = System.Net.WebUtility.UrlDecode(values["message"].ToString());
            if (subject == message)
                subject = "Domoticz";

            String deviceid = values["deviceid"].ToString();

            if (App.AppSettings.EnableNotifications)
                CrossLocalNotifications.Current.Show(subject, message);
            UserDialogs.Instance.Toast(message);
        }

        public async void OnRegistered(string token, DeviceType deviceType)
        {
            if (deviceType == DeviceType.Android)
            {
                Debug.WriteLine(string.Format("Push Notification - Device Registered - Token : {0}", token));
                String Id = Helpers.UsefulBits.GetDeviceID();
                bool bSuccess = await App.ApiService.CleanRegisteredDevice(Id);
                if (bSuccess)
                {
                    bSuccess = await App.ApiService.RegisterDevice(Id, token);
                    if (bSuccess)
                        Debug.WriteLine("Device registered on Domoticz");
                    else
                        Debug.WriteLine("Device not registered on Domoticz");
                }
            }
        }

        public void OnUnregistered(DeviceType deviceType)
        {
            Debug.WriteLine("Push Notification - Device Unnregistered");
        }

        public void OnError(string message, DeviceType deviceType)
        {
            Debug.WriteLine(string.Format("Push notification error - {0}", message));
        }

        public bool ShouldShowNotification()
        {
            return false;//we've implemented our own notifications onReceive method
        }
    }
}
