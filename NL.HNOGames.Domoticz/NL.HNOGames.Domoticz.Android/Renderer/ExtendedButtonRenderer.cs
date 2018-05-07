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
using Xamarin.Forms;
using NL.HNOGames.Domoticz.Controls;
using NL.HNOGames.Domoticz.Droid.Renderer;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(ExtendedButton), typeof(EnhancedButtonRenderer))]
namespace NL.HNOGames.Domoticz.Droid.Renderer
{
   public class EnhancedButtonRenderer : ButtonRenderer
   {
      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="context"></param>
      public EnhancedButtonRenderer(Context context)
        : base(context)
      {}

      protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
      {
         base.OnElementChanged(e);
         UpdatePadding();
      }

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