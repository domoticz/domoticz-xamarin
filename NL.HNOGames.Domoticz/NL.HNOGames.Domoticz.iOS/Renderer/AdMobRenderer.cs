using System;
using System.Collections.Generic;
using System.Text;
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
        const string AdmobID = "ca-app-pub-2210179934394995/1566859863";

        BannerView adView;
        bool viewOnScreen;
 
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null)
                return;
 
            if (e.OldElement == null)
            {
                UIViewController viewCtrl = null;
                foreach (UIWindow v in UIApplication.SharedApplication.Windows)
                {
                    if (v.RootViewController != null)
                    {
                        viewCtrl = v.RootViewController;
                    }
                }

                adView = new BannerView()
                {
                    AdUnitID = AdmobID,
                    RootViewController = viewCtrl
                };
 
                adView.AdReceived += (sender, args) =>
                {
                    if (!viewOnScreen) this.AddSubview(adView);
                    viewOnScreen = true;
                };

                adView.LoadRequest(Request.GetDefaultRequest());
                base.SetNativeControl(adView);
            }
        }
    }
}
