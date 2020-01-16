using NL.HNOGames.Domoticz.Data;
using NL.HNOGames.Domoticz.Resources;
using NL.HNOGames.Domoticz.ViewModels;
using NL.HNOGames.Domoticz.Views.Dialog;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.Views
{
    /// <summary>
    /// Defines the <see cref="ScenePage" />
    /// </summary>
    public partial class ScenePage
    {
        #region Variables

        /// <summary>
        /// Defines the _viewModel
        /// </summary>
        private readonly SceneViewModel _viewModel;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ScenePage"/> class.
        /// </summary>
        public ScenePage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SceneViewModel();
            _viewModel.SetListViewVisibilityMethod += DelegateListViewMethod;
            App.AddLog("Loading screen: Scenes");
            adView.IsVisible = !App.AppSettings.PremiumBought;

            searchIcon.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(OnSearchIconTapped) });
            searchBar.TextChanged += searchBar_TextChanged;
            searchBar.Cancelled += (s, e) => OnCancelled();
        }

        #endregion

        #region Private

        /// <summary>
        /// Set listview visibility (no items found)
        /// </summary>
        /// <param name="isvisible"></param>
        private void DelegateListViewMethod(bool isvisible)
        {
            listView.IsVisible = isvisible;
        }

        /// <summary>
        /// The OnSearchIconTapped
        /// </summary>
        private void OnSearchIconTapped()
        {
            BatchBegin();
            try
            {
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

        /// <summary>
        /// Show a actionsheet on item selected
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="args">The args<see cref="SelectedItemChangedEventArgs"/></param>
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Models.Scene;
            if (item == null)
                return;

            var actions = new List<string> { AppResources.favorite, AppResources.button_status_log };
            if (!string.IsNullOrEmpty(item.Timers) &&
                string.Compare(item.Timers, "true", StringComparison.OrdinalIgnoreCase) == 0)
                actions.Add(AppResources.button_status_timer);
            var tempDevice = new Models.Scene
            {
                idx = item.idx,
                Type = item.Type,
                Name = item.Name
            };

            var result = await DisplayActionSheet(item.Name, AppResources.cancel, null, actions.ToArray());
            if (result == AppResources.favorite)
                await SetFavorite(item);
            else if (result == AppResources.button_status_log)
                await PopupNavigation.Instance.PushAsync(new LogsPopup(tempDevice));
            else if (result == AppResources.button_status_timer)
                await PopupNavigation.Instance.PushAsync(new TimersPopup(tempDevice));
            listView.SelectedItem = null;
        }

        /// <summary>
        /// Set Favorite
        /// </summary>
        /// <param name="pair">The pair<see cref="Models.Scene"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task SetFavorite(Models.Scene pair)
        {
            if (!pair.FavoriteBoolean)
                App.ShowToast(pair.Name + " " + AppResources.favorite_added);
            else
                App.ShowToast(pair.Name + " " + AppResources.favorite_removed);
            var result = await App.ApiService.SetFavorite(pair.idx, true, !pair.FavoriteBoolean);
            if (!result)
                App.ShowToast(pair.Name + " " + AppResources.error_favorite);

            _viewModel.RefreshFavoriteCommand.Execute(null);
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
                    listView.ItemsSource = _viewModel.Devices;
                }
                else
                {
                    listView.ItemsSource = null;
                    listView.ItemsSource =
                        _viewModel.Devices.Where(i => i.Name.ToLower().Trim().Contains(filterText));
                }
            }
            catch (Exception)
            {
                listView.ItemsSource = null;
                listView.ItemsSource = _viewModel.Devices;
            }
        }

        /// <summary>
        /// Turn switch/device ON
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void btnOnButton_Clicked(object sender, EventArgs e)
        {
            if (_viewModel.OldData)
                return;
            Button oButton = (Button)sender;
            Models.Scene oDevice = (Models.Scene)oButton.BindingContext;
            App.ShowToast(AppResources.switch_on + ": " + oDevice.Name);

            await App.ApiService.SetSwitch(oDevice.idx, true,
                oDevice.Type == ConstantValues.Device.Scene.Type.GROUP ||
                oDevice.Type == ConstantValues.Device.Scene.Type.SCENE);
            _viewModel.RefreshActionCommand.Execute(null);
        }

        /// <summary>
        /// Turn switch/device OFF
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void btnOffButton_Clicked(object sender, EventArgs e)
        {
            var oButton = (Button)sender;
            var oDevice = (Models.Scene)oButton.BindingContext;
            App.ShowToast(AppResources.switch_off + ": " + oDevice.Name);

            await App.ApiService.SetSwitch(oDevice.idx, false,
                oDevice.Type == ConstantValues.Device.Scene.Type.GROUP ||
                oDevice.Type == ConstantValues.Device.Scene.Type.SCENE);
            _viewModel.RefreshActionCommand.Execute(null);
        }

        /// <summary>
        /// Toggle switch
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="ToggledEventArgs"/></param>
        private async void btnSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            var oSwitch = (Switch)sender;
            var oDevice = (Models.Scene)oSwitch.BindingContext;

            if (oSwitch.IsToggled == oDevice.StatusBoolean) return;
            if (oDevice.StatusBoolean)
                App.ShowToast(AppResources.switch_off + ": " + oDevice.Name);
            else
                App.ShowToast(AppResources.switch_on + ": " + oDevice.Name);
            await App.ApiService.SetSwitch(oDevice.idx, oSwitch.IsToggled,
                oDevice.Type == ConstantValues.Device.Scene.Type.GROUP ||
                oDevice.Type == ConstantValues.Device.Scene.Type.SCENE);
            _viewModel.RefreshActionCommand.Execute(null);
        }

        /// <summary>
        /// Set Selector value to Domoticz
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="FocusEventArgs"/></param>
        private void pSelector_Unfocused(object sender, FocusEventArgs e)
        {
        }

        /// <summary>
        /// Turn blinds ON/DOWN
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnBlindOnButton_Clicked(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Turn blinds OFF/UP
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnBlindOffButton_Clicked(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Turn blinds Stop
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnBlindStopButton_Clicked(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Slider value of the dimmer
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnLevelButton_Clicked(object sender, EventArgs e)
        {
        }

        #endregion

        /// <summary>
        /// On Appearing
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_viewModel.Devices == null || _viewModel.OldData)
                _viewModel.RefreshFavoriteCommand.Execute(null);
            //listView.RowHeight = 130;

            var info = await App.GetSunRiseInfoAsync();
            if (info != null)
                subtitle.Text = $"↑{info.Sunrise} ↓{info.Sunset}";
        }
    }
}
