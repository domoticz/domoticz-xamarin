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
        private object selectedDevice;
        private List<Log> logList;

        public LogsPopup(object device)
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
            string idx = "";
            if (selectedDevice is Models.Device)
                idx = ((Models.Device)selectedDevice).idx;
            else if (selectedDevice is Models.Scene)
                idx = ((Models.Scene)selectedDevice).idx;

            logList = new List<Log>();
            var logs = await App.ApiService.GetLogs(idx, selectedDevice is Models.Scene);
            if (logs == null || logs.result == null)
                logs = await App.ApiService.GetLogs(idx, false, true);

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