using Android.Content;
using NL.HNOGames.Domoticz.Controls;
using NL.HNOGames.Domoticz.Droid.Renderer;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(ExtendedButton), typeof(EnhancedButtonRenderer))]
namespace NL.HNOGames.Domoticz.Droid.Renderer
{
    /// <summary>
    /// Defines the <see cref="EnhancedButtonRenderer" />
    /// </summary>
    public class EnhancedButtonRenderer : ButtonRenderer
    {
        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EnhancedButtonRenderer"/> class.
        /// </summary>
        /// <param name="context"></param>
        public EnhancedButtonRenderer(Context context)
        : base(context)
        {
        }

        #endregion

        #region Private

        /// <summary>
        /// The UpdatePadding
        /// </summary>
        private void UpdatePadding()
        {
            var element = this.Element as ExtendedButton;
            if (element != null)
            {
                this.Control.SetPadding(
                    (int)element.Padding.Left,
                    (int)element.Padding.Top,
                    (int)element.Padding.Right,
                    (int)element.Padding.Bottom
                );
            }
        }

        #endregion

        /// <summary>
        /// The OnElementChanged
        /// </summary>
        /// <param name="e">The e<see cref="ElementChangedEventArgs{Xamarin.Forms.Button}"/></param>
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            UpdatePadding();
        }

        /// <summary>
        /// The OnElementPropertyChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="PropertyChangedEventArgs"/></param>
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
