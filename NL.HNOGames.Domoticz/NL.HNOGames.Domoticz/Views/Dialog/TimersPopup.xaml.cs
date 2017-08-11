using System;

using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using NL.HNOGames.Domoticz.Models;
using Rg.Plugins.Popup.Services;
using Device = NL.HNOGames.Domoticz.Models.Device;

namespace NL.HNOGames.Domoticz.Views.Dialog
{
    public partial class TimersPopup
    {
        private readonly object _selectedDevice;
        private List<Timer> _timerList;

        public TimersPopup(object device)
        {
            _selectedDevice = device;
            InitializeComponent();
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            listView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            new Command(async () => await ExecuteLoadNotificationsCommand()).Execute(null);
        }

        private async Task ExecuteLoadNotificationsCommand()
        {
            _timerList = new List<Timer>();

            var idx = "";
            if (_selectedDevice is Device device)
                idx = device.idx;
            else if (_selectedDevice is Scene)
                idx = ((Scene)_selectedDevice).idx;

            var timers = await App.ApiService.GetTimers(idx);
            if (timers?.result != null)
            {
                foreach (var t in timers.result)
                    _timerList.Add(t);
                listView.ItemsSource = _timerList;
            }
        }

        private void btnOK_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.PopAsync();
        }
    }
}
