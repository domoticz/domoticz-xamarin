using System;
using System.Threading.Tasks;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.ViewModels;
using Xamarin.Forms;
using Acr.UserDialogs;
using NL.HNOGames.Domoticz.Resources;
using System.Linq;
using System.Collections.Generic;
using NL.HNOGames.Domoticz.Data;
using Rg.Plugins.Popup.Services;
using NL.HNOGames.Domoticz.Views.Dialog;
using System.Globalization;

namespace NL.HNOGames.Domoticz.Views
{
    public partial class DashboardPage
    {
        private readonly DashboardViewModel _viewModel;
        private object ScrollItem { get; set; }

        public DashboardPage(DashboardViewModel.ScreenTypeEnum screentype, Plan plan = null)
        {
            InitializeComponent();
            BindingContext = _viewModel = new DashboardViewModel(screentype, plan);
            _viewModel.SetListViewVisibilityMethod += DelegateListViewMethod;
            App.AddLog("Loading screen: " + screentype);

            adView.IsVisible = !App.AppSettings.PremiumBought;
        }

        public DashboardPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new DashboardViewModel(DashboardViewModel.ScreenTypeEnum.Dashboard, null);
            _viewModel.SetListViewVisibilityMethod += DelegateListViewMethod;
            App.AddLog("Loading screen: Dashboard");

            adView.IsVisible = !App.AppSettings.PremiumBought;
        }

        /// <summary>
        /// Set listview visibility (no items found)
        /// </summary>
        /// <param name="isvisible"></param>
        private void DelegateListViewMethod(bool isvisible)
        {
            listView.IsVisible = isvisible;
        }

        /// <summary>
        /// Show a actionsheet on item selected
        /// </summary>
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Models.Device;
            ScrollItem = args.SelectedItem;

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
            var actions = AddActionMenuItems(item);
            var result = await DisplayActionSheet(item.Name, AppResources.cancel, null, actions.ToArray());

            if (result == AppResources.favorite)
                await SetFavorite(item);
            else if (result == AppResources.button_status_notifications)
                await PopupNavigation.PushAsync(new NotificationsPopup(item));
            else if (result == AppResources.button_status_timer)
                await PopupNavigation.PushAsync(new TimersPopup(item));
            else if (result == AppResources.button_status_log)
                await PopupNavigation.PushAsync(new LogsPopup(item));
            else if (result == AppResources.wizard_graph)
            {
                switch (_viewModel.ScreenType)
                {
                    case DashboardViewModel.ScreenTypeEnum.Temperature:
                        await Navigation.PushAsync(new GraphTabbedPage(item));
                        break;
                    case DashboardViewModel.ScreenTypeEnum.Weather:
                    {
                        var graphType = item.TypeImg
                            .ToLower()
                            .Replace("temperature", "temp")
                            .Replace("visibility", "counter");
                        await Navigation.PushAsync(new GraphTabbedPage(item, graphType));
                    }
                        break;
                    case DashboardViewModel.ScreenTypeEnum.Utilities:
                    {
                        var graphType = item.SubType
                            .Replace("Electric", "counter")
                            .Replace("kWh", "counter")
                            .Replace("Gas", "counter")
                            .Replace("Energy", "counter")
                            .Replace("Voltcraft", "counter")
                            .Replace("Voltage", "counter")
                            .Replace("Lux", "counter")
                            .Replace("SetPoint", "temp")
                            .Replace("YouLess counter", "counter");
                        if (graphType.Contains("counter"))
                            graphType = "counter";
                        await Navigation.PushAsync(new GraphTabbedPage(item, graphType));
                    }
                        break;
                    case DashboardViewModel.ScreenTypeEnum.Dashboard:
                        break;
                    case DashboardViewModel.ScreenTypeEnum.Switches:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (result == AppResources.security_disarm || result == AppResources.security_arm_home ||
                     result == AppResources.security_arm_away)
            {
                var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password,
                    inputType: InputType.NumericPassword);
                await Task.Delay(500);
                if (r.Ok && !string.IsNullOrEmpty(r.Text))
                {
                    var status = ConstantValues.Security.Status.DISARM;
                    if (result == AppResources.security_arm_home)
                        status = ConstantValues.Security.Status.ARMHOME;
                    else if (result == AppResources.security_arm_away)
                        status = ConstantValues.Security.Status.ARMAWAY;
                    var md5Pass = Helpers.UsefulBits.GetMD5String(r.Text);
                    if (!await App.ApiService.SetSecurityPanel(status, md5Pass))
                        App.ShowToast(AppResources.security_generic_error);
                    else
                        RefreshListView(true);
                }
            }
        }

