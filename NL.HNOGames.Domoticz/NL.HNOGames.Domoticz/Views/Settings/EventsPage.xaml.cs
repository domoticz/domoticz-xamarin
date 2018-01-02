using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using System.Linq;
using NL.HNOGames.Domoticz.Data;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    public partial class EventsPage
    {
        private List<Event> _eventList;

        public EventsPage()
        {
            InitializeComponent();
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            listView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            new Command(async () => await ExecuteLoadLogsCommand()).Execute(null);
        }

        /// <summary>
        /// Load all logs
        /// </summary>
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
        private void sbSearch_TextChanged(object sender, TextChangedEventArgs e)
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

                    var result = await App.ApiService.SetEvent(oDevice.id, oSwitch.IsToggled);
                    new Command(async () => await ExecuteLoadLogsCommand()).Execute(null);
                }
            }
            catch (Exception ex)
            {
                App.ShowToast(AppResources.var_input_error);
            }
        }
    }
}