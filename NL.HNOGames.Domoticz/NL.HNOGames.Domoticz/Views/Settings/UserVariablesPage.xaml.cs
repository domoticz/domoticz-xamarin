using Acr.UserDialogs;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    /// <summary>
    /// Defines the <see cref="UserVariablesPage" />
    /// </summary>
    public partial class UserVariablesPage
    {
        #region Variables

        /// <summary>
        /// Defines the _userList
        /// </summary>
        private List<UserVariable> _userList;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UserVariablesPage"/> class.
        /// </summary>
        public UserVariablesPage()
        {
            InitializeComponent();

            searchIcon.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(OnSearchIconTapped) });
            searchBar.TextChanged += searchBar_TextChanged;
            searchBar.Cancelled += (s, e) => OnCancelled();
        }

        #endregion

        #region Private

        /// <summary>
        /// The OnItemSelected
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="args">The args<see cref="SelectedItemChangedEventArgs"/></param>
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            try
            {
                if (!(args.SelectedItem is UserVariable selectedVar)) return;

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
            catch (Exception)
            {
                App.ShowToast(AppResources.var_input_error);
                new Command(async () => await ExecuteLoadLogsCommand()).Execute(null);
            }
        }

        /// <summary>
        /// The ValidateInput
        /// </summary>
        /// <param name="input">The input<see cref="String"/></param>
        /// <param name="type">The type<see cref="String"/></param>
        /// <returns>The <see cref="bool"/></returns>
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

        /// <summary>
        /// The ExecuteLoadLogsCommand
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
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
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="TextChangedEventArgs"/></param>
        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
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

        /// <summary>
        /// The OnSearchIconTapped
        /// </summary>
        private void OnSearchIconTapped()
        {
            BatchBegin();
            try
            {
                NavigationPage.SetHasBackButton(this, false);
                titleLayout.IsVisible = false;
                searchIcon.IsVisible = false;
                searchBar.IsVisible = true;
                searchBar.Focus();
            }
            finally
            {
                BatchCommit();
            }
        }

        /// <summary>
        /// The OnCancelled
        /// </summary>
        private void OnCancelled()
        {
            BatchBegin();
            try
            {
                NavigationPage.SetHasBackButton(this, true);
                searchBar.IsVisible = false;
                searchBar.Text = string.Empty;
                titleLayout.IsVisible = true;
                searchIcon.IsVisible = true;
            }
            finally
            {
                BatchCommit();
            }
        }

        #endregion

        /// <summary>
        /// The OnAppearing
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            new Command(async () => await ExecuteLoadLogsCommand()).Execute(null);
        }
    }
}
