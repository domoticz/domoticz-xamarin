using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.Controls
{
   public class ExtendedButton : Button
   {
      public static BindableProperty PaddingProperty = BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(ExtendedButton), default(Thickness), defaultBindingMode: BindingMode.OneWay);

      public Thickness Padding
      {
         get { return (Thickness)GetValue(PaddingProperty); }
         set { SetValue(PaddingProperty, value); }
      }
   }
}
