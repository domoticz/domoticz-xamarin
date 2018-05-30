using NL.HNOGames.Domoticz.Droid.Helpers;
using NL.HNOGames.Domoticz.Helpers;

[assembly: Xamarin.Forms.Dependency(typeof(CloseApplication))]
namespace NL.HNOGames.Domoticz.Droid.Helpers
{
   public class CloseApplication : ICloseApplication
   {
      /// <summary>
      /// Close application
      /// </summary>
      public void Close()
      {
         Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
      }
   }
}