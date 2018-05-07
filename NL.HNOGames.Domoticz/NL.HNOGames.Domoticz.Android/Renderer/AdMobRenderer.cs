using Android.Content;
using NL.HNOGames.Domoticz.Controls;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using NL.HNOGames.Domoticz.Droid.Renderer;

[assembly: ExportRenderer(typeof(AdMobView), typeof(AdMobRenderer))]
namespace NL.HNOGames.Domoticz.Droid.Renderer
{
   public class AdMobRenderer : ViewRenderer<AdMobView, Android.Gms.Ads.AdView>
   {
      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="context"></param>
      public AdMobRenderer(Context context)
        : base(context)
      { }

      protected override void OnElementChanged(ElementChangedEventArgs<AdMobView> e)
      {
         base.OnElementChanged(e);

         if (Control != null) return;
         var ad = new Android.Gms.Ads.AdView(Forms.Context)
         {
            AdSize = Android.Gms.Ads.AdSize.Banner,
            AdUnitId = "ca-app-pub-2210179934394995/1566859863"
         };

         var requestbuilder = new Android.Gms.Ads.AdRequest.Builder();
#if DEBUG
         requestbuilder.AddTestDevice("83DBECBB403C3E924CAA8B529F7E848E");
#endif
         ad.LoadAd(requestbuilder.Build());
         SetNativeControl(ad);
      }
   }
}