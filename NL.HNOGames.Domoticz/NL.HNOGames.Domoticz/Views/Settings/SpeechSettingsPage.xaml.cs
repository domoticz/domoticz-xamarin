using NL.HNOGames.Domoticz.Controls;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using NL.HNOGames.Domoticz.Views.Dialog;
using Plugin.SpeechRecognition;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    /// <summary>
    /// Defines the <see cref="SpeechSettingsPage" />
    /// </summary>
    public partial class SpeechSettingsPage
    {
        #region Variables

        /// <summary>
        /// Defines the _oListSource
        /// </summary>
        private readonly List<SpeechModel> _oListSource;

        /// <summary>
        /// Defines the _oSelectedSpeechCommand
        /// </summary>
        private SpeechModel _oSelectedSpeechCommand;

        /// <summary>
        /// Defines the speech
        /// </summary>
        readonly ISpeechRecognizer speech = CrossSpeechRecognition.Current;

        /// <summary>
        /// Defines the listener
        /// </summary>
        public static IDisposable listener = null;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeechSettingsPage"/> class.
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
                    if (!await ValidateSpeechRecognition())
                        swEnableSpeech.IsToggled = false;
                }
            };

            _oListSource = App.AppSettings.SpeechCommands;
            if (_oListSource != null)
                listView.ItemsSource = _oListSource;
        }

        #endregion

        #region Public

        /// <summary>
        /// Connect device to Speech Command
        /// </summary>
        /// <param name="device">The device<see cref="Models.Device"/></param>
        /// <param name="password">The password<see cref="String"/></param>
        /// <param name="value">The value<see cref="String"/></param>
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

        #endregion

        #region Private

        /// <summary>
        /// Check if this feature is supported for your device
        /// </summary>
        /// <returns></returns>
        private async Task<bool> ValidateSpeechRecognition()
        {
            if (!this.speech.IsSupported)
            {
                App.ShowToast("Speech recognition is not supported for this device at this moment..");
                return false;
            }
            else
            {
                var granted = await CrossSpeechRecognition.Current.RequestPermission();
                if (granted != SpeechRecognizerStatus.Available)
                {
                    App.AddLog("Permission denied for speech recognition");
                    App.ShowToast("Don't have the permission for the mic");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Add new Speech Command to system
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void ToolbarItem_Activated(object sender, EventArgs e)
        {
            if (!await ValidateSpeechRecognition())
            {
                swEnableSpeech.IsToggled = false;
                return;
            }

            try
            {
                App.ShowLoading(AppResources.Speech_register);
                listener = CrossSpeechRecognition
                    .Current
                    .ListenUntilPause()
                    .Subscribe(phrase =>
                    {
                        App.HideLoading();
                        App.ShowToast(phrase);
                        if (SpeechSettingsPage.listener != null)
                            SpeechSettingsPage.listener.Dispose();

                        try
                        {
                            var textId = phrase.GetHashCode() + "";
                            if (_oListSource.Any(o => string.Compare(o.Id, textId, StringComparison.OrdinalIgnoreCase) == 0))
                                App.ShowToast(AppResources.Speech_exists);
                            else
                                AddNewRecord(textId, phrase);
                        }
                        catch (Exception ex)
                        {
                            App.AddLog(ex.Message);
                        }
                    });
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
                App.ShowToast(ex.Message);
                if (listener != null)
                    listener.Dispose();
            }
        }

        /// <summary>
        /// Create new Speech object
        /// </summary>
        /// <param name="speechID">The speechID<see cref="string"/></param>
        /// <param name="speechText">The speechText<see cref="string"/></param>
        private void AddNewRecord(string speechID, string speechText)
        {
            App.ShowToast(AppResources.Speech_saved + " " + speechText);
            var speechObject = new SpeechModel()
            {
                Id = speechID,
                Name = speechText,
                Enabled = true,
            };
            _oListSource.Add(speechObject);
            SaveAndRefresh();
            App.ShowToast(AppResources.noSwitchSelected_explanation_Speech);
        }

        /// <summary>
        /// Delete a Speech Command from the list
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnDeleteButton_Clicked(object sender, EventArgs e)
        {
            var oSpeechCommand = (SpeechModel)((TintedCachedImage)sender).BindingContext;
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
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void btnConnect_Clicked(object sender, EventArgs e)
        {
            _oSelectedSpeechCommand = (SpeechModel)((TintedCachedImage)sender).BindingContext;
            var oSwitchPopup = new SwitchPopup();
            oSwitchPopup.DeviceSelectedMethod += DelegateMethod;
            await PopupNavigation.Instance.PushAsync(oSwitchPopup);
        }

        #endregion
    }
}
