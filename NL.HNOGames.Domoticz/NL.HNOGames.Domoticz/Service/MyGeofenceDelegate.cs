using NL.HNOGames.Domoticz.Resources;
using Plugin.LocalNotifications;
using Shiny.Locations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Service
{
    public class MyGeofenceDelegate : IGeofenceDelegate
    {
        /// <summary>
        /// On geofence changed
        /// </summary>
        public async Task OnStatusChanged(GeofenceState newState, GeofenceRegion region)
        {
            App.AddLog("Geofence Status changed: " + region.Identifier + " | " + newState.ToString());
            if (newState == GeofenceState.Entered || newState == GeofenceState.Exited)
                await processGeofenceId(region.Identifier, newState);
        }

        /// <summary>
        /// The GeofenceId
        /// </summary>
        private async Task processGeofenceId(string geofenceId, GeofenceState state)
        {
            var geofence = App.AppSettings.Geofences.FirstOrDefault(o => o.Id == geofenceId);
            if (geofence != null && geofence.Enabled)
            {
                App.AddLog("Geofence ID Found: " + geofenceId);
                _ = await App.ApiService.HandleSwitch(geofence.SwitchIDX, geofence.SwitchPassword, state == GeofenceState.Entered ? 1 : 0, geofence.Value, geofence.IsScene);
                if (App.AppSettings.GeofenceNotificationsEnabled)
                {
                    App.AddLog("Creating notification for : " + geofence.Name);
                    CrossLocalNotifications.Current.Show(state == GeofenceState.Entered ? AppResources.geofence_location_entering.Replace("%1$s", geofence.Name) : AppResources.geofence_location_leaving.Replace("%1$s", geofence.Name),
                        state == GeofenceState.Entered ? AppResources.geofence_location_entering_text : AppResources.geofence_location_leaving_text);
                }
            }
            else
                App.AddLog("Geofence ID not registered: " + geofenceId);
        }
    }
}
