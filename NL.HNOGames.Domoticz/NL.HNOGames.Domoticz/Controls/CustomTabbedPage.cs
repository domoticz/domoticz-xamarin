using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace NL.HNOGames.Domoticz.Controls
{
   /// <summary>
   /// Custom tabbed page object
   /// </summary>
   [Preserve(AllMembers = true)]
   public class CustomTabbedPage : TabbedPage
   {
      #region Constructor & Destructor

      /// <summary>
      /// Default constructor
      /// </summary>
      public CustomTabbedPage()
      {
         if (Device.RuntimePlatform == Device.iOS)
            NavigationPage.SetHasNavigationBar(this, false);
         else if (Device.RuntimePlatform == Device.Android)
            CurrentPageChanged += OnCurrentPageChanged;
      }

      #endregion

      #region Private

      /// <summary>
      /// Set title of the 'current page' as the bottom tabbed page title
      /// </summary>
      /// <param name="sender">Sender object</param>
      /// <param name="e">Event arguments</param>
      private void OnCurrentPageChanged(object sender, EventArgs e)
      {
         if (CurrentPage == null)
            return;

         // Set the title view of the current page to this page
         var titleView = NavigationPage.GetTitleView(CurrentPage);
         if (titleView != null)
            NavigationPage.SetTitleView(this, titleView);
      }

      #endregion

   }
}
