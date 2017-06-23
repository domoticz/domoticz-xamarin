using System;

using NL.HNOGames.Domoticz.ViewModels;
using Xamarin.Forms;
using Rg.Plugins.Popup.Pages;
using System.Threading.Tasks;
using System.Collections.Generic;
using NL.HNOGames.Domoticz.Models;
using Acr.UserDialogs;
using NL.HNOGames.Domoticz.Resources;
using Rg.Plugins.Popup.Services;

namespace NL.HNOGames.Domoticz.Views.Dialog
{
    public partial class LogsPopup : PopupPage
    {
        private Models.Device selectedDevice;
        private List<Log> logList;

        public LogsPopup(Models.Device device)
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
            logList = new List<Log>();
            var logs = await App.ApiService.GetLogs(selectedDevice, selectedDevice.IsScene);
            if (logs == null || logs.result == null)
                logs = await App.ApiService.GetLogs(selectedDevice, false, true);

            if (logs != null && logs.result != null)
            {
                foreach (Log n in logs.result)
                    logList.Add(n);
                listView.ItemsSource = logList;
            }
            else
            {
                UserDialogs.Instance.Toast(AppResources.error_logs);
                await Navigation.PopAsync();
            }
        }

        private void btnOK_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.PopAsync();
        }
    }
}