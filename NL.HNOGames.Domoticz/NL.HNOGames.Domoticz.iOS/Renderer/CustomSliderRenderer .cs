using System;
using Xamarin.Forms;
using NL.HNOGames.Domoticz.Controls;
using NL.HNOGames.Domoticz.iOS.Renderer;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(ExtendedSlider), typeof(CustomSliderRenderer))]
namespace NL.HNOGames.Domoticz.iOS.Renderer
{
    public class CustomSliderRenderer : SliderRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.TouchDown += Control_TouchDown;
                Control.TouchUpInside += Control_TouchUpInside;
                Control.TouchUpOutside += Control_TouchUpOutside;
            }
        }
        private void Control_TouchDown(object sender, EventArgs e)
        {
            var slider = Element as ExtendedSlider;
            slider.OnTouchDown(e);
        }

        private void Control_TouchUpInside(object sender, EventArgs e)
        {
            var slider = Element as ExtendedSlider;
            slider.OnTouchUpInside(e);
        }

        private void Control_TouchUpOutside(object sender, EventArgs e)
        {
            var slider = Element as ExtendedSlider;
            slider.OnTouchUpOutside(e);
        }
    }
}
