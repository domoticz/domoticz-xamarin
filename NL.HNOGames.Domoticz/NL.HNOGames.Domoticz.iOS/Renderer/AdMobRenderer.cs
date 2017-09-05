using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Google.MobileAds;
using UIKit;
using NL.HNOGames.Domoticz.Controls;
using NL.HNOGames.Domoticz.iOS.Renderer;

[assembly: ExportRenderer(typeof(AdMobView), typeof(AdMobRenderer))]
namespace NL.HNOGames.Domoticz.iOS.Renderer
{
    public class AdMobRenderer : ViewRenderer
    {
        private const string AdmobIdLight = "ca-app-pub-2210179934394995/7328793248";
        private const string AdmobIdDark = "ca-app-pub-2210179934394995/2623261472";

        private BannerView _adView;
        private bool _viewOnScreen;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null)
                return;

            if (e.OldElement != null) return;
            UIViewController viewCtrl = null;
            foreach (var v in UIApplication.SharedApplication.Windows)
            {
                if (v.RootViewController != null)
                {
                    viewCtrl = v.RootViewController;
                }
            }
            _adView = new BannerView(AdSizeCons.Banner)
            {
                AdUnitID = App.AppSettings.DarkTheme ? AdmobIdLight : AdmobIdDark,
                RootViewController = viewCtrl
            };
            _adView.AdReceived += (sender, args) =>
            {
                App.AddLog("********** BANNER AD RECEIVED");
                if (!_viewOnScreen) AddSubview(_adView);
                _viewOnScreen = true;
            };
            _adView.ReceiveAdFailed += (sender, args) =>
            {
                App.AddLog("********** BANNER AD FAILED");
            };

            var request = Request.GetDefaultRequest();
#if DEBUG
            request.TestDevices = new string[] { Request.SimulatorId };
#endif
            _adView.LoadRequest(request);

            SetNativeControl(_adView);
        }
    }
}
