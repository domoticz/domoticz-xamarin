using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    /// <summary>
    /// Defines the <see cref="ServerLogsPage" />
    /// </summary>
    public partial class ServerLogsPage
    {
        #region Variables

        /// <summary>
        /// Defines the _logList
        /// </summary>
        private List<ServerLog> _logList;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerLogsPage"/> class.
        /// </summary>
        public ServerLogsPage()
        {
            InitializeComponent();

            searchIcon.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(OnSearchIconTapped) });
            searchBar.TextChanged += searchBar_TextChanged;
            searchBar.Cancelled += (s, e) => OnCancelled();
        }

        #endregion

        #region Private

        /// <summary>
        /// The OnItemSelected
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="args">The args<see cref="SelectedItemChangedEventArgs"/></param>
        private void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            listView.SelectedItem = null;
        }

        /// <summary>
        /// The ExecuteLoadLogsCommand
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task ExecuteLoadLogsCommand()
        {
            App.ShowLoading();
            _logList = new List<ServerLog>();
            var logs = await App.ApiService.GetServerLogs();

            if (logs?.result != null)
            {
                foreach (var n in logs.result)
                    _logList.Add(n);
                _logList.Reverse();
                listView.ItemsSource = _logList;
                App.HideLoading();
            }
            else
            {
                App.HideLoading();
                App.ShowToast(AppResources.error_logs);
                await Navigation.PopAsync();
            }
        }

        /// <summary>
        /// Filter changed
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="TextChangedEventArgs"/></param>
        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var filterText = e.NewTextValue.ToLower().Trim();
                if (filterText == string.Empty)
                {
                    listView.ItemsSource = null;
                    listView.ItemsSource = _logList;
                }
                else
                {
                    listView.ItemsSource = null;
                    listView.ItemsSource = _logList.Where(i => i.message.ToLower().Trim().Contains(filterText));
                }
            }
            catch (Exception)
            {
                listView.ItemsSource = null;
                listView.ItemsSource = _logList;
            }
        }

        /// <summary>
        /// The OnSearchIconTapped
        /// </summary>
        private void OnSearchIconTapped()
        {
            BatchBegin();
            try
            {
                NavigationPage.SetHasBackButton(this, false);
                titleLayout.IsVisible = false;
                searchIcon.IsVisible = false;
                searchBar.IsVisible = true;
                searchBar.Focus();
            }
            finally
            {
                BatchCommit();
            }
        }

        /// <summary>
        /// The OnCancelled
        /// </summary>
        private void OnCancelled()
        {
            BatchBegin();
            try
            {
                NavigationPage.SetHasBackButton(this, true);
                searchBar.IsVisible = false;
                searchBar.Text = string.Empty;
                titleLayout.IsVisible = true;
                searchIcon.IsVisible = true;
            }
            finally
            {
                BatchCommit();
            }
        }

        #endregion

        /// <summary>
        /// The OnAppearing
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            new Command(async () => await ExecuteLoadLogsCommand()).Execute(null);
        }
    }
}
