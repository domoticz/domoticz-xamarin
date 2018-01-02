using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using BlendPOC.Controls;
using BlendPOC.iOS.Renderer;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(MaterialFrame), typeof(MaterialFrameRenderer))]
namespace NL.HNOGames.Domoticz.iOS.Renderer
{
    /// <summary>
    /// Renderer to update all frames with better shadows matching material design standards
    /// </summary>

    public class MaterialFrameRenderer : FrameRenderer
    {
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            // Update shadow to match better material design standards of elevation
            Layer.ShadowRadius = 2.0f;
            Layer.ShadowColor = UIColor.Gray.CGColor;
            Layer.ShadowOffset = new CGSize(2, 2);
            Layer.ShadowOpacity = 0.80f;
            Layer.ShadowPath = UIBezierPath.FromRect(Layer.Bounds).CGPath;
            Layer.MasksToBounds = false;
        }
    }
}