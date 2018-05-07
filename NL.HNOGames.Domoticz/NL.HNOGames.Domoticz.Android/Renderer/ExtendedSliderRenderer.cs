using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NL.HNOGames.Domoticz.Controls;
using NL.HNOGames.Domoticz.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedSlider), typeof(ExtendedSliderRenderer))]
namespace NL.HNOGames.Domoticz.Droid.Renderer
{
    public class ExtendedSliderRenderer : SliderRenderer
    {
      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="context"></param>
      public ExtendedSliderRenderer(Context context)
        : base(context)
      { }

      protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Touch += Control_Touch;
            }
        }

        private void Control_Touch(object sender, TouchEventArgs e)
        {
            var slider = Element as ExtendedSlider;

            if (e.Event.Action == MotionEventActions.Up)
            {
                slider.OnTouchUpInside(e);
            }
            else if (e.Event.Action == MotionEventActions.Outside)
            {
                // Doesn't seem to fire
                slider.OnTouchUpOutside(e);
            }
            else if (e.Event.Action == MotionEventActions.Down)
            {
                slider.OnTouchDown(e);
            }

            e.Handled = false;
        }
    }
}