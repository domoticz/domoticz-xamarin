using Microsoft.Extensions.DependencyInjection;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;

namespace NL.HNOGames.Domoticz.Service
{
    public class MyShinyStartup : ShinyStartup
    {
        public override void ConfigureServices(IServiceCollection services, IPlatform platform)
        {
            var beacons = new List<string>();
            App.AppSettings?.Beacons?.ForEach(o => beacons.Add(o.UUID.ToString()));
            
            services.UseGeofencing<MyGeofenceDelegate>();
            services.UseBeaconMonitoring<MyBeaconDelegate>(new Shiny.Beacons.BeaconMonitorConfig()
            {
                ScanServiceUuids = beacons
            });
        }
    }
}