        /// <summary>
        /// Init list of menu items per device
        /// </summary>
        private List<string> AddActionMenuItems(Models.Device item)
        {
            var actions = new List<string>();
            if (string.Compare(item.SubType, ConstantValues.Device.SubType.Name.SECURITYPANEL,
                    StringComparison.OrdinalIgnoreCase) == 0)
            {
                actions.Add(AppResources.security_arm_away);
                actions.Add(AppResources.security_arm_home);
                actions.Add(AppResources.security_disarm);
            }

            actions.Add(AppResources.favorite);
            if (!string.IsNullOrEmpty(item.Notifications) &&
                string.Compare(item.Notifications, "true", StringComparison.OrdinalIgnoreCase) == 0)
                actions.Add(AppResources.button_status_notifications);

            if (!string.IsNullOrEmpty(item.Timers) &&
                string.Compare(item.Timers, "true", StringComparison.OrdinalIgnoreCase) == 0)
                actions.Add(AppResources.button_status_timer);

            if (_viewModel.ScreenType == DashboardViewModel.ScreenTypeEnum.Switches)
                actions.Add(AppResources.button_status_log);

            if (_viewModel.ScreenType == DashboardViewModel.ScreenTypeEnum.Temperature ||
                _viewModel.ScreenType == DashboardViewModel.ScreenTypeEnum.Weather ||
                _viewModel.ScreenType == DashboardViewModel.ScreenTypeEnum.Utilities)
                actions.Add(AppResources.wizard_graph);

            return actions;
        }

