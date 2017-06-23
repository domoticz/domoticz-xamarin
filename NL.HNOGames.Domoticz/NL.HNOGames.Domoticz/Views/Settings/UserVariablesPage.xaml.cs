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
    public partial class UserVariablesPage : ContentPage
    {
        private List<UserVariable> userList;

        public UserVariablesPage()
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
            userList = new List<UserVariable>();
            var uservars = await App.ApiService.GetUserVariables();

            if (uservars != null && uservars.result != null)
            {
                foreach (UserVariable n in uservars.result)
                    userList.Add(n);
                listView.ItemsSource = userList;
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
                    listView.ItemsSource = userList;
                }
                else
                {
                    listView.ItemsSource = null;
                    listView.ItemsSource = userList.Where(i => i.Name.ToLower().Trim().Contains(filterText));
                }
            }
            catch (Exception)
            {
                listView.ItemsSource = null;
                listView.ItemsSource = userList;
            }
        }
    }
}