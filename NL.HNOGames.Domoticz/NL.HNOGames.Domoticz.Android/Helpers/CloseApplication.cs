using NL.HNOGames.Domoticz.Droid.Helpers;
using NL.HNOGames.Domoticz.Helpers;


[assembly: Xamarin.Forms.Dependency(typeof(CloseApplication))]
namespace NL.HNOGames.Domoticz.Droid.Helpers
{
    /// <summary>
    /// Defines the <see cref="CloseApplication" />
    /// </summary>
    public class CloseApplication : ICloseApplication
    {
        #region Public

        /// <summary>
        /// Close application
        /// </summary>
        public void Close()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }

        #endregion
    }
}
