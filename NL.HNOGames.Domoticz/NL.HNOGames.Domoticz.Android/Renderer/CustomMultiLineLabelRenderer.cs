using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using NL.HNOGames.Domoticz.Controls;
using Android.Content;

namespace NL.HNOGames.Domoticz.Droid.Renderer
{
   public class CustomMultiLineLabelRenderer : LabelRenderer
   {
      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="context"></param>
      public CustomMultiLineLabelRenderer(Context context)
        : base(context)
      { }

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