using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using NL.HNOGames.Domoticz.Controls;

namespace NL.HNOGames.Domoticz.iOS.Renderer
{
    public class CustomMultiLineLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            MultiLineLabel multiLineLabel = (MultiLineLabel)Element;

            if (multiLineLabel != null && multiLineLabel.Lines != -1)
                Control.Lines = multiLineLabel.Lines;
        }
    }
}