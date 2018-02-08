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
            App.AddLog("Message Arrived" + values.ToString());
            String subject = System.Net.WebUtility.UrlDecode(values["subject"].ToString());
            String message = System.Net.WebUtility.UrlDecode(values["message"].ToString());
            if (subject == message)
                subject = "Domoticz";

            String deviceid = values["deviceid"].ToString();

            if (App.AppSettings.EnableNotifications)
                CrossLocalNotifications.Current.Show(subject, message);
            App.ShowToast(message);
        }

        public async void OnRegistered(string token, DeviceType deviceType)
        {
            try
            {
               if (deviceType == DeviceType.Android)
               {
                  App.AddLog(string.Format("Push Notification - Device Registered - Token : {0}", token));
                  String Id = Helpers.UsefulBits.GetDeviceID();
                  //bool bSuccess = await App.ApiService.CleanRegisteredDevice(Id);
                  //if (bSuccess)
                  //{
                  bool bSuccess = await App.ApiService.RegisterDevice(Id, token);
                  if (bSuccess)
                     App.AddLog("Device registered on Domoticz");
                  else
                     App.AddLog("Device not registered on Domoticz");
                  //}
               }
            }
            catch (Exception ex) { }
        }

        public void OnUnregistered(DeviceType deviceType)
        {
            App.AddLog("Push Notification - Device Unnregistered");
        }

        public void OnError(string message, DeviceType deviceType)
        {
            App.AddLog(string.Format("Push notification error - {0}", message));
        }

        public bool ShouldShowNotification()
        {
            return false;//we've implemented our own notifications onReceive method
        }
    }
}
