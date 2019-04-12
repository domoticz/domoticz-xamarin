using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace NL.HNOGames.Domoticz.Controls
{
   [Preserve(AllMembers = true)]
   public class TitleViewSearchBar : SearchBar
   {
      public static readonly BindableProperty CancelButtonTextProperty = BindableProperty.Create(nameof(CancelButtonText), typeof(string), typeof(TitleViewSearchBar), null);
      public event EventHandler Cancelled;

      public string CancelButtonText
      {
         get => (string)GetValue(CancelButtonTextProperty);
         set => SetValue(CancelButtonTextProperty, value);
      }

      public void SendCancelled()
      {
         Cancelled?.Invoke(this, EventArgs.Empty);
      }
   }
}
