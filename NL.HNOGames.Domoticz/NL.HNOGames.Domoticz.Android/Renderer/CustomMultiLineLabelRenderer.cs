using Android.Content;
using NL.HNOGames.Domoticz.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace NL.HNOGames.Domoticz.Droid.Renderer
{
    /// <summary>
    /// Defines the <see cref="CustomMultiLineLabelRenderer" />
    /// </summary>
    public class CustomMultiLineLabelRenderer : LabelRenderer
    {
        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomMultiLineLabelRenderer"/> class.
        /// </summary>
        /// <param name="context"></param>
        public CustomMultiLineLabelRenderer(Context context)
        : base(context)
        {
        }

        #endregion

        /// <summary>
        /// The OnElementChanged
        /// </summary>
        /// <param name="e">The e<see cref="ElementChangedEventArgs{Label}"/></param>
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            MultiLineLabel multiLineLabel = (MultiLineLabel)Element;
            if (multiLineLabel != null && multiLineLabel.Lines != -1)
            {
                Control.SetSingleLine(false);
                Control.SetLines(multiLineLabel.Lines);
            }
        }
    }
}
