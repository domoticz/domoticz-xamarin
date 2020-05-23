using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;
using Xamarin.Forms;
using NL.HNOGames.Domoticz.Controls;
using NL.HNOGames.OOTT.iOS.Renderer;
using System.ComponentModel;
using NL.HNOGames.Domoticz;

[assembly: ExportRenderer(typeof(MaterialFrame), typeof(MaterialFrameRenderer))]
namespace NL.HNOGames.OOTT.iOS.Renderer
{
    /// <summary>
    /// Renderer to update all frames with better shadows matching material design standards
    /// </summary>

    public class MaterialFrameRenderer : FrameRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            MaterialFrame frame = (MaterialFrame)Element;

            if (frame != null)
            {
                // Update shadow to match better material design standards of elevation
                Layer.ShadowRadius = 2.0f;
                Layer.ShadowColor = UIColor.Gray.CGColor;
                Layer.ShadowOffset = new CGSize(2, 2);
                Layer.ShadowOpacity = 0.80f;

                if(!App.AppSettings.DarkTheme)
                    Layer.ShadowColor = UIColor.Gray.CGColor;
                else
                {
                    Layer.ShadowColor = UIColor.Gray.CGColor;
                    Layer.ShadowPath = UIBezierPath.FromRect(Layer.Bounds).CGPath;
                }

                Layer.MasksToBounds = false;
            }
        }
    }
}