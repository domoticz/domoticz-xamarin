using System;

using NL.HNOGames.Domoticz.ViewModels;
using Xamarin.Forms;
using Rg.Plugins.Popup.Pages;
using System.Threading.Tasks;
using System.Collections.Generic;
using NL.HNOGames.Domoticz.Models;
using Acr.UserDialogs;
using NL.HNOGames.Domoticz.Resources;
using System.Linq;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    public partial class EventsPage : ContentPage
    {
        private List<Event> eventList;

        public EventsPage()
        {
            InitializeComponent();
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            listView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            new Command(async () => await ExecuteLoadLogsCommand()).Execute(null);
        }

        async Task ExecuteLoadLogsCommand()
        {
            App.ShowLoading();
            eventList = new List<Event>();
            var events = await App.ApiService.GetEvents();

            if (events != null && events.result != null)
            {
                foreach (Event n in events.result)
                    eventList.Add(n);
                listView.ItemsSource = eventList;
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
        private void sbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                String filterText = e.NewTextValue.ToLower().Trim();
                if (filterText == string.Empty)
                {
                    listView.ItemsSource = null;
                    listView.ItemsSource = eventList;
                }
                else
                {
                    listView.ItemsSource = null;
                    listView.ItemsSource = eventList.Where(i => i.Name.ToLower().Trim().Contains(filterText));
                }
            }
            catch (Exception)
            {
                listView.ItemsSource = null;
                listView.ItemsSource = eventList;
            }
        }
    }
}