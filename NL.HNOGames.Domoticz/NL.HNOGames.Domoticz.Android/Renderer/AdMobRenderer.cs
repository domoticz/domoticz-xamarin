using Android.Content;
using NL.HNOGames.Domoticz.Controls;
using NL.HNOGames.Domoticz.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(AdMobView), typeof(AdMobRenderer))]
namespace NL.HNOGames.Domoticz.Droid.Renderer
{
    /// <summary>
    /// Defines the <see cref="AdMobRenderer" />
    /// </summary>
    public class AdMobRenderer : ViewRenderer<AdMobView, Android.Gms.Ads.AdView>
    {
        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AdMobRenderer"/> class.
        /// </summary>
        /// <param name="context"></param>
        public AdMobRenderer(Context context)
        : base(context)
        {
        }

        #endregion

        /// <summary>
        /// The OnElementChanged
        /// </summary>
        /// <param name="e">The e<see cref="ElementChangedEventArgs{AdMobView}"/></param>
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
