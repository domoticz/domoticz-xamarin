using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using System.Linq;
using Acr.UserDialogs;
using System.Globalization;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    public partial class UserVariablesPage
    {
        private List<UserVariable> _userList;

        public UserVariablesPage()
        {
            InitializeComponent();
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            try
            {
                var selectedVar = args.SelectedItem as Models.UserVariable;
                if (selectedVar == null) return;

                var r = await UserDialogs.Instance.PromptAsync(selectedVar.Name + " -> " + selectedVar.TypeValue, AppResources.title_vars,
                    inputType: selectedVar.Type == "0" || selectedVar.Type == "1" ? InputType.Number : InputType.Default);
                await Task.Delay(500);
                if (r.Ok)
                {
                    App.ShowLoading();
                    if (ValidateInput(r.Text, selectedVar.Type))
                    {
                        var result = await App.ApiService.SetUserVariable(selectedVar.idx, selectedVar.Name, selectedVar.Type, r.Text);
                        if (!result)
                            App.ShowToast(AppResources.var_input_error);
                    }
                    else
                        App.ShowToast(AppResources.var_input);
                    new Command(async () => await ExecuteLoadLogsCommand()).Execute(null);
                }
                listView.SelectedItem = null;
            }
            catch (Exception) {
                App.ShowToast(AppResources.var_input_error);
                new Command(async () => await ExecuteLoadLogsCommand()).Execute(null);
            }
        }

        private bool ValidateInput(String input, String type)
        {
            try
            {
                switch (type)
                {
                    case "0":
                        Convert.ToInt32(input);
                        break;
                    case "1":
                        float.Parse(input);
                        break;
                    case "3":
                        DateTime.ParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        break;
                    case "4":
                        DateTime.ParseExact(input, "HH:mm", CultureInfo.InvariantCulture);
                        break;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
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