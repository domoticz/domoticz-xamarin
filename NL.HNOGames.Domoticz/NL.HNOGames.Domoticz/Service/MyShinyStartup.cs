using Microsoft.Extensions.DependencyInjection;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;

namespace NL.HNOGames.Domoticz.Service
{
    public class MyShinyStartup : ShinyStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.UseGeofencing<MyGeofenceDelegate>();
            services.UseBeacons<MyBeaconDelegate>();
        }
    }
}