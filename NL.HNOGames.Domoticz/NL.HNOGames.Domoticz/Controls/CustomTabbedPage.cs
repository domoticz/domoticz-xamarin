using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace NL.HNOGames.Domoticz.Controls
{
    /// <summary>
    /// Defines the <see cref="CustomTabbedPage" />
    /// </summary>
    [Preserve(AllMembers = true)]
    public class CustomTabbedPage : TabbedPage
    {
        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTabbedPage"/> class.
        /// </summary>
        public CustomTabbedPage()
        {
            CurrentPageChanged += OnCurrentPageChanged;
        }

        #endregion

        #region Private

        /// <summary>
        /// The OnCurrentPageChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void OnCurrentPageChanged(object sender, EventArgs e)
        {
            if (CurrentPage == null)
                return;
            var titleView = NavigationPage.GetTitleView(CurrentPage);
            if (titleView != null)
                NavigationPage.SetTitleView(this, titleView);
        }

        #endregion
    }
}
