using System;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.Controls
{
   public class ExtendedSlider : Slider
   {
      public event EventHandler TouchDown;
      public event EventHandler TouchUpInside;
      public event EventHandler TouchUpOutside;

      public void OnTouchDown(EventArgs e)
      {
         TouchDown?.Invoke(this, e);
      }

      public void OnTouchUpInside(EventArgs e)
      {
         TouchUpInside?.Invoke(this, e);
      }

      public void OnTouchUpOutside(EventArgs e)
      {
         TouchUpOutside?.Invoke(this, e);
      }
   }
}
