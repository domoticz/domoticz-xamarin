using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace NL.HNOGames.Domoticz.Controls
{
   [Preserve(AllMembers = true)]
   public class CustomTabbedPage : TabbedPage
   {
      public CustomTabbedPage()
      {
         CurrentPageChanged += OnCurrentPageChanged;
      }

      private void OnCurrentPageChanged(object sender, EventArgs e)
      {
         if (CurrentPage == null)
            return;
         var titleView = NavigationPage.GetTitleView(CurrentPage);
         if (titleView != null)
            NavigationPage.SetTitleView(this, titleView);
      }
   }
}
