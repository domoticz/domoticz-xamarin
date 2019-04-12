using System;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.Controls
{
   /// <summary>
   /// Custom view cell
   /// </summary>
   public class CustomViewCell : ViewCell
   {
      /// <summary>
      /// Row height property
      /// </summary>
      public static BindableProperty RowHeightProperty = BindableProperty.Create(nameof(RowHeight), typeof(int), typeof(int), 0, propertyChanged: UpdateHeight);

      /// <summary>
      /// Row height
      /// </summary>
      public int RowHeight
      {
         get { return (int)GetValue(RowHeightProperty); }
         set { SetValue(RowHeightProperty, value); }
      }

      /// <summary>
      /// Update heights
      /// </summary>
      private static void UpdateHeight(BindableObject bindable, object oldHeight, object newHeight)
      {
         try
         {
            ((ViewCell)bindable).Height = (int)newHeight;
         }
         catch (Exception) { // Just a handler to make sure we dont crash
         }
      }
   }
}
