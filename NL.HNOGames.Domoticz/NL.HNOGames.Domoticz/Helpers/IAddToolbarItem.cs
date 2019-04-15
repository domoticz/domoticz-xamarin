using System;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.Helpers
{
   public interface IAddToolbarItem
   {
      event EventHandler ToolbarItemAdded;
      Color CellBackgroundColor { get; }
      Color CellTextColor { get; }
      Color MenuBackgroundColor { get; }
      float RowHeight { get; }
      Color ShadowColor { get; }
      float ShadowOpacity { get; }
      float ShadowRadius { get; }
      float ShadowOffsetDimension { get; }
      float TableWidth { get; }
   }
}