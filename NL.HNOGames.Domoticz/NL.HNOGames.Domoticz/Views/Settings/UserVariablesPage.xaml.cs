using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using System.Linq;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    public partial class UserVariablesPage
    {
        private List<UserVariable> _userList;

        public UserVariablesPage()
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
            _userList = new List<UserVariable>();
            var uservars = await App.ApiService.GetUserVariables();

            if (uservars?.result != null)
            {
                foreach (var n in uservars.result)
                    _userList.Add(n);
                listView.ItemsSource = _userList;
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
                    listView.ItemsSource = _userList;
                }
                else
                {
                    listView.ItemsSource = null;
                    listView.ItemsSource = _userList.Where(i => i.Name.ToLower().Trim().Contains(filterText));
                }
            }
            catch (Exception)
            {
                listView.ItemsSource = null;
                listView.ItemsSource = _userList;
            }
        }
    }
}