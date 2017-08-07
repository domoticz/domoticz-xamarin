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
        public SwitchPopup()
        {
            InitializeComponent();
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            listView.SelectedItem = null;
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