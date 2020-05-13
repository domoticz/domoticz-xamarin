﻿using Acr.UserDialogs;
using NL.HNOGames.Domoticz.Controls;
using NL.HNOGames.Domoticz.Data;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using NL.HNOGames.Domoticz.ViewModels;
using NL.HNOGames.Domoticz.Views.Dialog;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.Views
{
    /// <summary>
    /// Defines the <see cref="DashboardPage" />
    /// </summary>
    public partial class DashboardPage
    {
        #region Variables

        /// <summary>
        /// Defines the _viewModel
        /// </summary>
        private readonly DashboardViewModel _viewModel;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardPage"/> class.
        /// </summary>
        /// <param name="screentype">The screentype<see cref="DashboardViewModel.ScreenTypeEnum"/></param>
        /// <param name="plan">The plan<see cref="Plan"/></param>
        public DashboardPage(DashboardViewModel.ScreenTypeEnum screentype, Plan plan = null)
        {
            InitializeComponent();
            BindingContext = _viewModel = new DashboardViewModel(screentype, plan);
            _viewModel.SetListViewVisibilityMethod += DelegateListViewMethod;
            App.AddLog("Loading screen: " + screentype);

            searchIcon.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(OnSearchIconTapped) });
            searchBar.TextChanged += searchBar_TextChanged;
            searchBar.Cancelled += (s, e) => OnCancelled();

            adView.IsVisible = !App.AppSettings.PremiumBought;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardPage"/> class.
        /// </summary>
        public DashboardPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new DashboardViewModel(DashboardViewModel.ScreenTypeEnum.Dashboard, null);
            _viewModel.SetListViewVisibilityMethod += DelegateListViewMethod;
            App.AddLog("Loading screen: Dashboard");

            adView.IsVisible = !App.AppSettings.PremiumBought;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ScrollItem
        /// </summary>
        private object ScrollItem { get; set; }

        #endregion

        #region Public

        /// <summary>
        /// Set Favorite
        /// </summary>
        /// <param name="pair">The pair<see cref="Models.Device"/></param>
        /// <returns>The <see cref="Task"/></returns>
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
        /// Set Selector value to Domoticz
        /// </summary>
        /// <param name="pair">The pair<see cref="Models.Device"/></param>
        /// <param name="oPicker">The oPicker<see cref="Picker"/></param>
        /// <returns>The <see cref="Task"/></returns>
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
        /// Show a actionsheet on item selected
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="args">The args<see cref="SelectedItemChangedEventArgs"/></param>
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
        /// <param name="item">The item<see cref="Models.Device"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task ShowActionMenu(Models.Device item)
        {
            var actions = AddActionMenuItems(item);
            var result = await DisplayActionSheet(item.Name, AppResources.cancel, null, actions.ToArray());

            if (result == AppResources.favorite)
                await SetFavorite(item);
            else if (result == AppResources.button_status_notifications)
                await Navigation.PushAsync(new NotificationsPopup(item));
            else if (result == AppResources.button_status_timer)
                await Navigation.PushAsync(new TimersPopup(item));
            else if (result == AppResources.button_status_log)
                await Navigation.PushAsync(new LogsPopup(item));
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
                                .Replace("BWR102", "counter")
                                .Replace("Sound Level", "counter")
                                .Replace("Voltcraft", "counter")
                                .Replace("Voltage", "counter")
                                .Replace("Lux", "counter")
                                .Replace("SetPoint", "temp")
                                .Replace("YouLess counter", "counter")
                                .Replace("Pressure", "counter")
                                .Replace("Moisture", "counter")
                                .Replace("Managed Counter", "counter")
                                .Replace("Custom Sensor", "counter");
                            if (graphType.ToLower().Contains("counter"))
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
        /// <param name="item">The item<see cref="Models.Device"/></param>
        /// <returns>The <see cref="List{string}"/></returns>
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
            {
                if (_viewModel.ScreenType == DashboardViewModel.ScreenTypeEnum.Utilities &&
                    (string.Compare(item.SubType, ConstantValues.Device.Utility.SubType.TEXT, StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(item.SubType, ConstantValues.Device.Utility.SubType.ALERT, StringComparison.OrdinalIgnoreCase) == 0))
                    actions.Add(AppResources.button_status_log);
                else
                    actions.Add(AppResources.wizard_graph);
            }

            return actions;
        }

        /// <summary>
        /// Refresh ListView
        /// </summary>
        /// <param name="afterAction">The afterAction<see cref="bool"/></param>
        private void RefreshListView(bool afterAction)
        {
            if (!afterAction)
                _viewModel.RefreshFavoriteCommand.Execute(null);
            else
                _viewModel.RefreshActionCommand.Execute(null);

            searchBar.Text = string.Empty;
            if (ScrollItem == null)
                return;
            listView.ScrollTo(ScrollItem, ScrollToPosition.Center, true);
            ScrollItem = null;
        }

        /// <summary>
        /// Filter changed
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="TextChangedEventArgs"/></param>
        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == string.Empty)
            {
                App.AddLog("Cancel Pressed");
                listView.ItemsSource = _viewModel.Devices;
                searchBar.Unfocus();
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
        /// Set Selector value to Domoticz
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="FocusEventArgs"/></param>
        private async void pSelector_Unfocused(object sender, FocusEventArgs e)
        {
            if (_viewModel.OldData)
                return;
            var oPicker = (Picker)sender;
            await NewSelectorValueAsync((Models.Device)oPicker.BindingContext, oPicker);
        }

        /// <summary>
        /// Turn switch/device ON
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void btnOnButton_Clicked(object sender, EventArgs e)
        {
            var oButton = (Button)sender;
            var oDevice = (Models.Device)oButton.BindingContext;
            if (oDevice.Protected)
            {
                var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password,
                    inputType: InputType.Password);
                await Task.Delay(500);
                if (r.Ok)
                {
                    App.ShowToast(AppResources.switch_on + ": " + oDevice.Name);
                    var result = await App.ApiService.SetSwitch(oDevice.idx,
                       oDevice.SwitchTypeVal == ConstantValues.Device.Type.Value.BLINDINVERTED || oDevice.SwitchTypeVal == ConstantValues.Device.Type.Value.BLINDS || oDevice.SwitchTypeVal == ConstantValues.Device.Type.Value.DOORLOCKINVERTED ? false : true,
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
                await App.ApiService.SetSwitch(oDevice.idx,
                   oDevice.SwitchTypeVal == ConstantValues.Device.Type.Value.BLINDINVERTED || oDevice.SwitchTypeVal == ConstantValues.Device.Type.Value.BLINDS || oDevice.SwitchTypeVal == ConstantValues.Device.Type.Value.DOORLOCKINVERTED ? false : true,
                    oDevice.Type == ConstantValues.Device.Scene.Type.GROUP ||
                    oDevice.Type == ConstantValues.Device.Scene.Type.SCENE);
                RefreshListView(true);
            }
        }

        /// <summary>
        /// Turn switch/device OFF
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void btnOffButton_Clicked(object sender, EventArgs e)
        {
            var oButton = (Button)sender;
            var oDevice = (Models.Device)oButton.BindingContext;
            if (oDevice.Protected)
            {
                var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password,
                    inputType: InputType.Password);
                await Task.Delay(500);
                if (r.Ok)
                {
                    App.ShowToast(AppResources.switch_off + ": " + oDevice.Name);
                    var result = await App.ApiService.SetSwitch(oDevice.idx,
                       oDevice.SwitchTypeVal == ConstantValues.Device.Type.Value.BLINDINVERTED || oDevice.SwitchTypeVal == ConstantValues.Device.Type.Value.BLINDS || oDevice.SwitchTypeVal == ConstantValues.Device.Type.Value.DOORLOCKINVERTED ? true : false,
                        oDevice.Type == ConstantValues.Device.Scene.Type.GROUP || oDevice.Type == ConstantValues.Device.Scene.Type.SCENE,
                        r.Text);
                    if (!result)
                        App.ShowToast(AppResources.security_wrong_code);
                    RefreshListView(true);
                }
            }
            else
            {
                App.ShowToast(AppResources.switch_off + ": " + oDevice.Name);
                await App.ApiService.SetSwitch(oDevice.idx,
                      oDevice.SwitchTypeVal == ConstantValues.Device.Type.Value.BLINDINVERTED || oDevice.SwitchTypeVal == ConstantValues.Device.Type.Value.BLINDS || oDevice.SwitchTypeVal == ConstantValues.Device.Type.Value.DOORLOCKINVERTED ? true : false,
                    oDevice.Type == ConstantValues.Device.Scene.Type.GROUP || oDevice.Type == ConstantValues.Device.Scene.Type.SCENE);
                RefreshListView(true);
            }
        }

        /// <summary>
        /// Toggle switch
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="ToggledEventArgs"/></param>
        private async void btnSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            var oSwitch = (Switch)sender;
            var oDevice = (Models.Device)oSwitch.BindingContext;
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

                        var result = await App.ApiService.SetSwitch(oDevice.idx,
                          oDevice.SwitchTypeVal == ConstantValues.Device.Type.Value.BLINDS || oDevice.SwitchTypeVal == ConstantValues.Device.Type.Value.BLINDPERCENTAGE || oDevice.SwitchTypeVal == ConstantValues.Device.Type.Value.DOORLOCKINVERTED ? !oSwitch.IsToggled : oSwitch.IsToggled,
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
                    await App.ApiService.SetSwitch(oDevice.idx,
                       oDevice.SwitchTypeVal == ConstantValues.Device.Type.Value.BLINDS || oDevice.SwitchTypeVal == ConstantValues.Device.Type.Value.BLINDPERCENTAGE || oDevice.SwitchTypeVal == ConstantValues.Device.Type.Value.DOORLOCKINVERTED ? !oSwitch.IsToggled : oSwitch.IsToggled,
                        oDevice.Type == ConstantValues.Device.Scene.Type.GROUP ||
                        oDevice.Type == ConstantValues.Device.Scene.Type.SCENE);
                    RefreshListView(true);
                }
            }
        }

        /// <summary>
        /// set a specific value in a switch (temperature for example)
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void txtSetValue_Completed(object sender, EventArgs e)
        {
            if (_viewModel.OldData)
                return;
            var oEntry = (Entry)sender;
            var oDevice = (Models.Device)oEntry.BindingContext;
            if (Helpers.UsefulBits.IsNumeric(oEntry.Text))
                await SetPointValue(oEntry, oDevice);
        }

        /// <summary>
        /// Setpoint action
        /// </summary>
        /// <param name="oEntry">The oEntry<see cref="Entry"/></param>
        /// <param name="oDevice">The oDevice<see cref="Models.Device"/></param>
        /// <param name="refresh">The refresh<see cref="Boolean"/></param>
        /// <returns>The <see cref="Task"/></returns>
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
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void btnMinTemp_Clicked(object sender, EventArgs e)
        {
            if (_viewModel.OldData)
                return;
            var btnMinTemp = (Button)sender;
            var entry =
                ((Entry)((StackLayout)btnMinTemp.Parent).Children.FirstOrDefault(o => o.GetType() == typeof(Entry)));
            var currentValue = double.Parse(entry.Text, CultureInfo.InvariantCulture);
            var changedValue = currentValue - 0.5;
            entry.Text = changedValue.ToString(CultureInfo.InvariantCulture);
            await SetPointValue(entry, (Models.Device)btnMinTemp.BindingContext, false);
        }

        /// <summary>
        /// Add 0.5 to temp
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void btnPlusTemp_Clicked(object sender, EventArgs e)
        {
            if (_viewModel.OldData)
                return;
            var btnPlusTemp = (Button)sender;
            var entry =
                ((Entry)((StackLayout)btnPlusTemp.Parent).Children.FirstOrDefault(o => o.GetType() == typeof(Entry)));
            var currentValue = double.Parse(entry.Text, CultureInfo.InvariantCulture);
            var changedValue = currentValue + 0.5;
            entry.Text = changedValue.ToString(CultureInfo.InvariantCulture);
            await SetPointValue(entry, (Models.Device)btnPlusTemp.BindingContext, false);
        }

        /// <summary>
        /// Turn blinds ON/DOWN
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void btnBlindOnButton_Clicked(object sender, EventArgs e)
        {
            if (_viewModel.OldData)
                return;
            var oButton = (Button)sender;
            var oDevice = (Models.Device)oButton.BindingContext;
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
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void btnBlindOffButton_Clicked(object sender, EventArgs e)
        {
            if (_viewModel.OldData)
                return;
            var oButton = (Button)sender;
            var oDevice = (Models.Device)oButton.BindingContext;
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
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void btnBlindStopButton_Clicked(object sender, EventArgs e)
        {
            if (_viewModel.OldData)
                return;

            var oButton = (Button)sender;
            var oDevice = (Models.Device)oButton.BindingContext;
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

        /// <summary>
        /// Slider value of the dimmer
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void btnLevelButton_Clicked(object sender, EventArgs e)
        {
            var oButton = (Button)sender;
            var oDevice = (Models.Device)oButton.BindingContext;
            var oSlider = new SliderPopup(oDevice, _viewModel.RefreshFavoriteCommand);
            await PopupNavigation.Instance.PushAsync(oSlider);
            RefreshListView(true);
        }

        /// <summary>
        /// RGB button picker
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void btnRGBColorPicker_Tapped(object sender, EventArgs e)
        {
            var oButton = (TintedCachedImage)sender;
            var oDevice = (Models.Device)oButton.BindingContext;
            var oColorPicker = new ColorPopup(oDevice, _viewModel.RefreshFavoriteCommand);
            await PopupNavigation.Instance.PushAsync(oColorPicker);
            RefreshListView(true);
        }

        #endregion

        /// <summary>
        /// On Appearing of the screen
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel.Devices == null || _viewModel.OldData)
                _viewModel.RefreshFavoriteCommand.Execute(null);

            var info = await App.GetSunRiseInfoAsync();
            if (info != null)
                subtitle.Text = $"↑{info.Sunrise} ↓{info.Sunset}";
        }
    }
}