        /// <summary>
        /// On Appearing of the screen
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel.Devices == null || _viewModel.OldData)
                _viewModel.RefreshFavoriteCommand.Execute(null);

            if (_viewModel.ScreenType != DashboardViewModel.ScreenTypeEnum.Dashboard || App.AppSettings.ShowExtraData)
                listView.RowHeight = 130;
            else
                listView.RowHeight = 80;
        }


        /// <summary>
        /// Set Favorite
        /// </summary>
        public async Task SetFavorite(Models.Device pair)
        {
            var newValue = !pair.FavoriteBoolean;
            if (newValue)
                App.ShowToast(pair.Name + " " + AppResources.favorite_added);
            else
                App.ShowToast(pair.Name + " " + AppResources.favorite_removed);

            var result = await App.ApiService.SetFavorite(pair.idx, pair.IsScene, newValue);
            if (!result)
                App.ShowToast(pair.Name + " " + AppResources.error_favorite);
            RefreshListView(false);
        }

        /// <summary>
        /// Refresh ListView
        /// </summary>
        private void RefreshListView(bool afterAction)
        {
            if (!afterAction)
                _viewModel.RefreshFavoriteCommand.Execute(null);
            else
                _viewModel.RefreshActionCommand.Execute(null);

            sbSearch.Text = string.Empty;
            if (ScrollItem == null)
                return;
            listView.ScrollTo(ScrollItem, ScrollToPosition.Center, true);
            ScrollItem = null;
            
        }

        /// <summary>
        /// Filter changed
        /// </summary>
        private void sbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == string.Empty)
            {
                App.AddLog("Cancel Pressed");
                listView.ItemsSource = _viewModel.Devices;
                sbSearch.Unfocus();
            }
            else
            {
                try
                {
                    var filterText = e.NewTextValue.ToLower().Trim();
                    listView.ItemsSource = filterText == string.Empty
                        ? _viewModel.Devices
                        : _viewModel.Devices.Where(i => i.Name.ToLower().Trim().Contains(filterText));
                }
                catch (Exception)
                {
                    listView.ItemsSource = _viewModel.Devices;
                }
            }
        }

        #region Selector

        /// <summary>
        /// Set Selector value to Domoticz
        /// </summary>
        private async void pSelector_Unfocused(object sender, FocusEventArgs e)
        {
            if (_viewModel.OldData)
                return;
            var oPicker = (Picker) sender;
            await NewSelectorValueAsync((Models.Device) oPicker.BindingContext, oPicker);
        }

        /// <summary>
        /// Set Selector value to Domoticz
        /// </summary>
        public async Task NewSelectorValueAsync(Models.Device pair, Picker oPicker)
        {
            var newValue = 0;
            if (oPicker.SelectedIndex > 0)
                newValue = oPicker.SelectedIndex * 10;

            if (pair.LevelInt != newValue)
            {
                if (pair.Protected)
                {
                    var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password,
                        inputType: InputType.Password);
                    await Task.Delay(500);
                    if (r.Ok)
                    {
                        var result = await App.ApiService.SetDimmer(pair.idx, newValue, r.Text);
                        if (!result)
                            App.ShowToast(AppResources.security_wrong_code);
                        RefreshListView(true);
                    }
                }
                else
                {
                    await App.ApiService.SetDimmer(pair.idx, newValue);
                    RefreshListView(true);
                }
            }
        }

        #endregion Selector


        #region On Off Switches

        /// <summary>
        /// Turn switch/device ON
        /// </summary>
        private async void btnOnButton_Clicked(object sender, EventArgs e)
        {
            if (_viewModel.OldData)
                return;
            var oButton = (Button) sender;
            var oDevice = (Models.Device) oButton.BindingContext;
            if (oDevice.Protected)
            {
                var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password,
                    inputType: InputType.Password);
                await Task.Delay(500);
                if (r.Ok)
                {
                    App.ShowToast(AppResources.switch_on + ": " + oDevice.Name);
                    var result = await App.ApiService.SetSwitch(oDevice.idx, true,
                        oDevice.Type == ConstantValues.Device.Scene.Type.GROUP ||
                        oDevice.Type == ConstantValues.Device.Scene.Type.SCENE, r.Text);
                    if (!result)
                        App.ShowToast(AppResources.security_wrong_code);

                    RefreshListView(true);
                }
            }
            else
            {
                App.ShowToast(AppResources.switch_on + ": " + oDevice.Name);
                await App.ApiService.SetSwitch(oDevice.idx, true,
                    oDevice.Type == ConstantValues.Device.Scene.Type.GROUP ||
                    oDevice.Type == ConstantValues.Device.Scene.Type.SCENE);
                RefreshListView(true);
            }
        }

        /// <summary>
        /// Turn switch/device OFF
        /// </summary>
        private async void btnOffButton_Clicked(object sender, EventArgs e)
        {
            var oButton = (Button) sender;
            var oDevice = (Models.Device) oButton.BindingContext;
            if (oDevice.Protected)
            {
                var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password,
                    inputType: InputType.Password);
                await Task.Delay(500);
                if (r.Ok)
                {
                    App.ShowToast(AppResources.switch_off + ": " + oDevice.Name);
                    var result = await App.ApiService.SetSwitch(oDevice.idx, false,
                        oDevice.Type == ConstantValues.Device.Scene.Type.GROUP ||
                        oDevice.Type == ConstantValues.Device.Scene.Type.SCENE, r.Text);
                    if (!result)
                        App.ShowToast(AppResources.security_wrong_code);
                    RefreshListView(true);
                }
            }
            else
            {
                App.ShowToast(AppResources.switch_off + ": " + oDevice.Name);
                await App.ApiService.SetSwitch(oDevice.idx, false,
                    oDevice.Type == ConstantValues.Device.Scene.Type.GROUP ||
                    oDevice.Type == ConstantValues.Device.Scene.Type.SCENE);
                RefreshListView(true);
            }
        }

        /// <summary>
        /// Toggle switch
        /// </summary>
        private async void btnSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            var oSwitch = (Switch) sender;
            var oDevice = (Models.Device) oSwitch.BindingContext;
            if (oSwitch.IsToggled != oDevice.StatusBoolean)
            {
                if (oDevice.Protected)
                {
                    var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password,
                        inputType: InputType.Password);
                    await Task.Delay(500);
                    if (r.Ok)
                    {
                        if (oDevice.StatusBoolean)
                            App.ShowToast(AppResources.switch_off + ": " + oDevice.Name);
                        else
                            App.ShowToast(AppResources.switch_on + ": " + oDevice.Name);

                        var result = await App.ApiService.SetSwitch(oDevice.idx, oSwitch.IsToggled,
                            oDevice.Type == ConstantValues.Device.Scene.Type.GROUP ||
                            oDevice.Type == ConstantValues.Device.Scene.Type.SCENE, r.Text);
                        if (!result)
                            App.ShowToast(AppResources.security_wrong_code);

                        RefreshListView(true);
                    }
                }
                else
                {
                    if (oDevice.StatusBoolean)
                        App.ShowToast(AppResources.switch_off + ": " + oDevice.Name);
                    else
                        App.ShowToast(AppResources.switch_on + ": " + oDevice.Name);
                    await App.ApiService.SetSwitch(oDevice.idx, oSwitch.IsToggled,
                        oDevice.Type == ConstantValues.Device.Scene.Type.GROUP ||
                        oDevice.Type == ConstantValues.Device.Scene.Type.SCENE);
                    RefreshListView(true);
                }
            }
        }

        #endregion On Off Switches


        #region Set (Temperature)

        /// <summary>
        /// set a specific value in a switch (temperature for example)
        /// </summary>
        private async void txtSetValue_Completed(object sender, EventArgs e)
        {
            if (_viewModel.OldData)
                return;
            var oEntry = (Entry) sender;
            var oDevice = (Models.Device) oEntry.BindingContext;
            if (Helpers.UsefulBits.IsNumeric(oEntry.Text))
                await SetPointValue(oEntry, oDevice);
        }

        /// <summary>
        /// Setpoint action 
        /// </summary>
        private async Task SetPointValue(Entry oEntry, Models.Device oDevice, Boolean refresh = true)
        {
            var newValue = double.Parse(oEntry.Text, CultureInfo.InvariantCulture);
            App.AddLog("Set idx " + oDevice.idx + " to " + newValue);
            if (oDevice.Protected)
            {
                var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password,
                    inputType: InputType.Password);
                await Task.Delay(500);
                if (r.Ok)
                {
                    var result = await App.ApiService.SetPoint(oDevice.idx, newValue,
                        double.Parse(oDevice.SetPoint, CultureInfo.InvariantCulture), r.Text);
                    if (!result)
                        App.ShowToast(AppResources.security_wrong_code);
                    if (refresh) RefreshListView(true);
                }
            }
            else
            {
                await App.ApiService.SetPoint(oDevice.idx, newValue,
                    double.Parse(oDevice.SetPoint, CultureInfo.InvariantCulture));
                if (refresh) RefreshListView(true);
            }
        }

        /// <summary>
        /// min 0.5 from temp
        /// </summary>
        private async void btnMinTemp_Clicked(object sender, EventArgs e)
        {
            if (_viewModel.OldData)
                return;
            var btnMinTemp = (Button) sender;
            var entry =
                ((Entry) ((StackLayout) btnMinTemp.Parent).Children.FirstOrDefault(o => o.GetType() == typeof(Entry)));
            var currentValue = double.Parse(entry.Text, CultureInfo.InvariantCulture);
            var changedValue = currentValue - 0.5;
            entry.Text = changedValue.ToString(CultureInfo.InvariantCulture);
            await SetPointValue(entry, (Models.Device) btnMinTemp.BindingContext, false);
        }

        /// <summary>
        /// Add 0.5 to temp
        /// </summary>
        private async void btnPlusTemp_Clicked(object sender, EventArgs e)
        {
            if (_viewModel.OldData)
                return;
            var btnPlusTemp = (Button) sender;
            var entry =
                ((Entry) ((StackLayout) btnPlusTemp.Parent).Children.FirstOrDefault(o => o.GetType() == typeof(Entry)));
            var currentValue = double.Parse(entry.Text, CultureInfo.InvariantCulture);
            var changedValue = currentValue + 0.5;
            entry.Text = changedValue.ToString(CultureInfo.InvariantCulture);
            await SetPointValue(entry, (Models.Device) btnPlusTemp.BindingContext, false);
        }

        #endregion Set (Temperature)


        #region Blinds

        /// <summary>
        /// Turn blinds ON/DOWN
        /// </summary>
        private async void btnBlindOnButton_Clicked(object sender, EventArgs e)
        {
            if (_viewModel.OldData)
                return;
            var oButton = (Button) sender;
            var oDevice = (Models.Device) oButton.BindingContext;
            var action = ConstantValues.Device.Switch.Action.ON;
            if (oDevice.SwitchTypeVal == ConstantValues.Device.Type.Value.BLINDINVERTED)
            {
                action = ConstantValues.Device.Switch.Action.OFF;
                App.ShowToast(AppResources.blind_up + ": " + oDevice.Name);
            }
            else
                App.ShowToast(AppResources.blind_down + ": " + oDevice.Name);

            if (oDevice.Protected)
            {
                var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password,
                    inputType: InputType.Password);
                await Task.Delay(500);
                if (r.Ok)
                {
                    var result = await App.ApiService.SetBlind(oDevice.idx, action, r.Text);
                    if (!result)
                        App.ShowToast(AppResources.security_wrong_code);
                    RefreshListView(true);
                }
            }
            else
            {
                await App.ApiService.SetBlind(oDevice.idx, action);
                RefreshListView(true);
            }
        }

        /// <summary>
        /// Turn blinds OFF/UP
        /// </summary>
        private async void btnBlindOffButton_Clicked(object sender, EventArgs e)
        {
            if (_viewModel.OldData)
                return;
            var oButton = (Button) sender;
            var oDevice = (Models.Device) oButton.BindingContext;
            var action = ConstantValues.Device.Switch.Action.OFF;
            if (oDevice.SwitchTypeVal == ConstantValues.Device.Type.Value.BLINDINVERTED)
            {
                action = ConstantValues.Device.Switch.Action.ON;
                App.ShowToast(AppResources.blind_down + ": " + oDevice.Name);
            }
            else
                App.ShowToast(AppResources.blind_up + ": " + oDevice.Name);

            if (oDevice.Protected)
            {
                var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password,
                    inputType: InputType.Password);
                await Task.Delay(500);
                if (r.Ok)
                {
                    var result = await App.ApiService.SetBlind(oDevice.idx, action, r.Text);
                    if (!result)
                        App.ShowToast(AppResources.security_wrong_code);
                    RefreshListView(true);
                }
            }
            else
            {
                await App.ApiService.SetBlind(oDevice.idx, action);
                RefreshListView(true);
            }
        }

        /// <summary>
        /// Turn blinds Stop
        /// </summary>
        private async void btnBlindStopButton_Clicked(object sender, EventArgs e)
        {
            if (_viewModel.OldData)
                return;

            var oButton = (Button) sender;
            var oDevice = (Models.Device) oButton.BindingContext;
            App.ShowToast(AppResources.blind_stop + ": " + oDevice.Name);
            if (oDevice.Protected)
            {
                var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password,
                    inputType: InputType.Password);
                await Task.Delay(500);
                if (r.Ok)
                {
                    var result =
                        await App.ApiService.SetBlind(oDevice.idx, ConstantValues.Device.Blind.Action.STOP, r.Text);
                    if (!result)
                        App.ShowToast(AppResources.security_wrong_code);
                    RefreshListView(true);
                }
            }
            else
            {
                await App.ApiService.SetBlind(oDevice.idx, ConstantValues.Device.Blind.Action.STOP);
                RefreshListView(true);
            }
        }

        #endregion Blinds


        #region Dimmer

        /// <summary>
        /// Slider value of the dimmer
        /// </summary>
        private async void btnLevelButton_Clicked(object sender, EventArgs e)
        {
            var oButton = (Button) sender;
            var oDevice = (Models.Device) oButton.BindingContext;
            var oSlider = new SliderPopup(oDevice, _viewModel.RefreshFavoriteCommand);
            await PopupNavigation.PushAsync(oSlider);
            RefreshListView(true);
        }

        #endregion Dimmer
    }
}