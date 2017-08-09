using System;

using NL.HNOGames.Domoticz.ViewModels;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Plugin.Share;
using NL.HNOGames.Domoticz.Resources;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;
using System.Linq;
using NL.HNOGames.Domoticz.Models;
using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using NL.HNOGames.Domoticz.Views.Dialog;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    public partial class SpeechSettingsPage : ContentPage
    {
        private List<SpeechModel> oListSource = new List<SpeechModel>();
        private SpeechModel oSelectedSpeechCommand = null;

        /// <summary>
        /// Constructor of QRCode page
        /// </summary>
        public SpeechSettingsPage()
        {
            InitializeComponent();

            App.ShowToast(AppResources.Speech_register);
            swEnableSpeech.IsToggled = App.AppSettings.SpeechEnabled;
            swEnableSpeech.Toggled += (sender, args) =>
            {
                App.AppSettings.SpeechEnabled = swEnableSpeech.IsToggled;
            };

            oListSource = App.AppSettings.SpeechCommands;
            if (oListSource != null)
                listView.ItemsSource = oListSource;
        }


        /// <summary>
        /// Deselect item
        /// </summary>
        async Task OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            listView.SelectedItem = null;
        }

        /// <summary>
        /// Add new Speech Command to system
        /// </summary>
        private async Task ToolbarItem_Activated(object sender, EventArgs e)
        {
            if (!App.AppSettings.SpeechEnabled)
                return;

            //todo record speech commad
        }

        /// <summary>
        /// Delete a Speech Command from the list
        /// </summary>
        private void btnDeleteButton_Clicked(object sender, EventArgs e)
        {
            SpeechModel oSpeechCommand = (SpeechModel)((Button)sender).BindingContext;
            App.ShowToast(AppResources.something_deleted.Replace("%1$s", oSpeechCommand.Name));
            oListSource.Remove(oSpeechCommand);
            SaveAndRefresh();
        }

        /// <summary>
        /// Save and refresh the list of Speech Commands
        /// </summary>
        private void SaveAndRefresh()
        {
            App.AppSettings.SpeechCommands = oListSource;
            listView.ItemsSource = null;
            listView.ItemsSource = oListSource;
        }

        /// <summary>
        /// Connect device to Speech Command
        /// </summary>
        private async Task btnConnect_Clicked(object sender, EventArgs e)
        {
            oSelectedSpeechCommand = (SpeechModel)((Button)sender).BindingContext;
            SwitchPopup oSwitchPopup = new SwitchPopup();
            oSwitchPopup.DeviceSelectedMethod += DelegateMethod;
            await PopupNavigation.PushAsync(oSwitchPopup);
        }

        /// <summary>
        /// Connect device to Speech Command
        /// </summary>
        public void DelegateMethod(Models.Device device, String password, String value)
        {
            App.ShowToast("Connecting " + oSelectedSpeechCommand.Name + " with switch " + device.Name);
            oSelectedSpeechCommand.SwitchIDX = device.idx;
            oSelectedSpeechCommand.SwitchName = device.Name;
            oSelectedSpeechCommand.Value = value;
            oSelectedSpeechCommand.SwitchPassword = password;
            oSelectedSpeechCommand.IsScene = device.IsScene;
            oSelectedSpeechCommand.IsScene = device.IsScene;
            SaveAndRefresh();
        }
    }
}