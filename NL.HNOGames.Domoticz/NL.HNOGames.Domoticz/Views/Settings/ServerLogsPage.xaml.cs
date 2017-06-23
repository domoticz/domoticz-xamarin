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
    public partial class ServerLogsPage : ContentPage
    {
        private List<ServerLog> logList;

        public ServerLogsPage()
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
            logList = new List<ServerLog>();
            var logs = await App.ApiService.GetServerLogs();

            if (logs != null && logs.result != null)
            {
                foreach (ServerLog n in logs.result)
                    logList.Add(n);
                logList.Reverse();
                listView.ItemsSource = logList;
                App.HideLoading();
            }
            else
            {
                App.HideLoading();
                UserDialogs.Instance.Toast(AppResources.error_logs);
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
                    listView.ItemsSource = logList;
                }
                else
                {
                    listView.ItemsSource = null;
                    listView.ItemsSource = logList.Where(i => i.message.ToLower().Trim().Contains(filterText));
                }
            }
            catch (Exception)
            {
                listView.ItemsSource = null;
                listView.ItemsSource = logList;
            }
        }
    }
}