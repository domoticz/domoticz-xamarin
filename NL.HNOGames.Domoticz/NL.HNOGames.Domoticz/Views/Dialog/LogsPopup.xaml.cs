using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using Rg.Plugins.Popup.Services;
using Device = NL.HNOGames.Domoticz.Models.Device;

namespace NL.HNOGames.Domoticz.Views.Dialog
{
    public partial class LogsPopup
    {
        private readonly object _selectedDevice;
        private List<Log> _logList;

        public LogsPopup(object device)
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
            var idx = "";
            if (_selectedDevice is Device device)
                idx = device.idx;
            else if (_selectedDevice is Scene)
                idx = ((Scene) _selectedDevice).idx;

            _logList = new List<Log>();
            var logs = await App.ApiService.GetLogs(idx, _selectedDevice is Scene);
            if (logs?.result == null)
                logs = await App.ApiService.GetLogs(idx, false, true);

            int counterMaxLogs = 0;
            if (logs?.result != null)
            {
                foreach (var n in logs.result)
                {
                    if (counterMaxLogs > 250)
                        break;
                    _logList.Add(n);
                    counterMaxLogs++;
                }
                listView.ItemsSource = _logList;
            }
            else
            {
                App.ShowToast(AppResources.error_logs);
                await Navigation.PopAsync();
            }
        }

        private void btnOK_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}