using System;
using System.Threading.Tasks;

using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.ViewModels;

using Xamarin.Forms;
using NL.HNOGames.Domoticz.Controls;
using Acr.UserDialogs;
using NL.HNOGames.Domoticz.Resources;
using System.Linq;
using System.Collections.Generic;
using NL.HNOGames.Domoticz.Data;
using Rg.Plugins.Popup.Services;
using NL.HNOGames.Domoticz.Views.Dialog;
using System.Globalization;
using System.Threading;
using System.Diagnostics;

namespace NL.HNOGames.Domoticz.Views
{
    public partial class DashboardPage : ContentPage
    {
        DashboardViewModel viewModel;

        public DashboardPage(DashboardViewModel.ScreenType screentype, Plan plan = null)
        {
            InitializeComponent();
            BindingContext = viewModel = new DashboardViewModel(screentype, plan);
            App.AddLog("Loading screen: " + screentype.ToString());
        }

        /// <summary>
        /// Show a actionsheet on item selected
        /// </summary>
        async Task OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Models.Device;
            if (item == null)
                return;
            await ShowActionMenu(item);

            listView.SelectedItem = null;
        }

        /// <summary>
        /// Show menu for device
        /// </summary>
        private async Task ShowActionMenu(Models.Device item)
        {
            List<String> actions = AddActionMenuItems(item);
            var result = await this.DisplayActionSheet(item.Name, AppResources.cancel, null, actions.ToArray());

            if (result == AppResources.favorite)
                await setFavorite(item);
            else if (result == AppResources.button_status_notifications)
                await PopupNavigation.PushAsync(new NotificationsPopup(item));
            else if (result == AppResources.button_status_timer)
                await PopupNavigation.PushAsync(new TimersPopup(item));
            else if (result == AppResources.button_status_log)
                await PopupNavigation.PushAsync(new LogsPopup(item));
            else if (result == AppResources.wizard_graph)
            {
                if (viewModel.screenType == DashboardViewModel.ScreenType.Temperature)
                    await Navigation.PushAsync(new GraphTabbedPage(item, "temp"));
                else if (viewModel.screenType == DashboardViewModel.ScreenType.Weather)
                {
                    String graphType = item.TypeImg
                       .ToLower()
                       .Replace("temperature", "temp")
                       .Replace("visibility", "counter");
                    await Navigation.PushAsync(new GraphTabbedPage(item, graphType));
                }
                else if (viewModel.screenType == DashboardViewModel.ScreenType.Utilities)
                {
                    String graphType = item.SubType
                            .Replace("Electric", "counter")
                            .Replace("kWh", "counter")
                            .Replace("Gas", "counter")
                            .Replace("Energy", "counter")
                            .Replace("Voltcraft", "counter")
                            .Replace("SetPoint", "temp")
                            .Replace("YouLess counter", "counter");
                    if (graphType.Contains("counter"))
                        graphType = "counter";
                    await Navigation.PushAsync(new GraphTabbedPage(item, graphType));
                }
            }
            else if (result == AppResources.security_disarm || result == AppResources.security_arm_home || result == AppResources.security_arm_away)
            {
                var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password, inputType: InputType.NumericPassword);
                await Task.Delay(500);
                if (r.Ok && !String.IsNullOrEmpty(r.Text))
                {
                    int status = ConstantValues.Security.Status.DISARM;
                    if (result == AppResources.security_arm_home)
                        status = ConstantValues.Security.Status.ARMHOME;
                    else if (result == AppResources.security_arm_away)
                        status = ConstantValues.Security.Status.ARMAWAY;

                    String md5Pass = Helpers.UsefulBits.GetMD5String(r.Text);
                    if (!await App.ApiService.SetSecurityPanel(status, md5Pass))
                        App.ShowToast(AppResources.security_generic_error);
                    else
                        RefreshListView();
                }
            }
        }

        /// <summary>
        /// Init list of menu items per device
        /// </summary>
        private List<string> AddActionMenuItems(Models.Device item)
        {
            List<string> actions = new List<string>();
            if (String.Compare(item.SubType, ConstantValues.Device.SubType.Name.SECURITYPANEL, StringComparison.OrdinalIgnoreCase) == 0)
            {
                actions.Add(AppResources.security_arm_away);
                actions.Add(AppResources.security_arm_home);
                actions.Add(AppResources.security_disarm);
            }

            actions.Add(AppResources.favorite);
            if (!String.IsNullOrEmpty(item.Notifications) && String.Compare(item.Notifications, "true", StringComparison.OrdinalIgnoreCase) == 0)
                actions.Add(AppResources.button_status_notifications);

            if (!String.IsNullOrEmpty(item.Timers) && String.Compare(item.Timers, "true", StringComparison.OrdinalIgnoreCase) == 0)
                actions.Add(AppResources.button_status_timer);

            if (viewModel.screenType == DashboardViewModel.ScreenType.Switches)
                actions.Add(AppResources.button_status_log);

            if (viewModel.screenType == DashboardViewModel.ScreenType.Temperature ||
                viewModel.screenType == DashboardViewModel.ScreenType.Weather ||
                viewModel.screenType == DashboardViewModel.ScreenType.Utilities)
                actions.Add(AppResources.wizard_graph);

            return actions;
        }

        /// <summary>
        /// On Appearing of the screen
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Devices == null || viewModel.OldData)
                viewModel.RefreshFavoriteCommand.Execute(null);

            if (viewModel.screenType != DashboardViewModel.ScreenType.Dashboard || App.AppSettings.ShowExtraData)
                listView.RowHeight = 130;
            else
                listView.RowHeight = 80;
        }

        /// <summary>
        /// set favorite true / false
        /// </summary>
        private async Task TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (viewModel.OldData)
                return;
            TintedCachedImage oImage = (TintedCachedImage)sender;
            oImage.IsVisible = false;
            await setFavorite((Models.Device)oImage.BindingContext);
            oImage.IsVisible = true;
        }

        /// <summary>
        /// Set Favorite
        /// </summary>
        public async Task setFavorite(Models.Device pair)
        {
            bool newValue = !pair.FavoriteBoolean;
            if (newValue)
                App.ShowToast(pair.Name + " " + AppResources.favorite_added);
            else
                App.ShowToast(pair.Name + " " + AppResources.favorite_removed);

            var result = await App.ApiService.SetFavorite(pair.idx, pair.IsScene, newValue);
            if (!result)
                App.ShowToast(pair.Name + " " + AppResources.error_favorite);
            RefreshListView();
        }

        /// <summary>
        /// Refresh ListView
        /// </summary>
        private void RefreshListView()
        {
            viewModel.RefreshFavoriteCommand.Execute(null);
            sbSearch.Text = string.Empty;
        }

        /// <summary>
        /// Filter changed
        /// </summary>
        private void sbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == string.Empty)
            {
                App.AddLog("Cancel Pressed");
                listView.ItemsSource = this.viewModel.Devices;
                sbSearch.Unfocus();
            }
            else
            {
                try
                {
                    String filterText = e.NewTextValue.ToLower().Trim();
                    if (filterText == string.Empty)
                        listView.ItemsSource = this.viewModel.Devices;
                    else
                        listView.ItemsSource = this.viewModel.Devices.Where(i => i.Name.ToLower().Trim().Contains(filterText));
                }
                catch (Exception)
                {
                    listView.ItemsSource = this.viewModel.Devices;
                }
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
                if (pair.Protected)
                {
                    var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password, inputType: InputType.Password);
                    await Task.Delay(500);
                    if (r.Ok)
                    {
                        var result = await App.ApiService.SetDimmer(pair.idx, newValue, r.Text);
                        if (!result)
                            App.ShowToast(AppResources.security_wrong_code);
                        RefreshListView();
                    }
                }
                else
                {
                    await App.ApiService.SetDimmer(pair.idx, newValue);
                    RefreshListView();
                }
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
            if (oDevice.Protected)
            {
                var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password, inputType: InputType.Password);
                await Task.Delay(500);
                if (r.Ok)
                {
                    App.ShowToast(AppResources.switch_on + ": " + oDevice.Name);
                    var result = await App.ApiService.SetSwitch(oDevice.idx, true, oDevice.Type == ConstantValues.Device.Scene.Type.GROUP || oDevice.Type == ConstantValues.Device.Scene.Type.SCENE ? true : false, r.Text);
                    if (!result)
                        App.ShowToast(AppResources.security_wrong_code);

                    RefreshListView();
                }
            }
            else
            {
                App.ShowToast(AppResources.switch_on + ": " + oDevice.Name);
                var result = await App.ApiService.SetSwitch(oDevice.idx, true, oDevice.Type == ConstantValues.Device.Scene.Type.GROUP || oDevice.Type == ConstantValues.Device.Scene.Type.SCENE ? true : false);

                RefreshListView();
            }
        }

        /// <summary>
        /// Turn switch/device OFF
        /// </summary>
        private async Task btnOffButton_Clicked(object sender, EventArgs e)
        {
            Button oButton = (Button)sender;
            Models.Device oDevice = (Models.Device)oButton.BindingContext;
            if (oDevice.Protected)
            {
                var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password, inputType: InputType.Password);
                await Task.Delay(500);
                if (r.Ok)
                {
                    App.ShowToast(AppResources.switch_off + ": " + oDevice.Name);
                    var result = await App.ApiService.SetSwitch(oDevice.idx, false, oDevice.Type == ConstantValues.Device.Scene.Type.GROUP || oDevice.Type == ConstantValues.Device.Scene.Type.SCENE ? true : false, r.Text);
                    if (!result)
                        App.ShowToast(AppResources.security_wrong_code);
                    RefreshListView();
                }
            }
            else
            {
                App.ShowToast(AppResources.switch_off + ": " + oDevice.Name);
                var result = await App.ApiService.SetSwitch(oDevice.idx, false, oDevice.Type == ConstantValues.Device.Scene.Type.GROUP || oDevice.Type == ConstantValues.Device.Scene.Type.SCENE ? true : false);
                RefreshListView();
            }
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
                if (oDevice.Protected)
                {
                    var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password, inputType: InputType.Password);
                    await Task.Delay(500);
                    if (r.Ok)
                    {
                        if (oDevice.StatusBoolean)
                            App.ShowToast(AppResources.switch_off + ": " + oDevice.Name);
                        else
                            App.ShowToast(AppResources.switch_on + ": " + oDevice.Name);

                        var result = await App.ApiService.SetSwitch(oDevice.idx, oSwitch.IsToggled,
                            oDevice.Type == ConstantValues.Device.Scene.Type.GROUP || oDevice.Type == ConstantValues.Device.Scene.Type.SCENE ? true : false, r.Text);
                        if (!result)
                            App.ShowToast(AppResources.security_wrong_code);

                        RefreshListView();
                    }
                }
                else
                {
                    if (oDevice.StatusBoolean)
                        App.ShowToast(AppResources.switch_off + ": " + oDevice.Name);
                    else
                        App.ShowToast(AppResources.switch_on + ": " + oDevice.Name);

                    var result = await App.ApiService.SetSwitch(oDevice.idx, oSwitch.IsToggled, oDevice.Type == ConstantValues.Device.Scene.Type.GROUP || oDevice.Type == ConstantValues.Device.Scene.Type.SCENE ? true : false);
                    RefreshListView();
                }
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
                await SetPointValue(oEntry, oDevice);
        }

        /// <summary>
        /// Setpoint action 
        /// </summary>
        private async Task SetPointValue(Entry oEntry, Models.Device oDevice, Boolean refresh = true)
        {
            Double newValue = Double.Parse(oEntry.Text, CultureInfo.InvariantCulture);
            App.AddLog("Set idx " + oDevice.idx + " to " + newValue);
            if (oDevice.Protected)
            {
                var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password, inputType: InputType.Password);
                await Task.Delay(500);
                if (r.Ok)
                {
                    var result = await App.ApiService.SetPoint(oDevice.idx, newValue, Double.Parse(oDevice.SetPoint, CultureInfo.InvariantCulture), r.Text);
                    if (!result)
                        App.ShowToast(AppResources.security_wrong_code);
                    if (refresh) RefreshListView();
                }
            }
            else
            {
                var result = await App.ApiService.SetPoint(oDevice.idx, newValue, Double.Parse(oDevice.SetPoint, CultureInfo.InvariantCulture));
                if (refresh) RefreshListView();
            }
        }

        /// <summary>
        /// min 0.5 from temp
        /// </summary>
        private async Task btnMinTemp_Clicked(object sender, EventArgs e)
        {
            if (viewModel.OldData)
                return;
            Button btnMinTemp = (Button)sender;
            var entry = ((Entry)((StackLayout)btnMinTemp.Parent).Children.Where(o => o.GetType() == typeof(Entry)).FirstOrDefault());

            var currentValue = Double.Parse(entry.Text, CultureInfo.InvariantCulture);
            var changedValue = currentValue - 0.5;
            entry.Text = changedValue.ToString(CultureInfo.InvariantCulture);

            await SetPointValue(entry, (Models.Device)btnMinTemp.BindingContext, false);
        }

        /// <summary>
        /// Add 0.5 to temp
        /// </summary>
        private async Task btnPlusTemp_Clicked(object sender, EventArgs e)
        {
            if (viewModel.OldData)
                return;
            Button btnPlusTemp = (Button)sender;
            var entry = ((Entry)((StackLayout)btnPlusTemp.Parent).Children.Where(o => o.GetType() == typeof(Entry)).FirstOrDefault());

            var currentValue = Double.Parse(entry.Text, CultureInfo.InvariantCulture);
            var changedValue = currentValue + 0.5;
            entry.Text = changedValue.ToString(CultureInfo.InvariantCulture);

            await SetPointValue(entry, (Models.Device)btnPlusTemp.BindingContext, false);
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
                App.ShowToast(AppResources.blind_up + ": " + oDevice.Name);
            }
            else
                App.ShowToast(AppResources.blind_down + ": " + oDevice.Name);

            if (oDevice.Protected)
            {
                var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password, inputType: InputType.Password);
                await Task.Delay(500);
                if (r.Ok)
                {
                    var result = await App.ApiService.SetBlind(oDevice.idx, action, r.Text);
                    if (!result)
                        App.ShowToast(AppResources.security_wrong_code);
                    RefreshListView();
                }
            }
            else
            {
                var result = await App.ApiService.SetBlind(oDevice.idx, action);
                RefreshListView();
            }
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
                App.ShowToast(AppResources.blind_down + ": " + oDevice.Name);
            }
            else
                App.ShowToast(AppResources.blind_up + ": " + oDevice.Name);

            if (oDevice.Protected)
            {
                var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password, inputType: InputType.Password);
                await Task.Delay(500);
                if (r.Ok)
                {
                    var result = await App.ApiService.SetBlind(oDevice.idx, action, r.Text);
                    if (!result)
                        App.ShowToast(AppResources.security_wrong_code);
                    RefreshListView();
                }
            }
            else
            {
                var result = await App.ApiService.SetBlind(oDevice.idx, action);
                RefreshListView();
            }
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
            App.ShowToast(AppResources.blind_stop + ": " + oDevice.Name);

            if (oDevice.Protected)
            {
                var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password, inputType: InputType.Password);
                await Task.Delay(500);
                if (r.Ok)
                {
                    var result = await App.ApiService.SetBlind(oDevice.idx, ConstantValues.Device.Blind.Action.STOP, r.Text);
                    if (!result)
                        App.ShowToast(AppResources.security_wrong_code);
                    RefreshListView();
                }
            }
            else
            {
                var result = await App.ApiService.SetBlind(oDevice.idx, ConstantValues.Device.Blind.Action.STOP);
                RefreshListView();
            }
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
            RefreshListView();
        }

        #endregion Dimmer

    }
}
