using Shiny.Locations;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Service
{
    public class MyGeofenceDelegate : IGeofenceDelegate
    {
        public async Task OnStatusChanged(GeofenceState newState, GeofenceRegion region)
        {
            if (newState == GeofenceState.Entered)
            {

            }

            else if (newState == GeofenceState.Exited)
            {

            }
        }
    }
}
