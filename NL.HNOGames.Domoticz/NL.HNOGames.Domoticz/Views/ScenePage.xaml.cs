using System;
using System.Threading.Tasks;

using NL.HNOGames.Domoticz.ViewModels;

using Xamarin.Forms;
using NL.HNOGames.Domoticz.Controls;
using Acr.UserDialogs;
using NL.HNOGames.Domoticz.Resources;
using System.Linq;
using NL.HNOGames.Domoticz.Data;
using Rg.Plugins.Popup.Services;
using NL.HNOGames.Domoticz.Views.Dialog;
using System.Collections.Generic;

namespace NL.HNOGames.Domoticz.Views
{
    public partial class ScenePage : ContentPage
    {
        SceneViewModel viewModel;

        public ScenePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new SceneViewModel();
            App.AddLog("Loading screen: Scenes");
        }

        /// <summary>
        /// Show a actionsheet on item selected
        /// </summary>
        async Task OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Models.Scene;
            if (item == null)
                return;

            List<String> actions = new List<string>();
            actions.Add(AppResources.favorite);
            actions.Add(AppResources.button_status_log);

            if (!String.IsNullOrEmpty(item.Timers) && String.Compare(item.Timers, "true", StringComparison.OrdinalIgnoreCase) == 0)
                actions.Add(AppResources.button_status_timer);

            Models.Scene tempDevice = new Models.Scene();
            tempDevice.idx = item.idx;
            tempDevice.Type = item.Type;
            tempDevice.Name = item.Name;

            var result = await this.DisplayActionSheet(item.Name, AppResources.cancel, null, actions.ToArray());
            if (result == AppResources.favorite)
                await setFavorite(item);
            else if (result == AppResources.button_status_log)
                await PopupNavigation.PushAsync(new LogsPopup(tempDevice));
            else if (result == AppResources.button_status_timer)
                await PopupNavigation.PushAsync(new TimersPopup(tempDevice));
            listView.SelectedItem = null;
        }

        /// <summary>
        /// Set Favorite
        /// </summary>
        public async Task setFavorite(Models.Scene pair)
        {
            bool newValue = !pair.FavoriteBoolean;
            if (newValue)
                App.ShowToast(pair.Name + " " + AppResources.favorite_added);
            else
                App.ShowToast(pair.Name + " " + AppResources.favorite_removed);

            var result = await App.ApiService.SetFavorite(pair.idx, true, newValue);
            if (!result)
                App.ShowToast(pair.Name + " " + AppResources.error_favorite);

            viewModel.RefreshFavoriteCommand.Execute(null);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Devices == null || viewModel.OldData)
                viewModel.RefreshFavoriteCommand.Execute(null);
            listView.RowHeight = 130;
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
                    listView.ItemsSource = this.viewModel.Devices;
                }
                else
                {
                    listView.ItemsSource = null;
                    listView.ItemsSource = this.viewModel.Devices.Where(i => i.Name.ToLower().Trim().Contains(filterText));
                }
            }
            catch (Exception)
            {
                listView.ItemsSource = null;
                listView.ItemsSource = this.viewModel.Devices;
            }
        }

        #region On Off Switches

        /// <summary>
        /// Turn switch/device ON
        /// </summary>
        private async Task btnOnButton_Clicked(object sender, EventArgs e)
        {
            if (viewModel.OldData)
                return;
            Button oButton = (Button)sender;
            Models.Scene oDevice = (Models.Scene)oButton.BindingContext;
            App.ShowToast(AppResources.switch_on + ": " + oDevice.Name);

            await App.ApiService.SetSwitch(oDevice.idx, true, oDevice.Type == ConstantValues.Device.Scene.Type.GROUP || oDevice.Type == ConstantValues.Device.Scene.Type.SCENE ? true : false);
            viewModel.RefreshFavoriteCommand.Execute(null);
        }

        /// <summary>
        /// Turn switch/device OFF
        /// </summary>
        private async Task btnOffButton_Clicked(object sender, EventArgs e)
        {
            Button oButton = (Button)sender;
            Models.Scene oDevice = (Models.Scene)oButton.BindingContext;
            App.ShowToast(AppResources.switch_off + ": " + oDevice.Name);

            await App.ApiService.SetSwitch(oDevice.idx, false, oDevice.Type == ConstantValues.Device.Scene.Type.GROUP || oDevice.Type == ConstantValues.Device.Scene.Type.SCENE ? true : false);
            viewModel.RefreshFavoriteCommand.Execute(null);
        }

        /// <summary>
        /// Toggle switch
        /// </summary>
        private async Task btnSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Switch oSwitch = (Switch)sender;
            Models.Scene oDevice = (Models.Scene)oSwitch.BindingContext;

            if (oSwitch.IsToggled != oDevice.StatusBoolean)
            {
                if (oDevice.StatusBoolean)
                    App.ShowToast(AppResources.switch_off + ": " + oDevice.Name);
                else
                    App.ShowToast(AppResources.switch_on + ": " + oDevice.Name);

                await App.ApiService.SetSwitch(oDevice.idx, oSwitch.IsToggled, oDevice.Type == ConstantValues.Device.Scene.Type.GROUP || oDevice.Type == ConstantValues.Device.Scene.Type.SCENE ? true : false);
                viewModel.RefreshFavoriteCommand.Execute(null);
            }
        }

        #endregion On Off Switches


        #region Selector

        /// <summary>
        /// Set Selector value to Domoticz
        /// </summary>
        private void pSelector_Unfocused(object sender, FocusEventArgs e)
        {
        }

        /// <summary>
        /// Set Selector value to Domoticz
        /// </summary>
        public void NewSelectorValueAsync(Models.Scene pair, Picker oPicker)
        {
        }

        #endregion Selector

        #region Blinds

        /// <summary>
        /// Turn blinds ON/DOWN
        /// </summary>
        private void btnBlindOnButton_Clicked(object sender, EventArgs e)
        { }

        /// <summary>
        /// Turn blinds OFF/UP
        /// </summary>
        private void btnBlindOffButton_Clicked(object sender, EventArgs e)
        {}

        /// <summary>
        /// Turn blinds Stop
        /// </summary>
        private void btnBlindStopButton_Clicked(object sender, EventArgs e)
        { }

        #endregion Blinds


        #region Dimmer

        /// <summary>
        /// Slider value of the dimmer
        /// </summary>
        private void btnLevelButton_Clicked(object sender, EventArgs e)
        {}

        #endregion Dimmer

    }
}
