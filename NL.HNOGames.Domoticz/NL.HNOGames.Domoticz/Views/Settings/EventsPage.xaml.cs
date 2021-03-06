﻿using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    /// <summary>
    /// Events page
    /// </summary>
    public partial class EventsPage
    {
        #region Variables

        /// <summary>
        /// Event list
        /// </summary>
        private List<Event> _eventList;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EventsPage"/> class.
        /// </summary>
        public EventsPage()
        {
            InitializeComponent();

            searchIcon.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(OnSearchIconTapped) });
            searchBar.TextChanged += searchBar_TextChanged;
            searchBar.Cancelled += (s, e) => OnCancelled();
        }

        #endregion

        #region Private

        /// <summary>
        /// On item selected
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="args">The args<see cref="SelectedItemChangedEventArgs"/></param>
        private void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            listView.SelectedItem = null;
        }

        /// <summary>
        /// Load all logs
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task ExecuteLoadLogsCommand()
        {
            App.ShowLoading();
            _eventList = new List<Event>();
            var events = await App.ApiService.GetEvents();
            if (events?.result != null)
            {
                foreach (var n in events.result)
                    _eventList.Add(n);
                listView.ItemsSource = _eventList;
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
                    listView.ItemsSource = _eventList;
                }
                else
                {
                    listView.ItemsSource = null;
                    listView.ItemsSource = _eventList.Where(i => i.Name.ToLower().Trim().Contains(filterText));
                }
            }
            catch (Exception)
            {
                listView.ItemsSource = null;
                listView.ItemsSource = _eventList;
            }
        }

        /// <summary>
        /// event swtich toggled
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="ToggledEventArgs"/></param>
        private async void btnSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            try
            {
                var oSwitch = (Switch)sender;
                var oDevice = (Models.Event)oSwitch.BindingContext;
                if (oSwitch.IsToggled != oDevice.Enabled)
                {
                    if (oDevice.Enabled)
                        App.ShowToast(AppResources.switch_off + ": " + oDevice.Name);
                    else
                        App.ShowToast(AppResources.switch_on + ": " + oDevice.Name);
                    await App.ApiService.SetEvent(oDevice.id, oSwitch.IsToggled);
                    new Command(async () => await ExecuteLoadLogsCommand()).Execute(null);
                }
            }
            catch (Exception) // Just in case
            { }
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
        /// On Appearing
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            new Command(async () => await ExecuteLoadLogsCommand()).Execute(null);
        }
    }
}
