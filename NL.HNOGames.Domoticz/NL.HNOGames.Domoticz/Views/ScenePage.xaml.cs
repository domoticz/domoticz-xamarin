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

            Models.Device tempDevice = new Models.Device();
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
                UserDialogs.Instance.Toast(pair.Name + " " + AppResources.favorite_added);
            else
                UserDialogs.Instance.Toast(pair.Name + " " + AppResources.favorite_removed);

            var result = await App.ApiService.SetFavorite(pair.idx, true, newValue);
            if (!result)
                UserDialogs.Instance.Toast(pair.Name + " " + AppResources.error_favorite);

            viewModel.RefreshFavoriteCommand.Execute(null);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Devices == null || viewModel.OldData)
                viewModel.RefreshFavoriteCommand.Execute(null);

            listView.HeightRequest = 130;
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



        #region Selector

        /// <summary>
        /// Set Selector value to Domoticz
        /// </summary>
        private async Task pSelector_Unfocused(object sender, FocusEventArgs e)
        {
            if (viewModel.OldData)
                return;
            Picker oPicker = (Picker)sender;
            await NewSelectorValueAsync((Models.Device)oPicker.BindingContext, oPicker);
        }

        /// <summary>
        /// Set Selector value to Domoticz
        /// </summary>
        public async Task NewSelectorValueAsync(Models.Device pair, Picker oPicker)
        {
            int newValue = 0;
            if (oPicker.SelectedIndex > 0)
                newValue = oPicker.SelectedIndex * 10;

            if (pair.LevelInt != newValue)
            {
                await App.ApiService.SetDimmer(pair.idx, newValue);
                viewModel.RefreshFavoriteCommand.Execute(null);
            }
        }

        #endregion Selector


        #region On Off Switches

        /// <summary>
        /// Turn switch/device ON
        /// </summary>
        private async Task btnOnButton_Clicked(object sender, EventArgs e)
        {
            if (viewModel.OldData)
                return;
            Button oButton = (Button)sender;
            Models.Device oDevice = (Models.Device)oButton.BindingContext;
            UserDialogs.Instance.Toast(AppResources.switch_on + ": " + oDevice.Name);

            await App.ApiService.SetSwitch(oDevice.idx, true, oDevice.Type == ConstantValues.Device.Scene.Type.GROUP || oDevice.Type == ConstantValues.Device.Scene.Type.SCENE ? true : false);
            viewModel.RefreshFavoriteCommand.Execute(null);
        }

        /// <summary>
        /// Turn switch/device OFF
        /// </summary>
        private async Task btnOffButton_Clicked(object sender, EventArgs e)
        {
            Button oButton = (Button)sender;
            Models.Device oDevice = (Models.Device)oButton.BindingContext;
            UserDialogs.Instance.Toast(AppResources.switch_off + ": " + oDevice.Name);

            await App.ApiService.SetSwitch(oDevice.idx, false, oDevice.Type == ConstantValues.Device.Scene.Type.GROUP || oDevice.Type == ConstantValues.Device.Scene.Type.SCENE ? true : false);
            viewModel.RefreshFavoriteCommand.Execute(null);
        }
        /// <summary>
        /// Toggle switch
        /// </summary>
        private async Task btnSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Switch oSwitch = (Switch)sender;
            Models.Device oDevice = (Models.Device)oSwitch.BindingContext;

            if (oSwitch.IsToggled != oDevice.StatusBoolean)
            {
                if (oDevice.StatusBoolean)
                    UserDialogs.Instance.Toast(AppResources.switch_off + ": " + oDevice.Name);
                else
                    UserDialogs.Instance.Toast(AppResources.switch_on + ": " + oDevice.Name);

                await App.ApiService.SetSwitch(oDevice.idx, oSwitch.IsToggled, oDevice.Type == ConstantValues.Device.Scene.Type.GROUP || oDevice.Type == ConstantValues.Device.Scene.Type.SCENE ? true : false);
                viewModel.RefreshFavoriteCommand.Execute(null);
            }
        }
        #endregion On Off Switches


        #region Set (Temperature)

        /// <summary>
        /// set a specific value in a switch (temperature for example)
        /// </summary>
        private async Task txtSetValue_Completed(object sender, EventArgs e)
        {
            if (viewModel.OldData)
                return;
            Entry oEntry = (Entry)sender;
            Models.Device oDevice = (Models.Device)oEntry.BindingContext;
            if (Helpers.UsefulBits.IsNumeric(oEntry.Text))
            {
                float newValue = float.Parse(oEntry.Text);
                await App.ApiService.SetPoint(oDevice.idx, newValue, float.Parse(oDevice.SetPoint));
                viewModel.RefreshFavoriteCommand.Execute(null);
            }
        }

        #endregion Set (Temperature)


        #region Blinds

        /// <summary>
        /// Turn blinds ON/DOWN
        /// </summary>
        private async Task btnBlindOnButton_Clicked(object sender, EventArgs e)
        {
            if (viewModel.OldData)
                return;
            Button oButton = (Button)sender;
            Models.Device oDevice = (Models.Device)oButton.BindingContext;

            int action = ConstantValues.Device.Switch.Action.ON;
            if (oDevice.SwitchTypeVal == ConstantValues.Device.Type.Value.BLINDINVERTED)
            {
                action = ConstantValues.Device.Switch.Action.OFF;
                UserDialogs.Instance.Toast(AppResources.blind_up + ": " + oDevice.Name);
            }
            else
                UserDialogs.Instance.Toast(AppResources.blind_down + ": " + oDevice.Name);

            await App.ApiService.SetBlind(oDevice.idx, action);
            viewModel.RefreshFavoriteCommand.Execute(null);
        }

        /// <summary>
        /// Turn blinds OFF/UP
        /// </summary>
        private async Task btnBlindOffButton_Clicked(object sender, EventArgs e)
        {
            if (viewModel.OldData)
                return;
            Button oButton = (Button)sender;
            Models.Device oDevice = (Models.Device)oButton.BindingContext;

            int action = ConstantValues.Device.Switch.Action.OFF;
            if (oDevice.SwitchTypeVal == ConstantValues.Device.Type.Value.BLINDINVERTED)
            {
                action = ConstantValues.Device.Switch.Action.ON;
                UserDialogs.Instance.Toast(AppResources.blind_down + ": " + oDevice.Name);
            }
            else
                UserDialogs.Instance.Toast(AppResources.blind_up + ": " + oDevice.Name);

            await App.ApiService.SetBlind(oDevice.idx, action);
            viewModel.RefreshFavoriteCommand.Execute(null);
        }

        /// <summary>
        /// Turn blinds Stop
        /// </summary>
        private async Task btnBlindStopButton_Clicked(object sender, EventArgs e)
        {
            if (viewModel.OldData)
                return;
            Button oButton = (Button)sender;
            Models.Device oDevice = (Models.Device)oButton.BindingContext;
            UserDialogs.Instance.Toast(AppResources.blind_stop + ": " + oDevice.Name);

            await App.ApiService.SetBlind(oDevice.idx, ConstantValues.Device.Blind.Action.STOP);
            viewModel.RefreshFavoriteCommand.Execute(null);
        }

        #endregion Blinds


        #region Dimmer

        /// <summary>
        /// Slider value of the dimmer
        /// </summary>
        private async Task btnLevelButton_Clicked(object sender, EventArgs e)
        {
            Button oButton = (Button)sender;
            Models.Device oDevice = (Models.Device)oButton.BindingContext;

            SliderPopup oSlider = new SliderPopup(oDevice, viewModel.RefreshFavoriteCommand);
            await PopupNavigation.PushAsync(oSlider);
        }

        #endregion Dimmer

    }
}
