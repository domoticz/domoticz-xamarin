using System;

using NL.HNOGames.Domoticz.ViewModels;
using Xamarin.Forms;
using Rg.Plugins.Popup.Pages;
using System.Threading.Tasks;
using System.Collections.Generic;
using NL.HNOGames.Domoticz.Models;

namespace NL.HNOGames.Domoticz.Views.Dialog
{
    public partial class TimersPopup : PopupPage
    {
        private Models.Device selectedDevice;
        private List<Timer> timerList;

        public TimersPopup(Models.Device device)
        {
            selectedDevice = device;
            InitializeComponent();
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            listView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            new Command(async () => await ExecuteLoadNotificationsCommand()).Execute(null);
        }

        async Task ExecuteLoadNotificationsCommand()
        {
            timerList = new List<Timer>();

            var timers = await App.ApiService.GetTimers(selectedDevice);
            if (timers != null && timers.result != null)
            {
                foreach (Timer t in timers.result)
                    timerList.Add(t);
                listView.ItemsSource = timerList;
            }
        }
    }
}
