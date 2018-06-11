using NL.HNOGames.OOTT.iOS.Helpers;
using NL.HNOGames.Domoticz.Helpers;
using System.Threading;

[assembly: Xamarin.Forms.Dependency(typeof(CloseApplication))]
namespace NL.HNOGames.OOTT.iOS.Helpers
{
   public class CloseApplication : ICloseApplication
   {
      /// <summary>
      /// Close application
      /// </summary>
      public void Close()
      {
         Thread.CurrentThread.Abort();
      }
   }
}