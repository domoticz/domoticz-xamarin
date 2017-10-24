using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Google.MobileAds;
using UIKit;
using NL.HNOGames.Domoticz.Controls;
using NL.HNOGames.Domoticz.iOS.Renderer;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(ExtendedButton), typeof(ExtendedButtonRenderer))]
namespace NL.HNOGames.Domoticz.iOS.Renderer
{
    public class ExtendedButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            UpdatePadding();
        }

        private void UpdatePadding()
        {
            var element = this.Element as ExtendedButton;
            if (element != null)
            {
                this.Control.ContentEdgeInsets = new UIEdgeInsets(

                    (int)element.Padding.Top,
                    (int)element.Padding.Left,
                    (int)element.Padding.Bottom,
                    (int)element.Padding.Right
                );
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(ExtendedButton.Padding))
            {
                UpdatePadding();
            }
        }
    }
}
