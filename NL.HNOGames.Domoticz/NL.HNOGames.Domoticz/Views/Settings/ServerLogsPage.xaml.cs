using System;

using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using System.Linq;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    public partial class ServerLogsPage
    {
        private List<ServerLog> _logList;

        public ServerLogsPage()
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

        private async Task ExecuteLoadLogsCommand()
        {
            App.ShowLoading();
            _logList = new List<ServerLog>();
            var logs = await App.ApiService.GetServerLogs();

            if (logs?.result != null)
            {
                foreach (var n in logs.result)
                    _logList.Add(n);
                _logList.Reverse();
                listView.ItemsSource = _logList;
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
                    listView.ItemsSource = _logList;
                }
                else
                {
                    listView.ItemsSource = null;
                    listView.ItemsSource = _logList.Where(i => i.message.ToLower().Trim().Contains(filterText));
                }
            }
            catch (Exception)
            {
                listView.ItemsSource = null;
                listView.ItemsSource = _logList;
            }
        }
    }
}