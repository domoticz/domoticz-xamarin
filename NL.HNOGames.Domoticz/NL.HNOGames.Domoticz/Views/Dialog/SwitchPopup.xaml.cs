using Acr.UserDialogs;
using NL.HNOGames.Domoticz.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.Views.Dialog
{
    /// <summary>
    /// Defines the <see cref="SwitchPopup" />
    /// </summary>
    public partial class SwitchPopup
    {
        #region Variables

        private List<Models.Device> switchList;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchPopup"/> class.
        /// </summary>
        public SwitchPopup()
        {
            InitializeComponent();

            searchIcon.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(OnSearchIconTapped) });
            searchBar.TextChanged += searchBar_TextChanged;
            searchBar.Cancelled += (s, e) => OnCancelled();
        }

        #endregion

        #region Delegates

        /// <summary>
        /// The DeviceSelected
        /// </summary>
        /// <param name="device">The device<see cref="Models.Device"/></param>
        /// <param name="pasword">The pasword<see cref="string"/></param>
        /// <param name="value">The value<see cref="string"/></param>
        public delegate void DeviceSelected(Models.Device device, string pasword, string value);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the DeviceSelectedMethod
        /// </summary>
        public DeviceSelected DeviceSelectedMethod { get; set; }

        #endregion

        #region Private

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
                    listView.ItemsSource = switchList;
                }
                else
                {
                    listView.ItemsSource = null;
                    listView.ItemsSource = switchList.Where(i => i.Name.ToLower().Trim().Contains(filterText));
                }
            }
            catch (Exception)
            {
                listView.ItemsSource = null;
                listView.ItemsSource = switchList;
            }
        }

        /// <summary>
        /// The OnItemSelected
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="args">The args<see cref="SelectedItemChangedEventArgs"/></param>
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Models.Device;
            if (item == null)
                return;
            listView.SelectedItem = null;

            string password = null;
            string value = null;
            if (item.Protected)
            {
                var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password,
                    inputType: InputType.Password);
                await Task.Delay(500);
                if (r.Ok)
                    password = r.Text;
                else
                    return;
            }

            if (item.SwitchTypeVal == Data.ConstantValues.Device.Type.Value.SELECTOR)
            {
                //show value popup
                if (item.LevelNamesArray != null && item.LevelNames.Length > 0)
                {
                    value = await DisplayActionSheet(AppResources.selector_value, AppResources.cancel, null,
                        item.LevelNamesArray);
                    if (string.IsNullOrEmpty(value))
                        return;
                }
            }

            DeviceSelectedMethod?.Invoke(item, password, value);
            await Navigation.PopAsync();
        }

        /// <summary>
        /// The ExecuteGetSwitchesCommand
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task ExecuteGetSwitchesCommand()
        {
            listView.IsRefreshing = true;
            var result = await App.ApiService.GetDevices(0, "all");
            if (result == null)
                return;

            switchList = result?.result?.Where(Data.ConstantValues.CanHandleAutomatedToggle)?.ToList();
            listView.ItemsSource = switchList;
            listView.IsRefreshing = false;
        }

        /// <summary>
        /// The btnCancel_Clicked
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
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
            new Command(async () => await ExecuteGetSwitchesCommand()).Execute(null);
        }
    }
}
