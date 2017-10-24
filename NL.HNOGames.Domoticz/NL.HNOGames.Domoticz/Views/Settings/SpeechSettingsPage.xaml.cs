using System;
using Xamarin.Forms;
using System.Linq;
using System.Reactive.Linq;
using System.Collections.Generic;
using NL.HNOGames.Domoticz.Resources;
using NL.HNOGames.Domoticz.Models;
using Rg.Plugins.Popup.Services;
using NL.HNOGames.Domoticz.Views.Dialog;
using Plugin.SpeechRecognition;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    public partial class SpeechSettingsPage
    {
        private readonly List<SpeechModel> _oListSource;
        private SpeechModel _oSelectedSpeechCommand;
        readonly ISpeechRecognizer speech = CrossSpeechRecognition.Current;

        /// <summary>
        /// Constructor of QRCode page
        /// </summary>
        public SpeechSettingsPage()
        {

            _oSelectedSpeechCommand = null;
            InitializeComponent();

            App.ShowToast(AppResources.Speech_register);
            swEnableSpeech.IsToggled = App.AppSettings.SpeechEnabled;
            swEnableSpeech.Toggled += async (sender, args) =>
            {

                App.AppSettings.SpeechEnabled = swEnableSpeech.IsToggled;

                if (swEnableSpeech.IsToggled)
                {
                    if (!this.speech.IsSupported)
                    {
                        App.ShowToast("SPEECH RECOGNITION NOT SUPPORTED FOR THIS DEVICE");
                        swEnableSpeech.IsToggled = false;
                    }
                    else
                    {
                        var status = await speech.RequestPermission();
                        if (status != SpeechRecognizerStatus.Available)
                        {
                            App.AddLog("Permission denied for speech recognition");
                            App.ShowToast("Don't have the permission for the mic");
                            swEnableSpeech.IsToggled = false;
                        }
                    }
                }
            };
            _oListSource = App.AppSettings.SpeechCommands;
            if (_oListSource != null)
                listView.ItemsSource = _oListSource;
        }

        /// <summary>
        /// Add new Speech Command to system
        /// </summary>
        private async void ToolbarItem_Activated(object sender, EventArgs e)
        {
            //var granted = await speech.RequestPermission();
            //if (granted == SpeechRecognizerStatus.Available)
            //{
            App.ShowToast("Waiting for input");
            CrossSpeechRecognition
                .Current
                .ListenUntilPause()
                .Subscribe(phrase => { App.ShowToast(phrase); });
            //};
        }

        /// <summary>
        /// Delete a Speech Command from the list
        /// </summary>
        private void btnDeleteButton_Clicked(object sender, EventArgs e)
        {
            var oSpeechCommand = (SpeechModel)((Button)sender).BindingContext;
            App.ShowToast(AppResources.something_deleted.Replace("%1$s", oSpeechCommand.Name));
            _oListSource.Remove(oSpeechCommand);
            SaveAndRefresh();
        }

        /// <summary>
        /// Save and refresh the list of Speech Commands
        /// </summary>
        private void SaveAndRefresh()
        {
            App.AppSettings.SpeechCommands = _oListSource;
            listView.ItemsSource = null;
            listView.ItemsSource = _oListSource;
        }

        /// <summary>
        /// Connect device to Speech Command
        /// </summary>
        private async void btnConnect_Clicked(object sender, EventArgs e)
        {
            _oSelectedSpeechCommand = (SpeechModel)((Button)sender).BindingContext;
            var oSwitchPopup = new SwitchPopup();
            oSwitchPopup.DeviceSelectedMethod += DelegateMethod;
            await PopupNavigation.PushAsync(oSwitchPopup);
        }

        /// <summary>
        /// Connect device to Speech Command
        /// </summary>
        public void DelegateMethod(Models.Device device, String password, String value)
        {
            App.ShowToast("Connecting " + _oSelectedSpeechCommand.Name + " with switch " + device.Name);
            _oSelectedSpeechCommand.SwitchIDX = device.idx;
            _oSelectedSpeechCommand.SwitchName = device.Name;
            _oSelectedSpeechCommand.Value = value;
            _oSelectedSpeechCommand.SwitchPassword = password;
            _oSelectedSpeechCommand.IsScene = device.IsScene;
            _oSelectedSpeechCommand.IsScene = device.IsScene;
            SaveAndRefresh();
        }
    }
}