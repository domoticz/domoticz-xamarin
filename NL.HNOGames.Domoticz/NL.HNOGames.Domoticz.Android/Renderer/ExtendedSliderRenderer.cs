using Android.Content;
using Android.Views;
using NL.HNOGames.Domoticz.Controls;
using NL.HNOGames.Domoticz.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(ExtendedSlider), typeof(ExtendedSliderRenderer))]
namespace NL.HNOGames.Domoticz.Droid.Renderer
{
    /// <summary>
    /// Defines the <see cref="ExtendedSliderRenderer" />
    /// </summary>
    public class ExtendedSliderRenderer : SliderRenderer
    {
        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedSliderRenderer"/> class.
        /// </summary>
        /// <param name="context"></param>
        public ExtendedSliderRenderer(Context context)
        : base(context)
        {
        }

        #endregion

        #region Private

        /// <summary>
        /// The Control_Touch
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="TouchEventArgs"/></param>
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

        #endregion

        /// <summary>
        /// The OnElementChanged
        /// </summary>
        /// <param name="e">The e<see cref="ElementChangedEventArgs{Slider}"/></param>
        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Touch += Control_Touch;
            }
        }
    }
}
