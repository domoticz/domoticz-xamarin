using NL.HNOGames.Domoticz.Resources;
using Plugin.LocalNotifications;
using Shiny.Beacons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Service
{
    class MyBeaconDelegate : IBeaconDelegate
    {
        /// <summary>
        /// On beacon changed
        /// </summary>
        public async Task OnStatusChanged(BeaconRegionState newState, BeaconRegion region)
        {
            App.AddLog("Beacon Status changed: " + region.Uuid.ToString() + " | " + newState.ToString());
            if (newState == BeaconRegionState.Entered || newState == BeaconRegionState.Exited)
                await processBeaconId(region.Uuid.ToString(), newState);
        }

        /// <summary>
        /// The beaconId
        /// </summary>
        private async Task processBeaconId(string beaconId, BeaconRegionState state)
        {
            var beacon = App.AppSettings.Beacons.FirstOrDefault(o => o.Id == beaconId);
            if (beacon != null && beacon.Enabled)
            {
                App.AddLog("Beacon ID Found: " + beaconId);
                _ = await App.ApiService.HandleSwitch(beacon.SwitchIDX, beacon.SwitchPassword, state == BeaconRegionState.Entered ? 1 : 0, beacon.Value, beacon.IsScene);
                if (App.AppSettings.BeaconNotificationsEnabled)
                {
                    App.AddLog("Creating notification for : " + beacon.Name);
                    CrossLocalNotifications.Current.Show(state == BeaconRegionState.Entered ? AppResources.geofence_location_entering.Replace("%1$s", beacon.Name) : AppResources.geofence_location_leaving.Replace("%1$s", beacon.Name),
                        state == BeaconRegionState.Entered ? AppResources.geofence_location_entering_text : AppResources.geofence_location_leaving_text);
                }
            }
            else
                App.AddLog("beacon ID not registered: " + beaconId);
        }
    }
}
