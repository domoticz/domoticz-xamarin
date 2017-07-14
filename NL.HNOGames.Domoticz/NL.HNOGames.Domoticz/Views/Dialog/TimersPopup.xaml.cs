using System;

using NL.HNOGames.Domoticz.ViewModels;
using Xamarin.Forms;
using Rg.Plugins.Popup.Pages;
using System.Threading.Tasks;
using System.Collections.Generic;
using NL.HNOGames.Domoticz.Models;
using Rg.Plugins.Popup.Services;

namespace NL.HNOGames.Domoticz.Views.Dialog
{
    public partial class TimersPopup : PopupPage
    {
        private object selectedDevice;
        private List<Timer> timerList;

        public TimersPopup(object device)
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

            string idx = "";
            if (selectedDevice is Models.Device)
                idx = ((Models.Device)selectedDevice).idx;
            else if (selectedDevice is Models.Scene)
                idx = ((Models.Scene)selectedDevice).idx;

            var timers = await App.ApiService.GetTimers(idx);
            if (timers != null && timers.result != null)
            {
                foreach (Timer t in timers.result)
                    timerList.Add(t);
                listView.ItemsSource = timerList;
            }
        }

        private void btnOK_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.PopAsync();
        }
    }
}
