using System;

using NL.HNOGames.Domoticz.ViewModels;
using Xamarin.Forms;
using Rg.Plugins.Popup.Pages;
using System.Threading.Tasks;
using System.Collections.Generic;
using NL.HNOGames.Domoticz.Models;
using Acr.UserDialogs;
using NL.HNOGames.Domoticz.Resources;
using Rg.Plugins.Popup.Services;
using System.Linq;

namespace NL.HNOGames.Domoticz.Views.Dialog
{
    public partial class SwitchPopup : PopupPage
    {
        public delegate void DeviceSelected(Models.Device device, String pasword, String value);
        public DeviceSelected DeviceSelectedMethod { get; set; }

        public SwitchPopup()
        {
            InitializeComponent();
        }

        async Task OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Models.Device;
            if (item == null)
                return;
            listView.SelectedItem = null;

            String password = null;
            String value = null;
            if (item.Protected)
            {
                var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password, inputType: InputType.Password);
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
                    value = await DisplayActionSheet(AppResources.selector_value, AppResources.cancel, null, item.LevelNamesArray);
                    if (String.IsNullOrEmpty(value))
                        return;
                }
            }

            if (DeviceSelectedMethod != null)
                DeviceSelectedMethod(item, password, value);
            await PopupNavigation.PopAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            new Command(async () => await ExecuteGetSwitchesCommand()).Execute(null);
        }

        async Task ExecuteGetSwitchesCommand()
        {
            listView.IsRefreshing = true;
            var result = await App.ApiService.GetDevices(0, "all");
            if (result == null)
                return;

            var switchList = result.result.Where(o => Data.ConstantValues.CanHandleAutomatedToggle(o));
            listView.ItemsSource = switchList;
            listView.IsRefreshing = false;
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.PopAsync();
        }
    }
}