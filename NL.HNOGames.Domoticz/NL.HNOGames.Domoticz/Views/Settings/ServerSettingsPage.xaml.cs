using Acr.UserDialogs;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServerSettingsPage
    {
        private ServerSettings _oServerSettings;

        public ServerSettings ServerSettings
        {
            //Property that will be used to get and set the item
            get => _oServerSettings;
            set
            {
                _oServerSettings = value;
                BindingContext = _oServerSettings;
            }
        }

        public ServerSettingsPage()
        {
            ServerSettings = App.AppSettings.ActiveServerSettings ?? new ServerSettings();

            InitializeComponent();
            BindingContext = ServerSettings;
            btnCheck.IsVisible = true;
            lblSSLWarning.IsVisible = string.Compare(txtRemoteProtocol.Items[txtRemoteProtocol.SelectedIndex], "https",
                                          StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>
        /// Enable local settings
        /// </summary>
        private void swEnableLocalSettings_Toggled(object sender, ToggledEventArgs e)
        {
            lyLocalSettings.IsVisible = swEnableLocalSettings.IsToggled;
        }

        /// <summary>
        /// Validate mandatory settings
        /// </summary>
        public bool ValidateServerSettings()
        {
            if (string.IsNullOrEmpty(ServerSettings.REMOTE_SERVER_URL))
                return false;
            //if (string.IsNullOrEmpty(ServerSettings.REMOTE_SERVER_PORT))
            //    return false;
            if (!ServerSettings.IS_LOCAL_SERVER_ADDRESS_DIFFERENT) return true;
            return !string.IsNullOrEmpty(ServerSettings.REMOTE_SERVER_URL);
            //if (string.IsNullOrEmpty(ServerSettings.REMOTE_SERVER_PORT))
            //    return false;
        }

        /// <summary>
        /// Validate mandatory settings
        /// </summary>
        public bool ValidateServerUrl()
        {
            return !ServerSettings.LOCAL_SERVER_URL.Contains("http") &&
                   !ServerSettings.REMOTE_SERVER_URL.Contains("http");
        }

        /// <summary>
        /// Connection Finished
        /// </summary>
        private void btnFinish_Clicked(object sender, EventArgs e)
        {
            App.SetMainPage();
        }

        /// <summary>
        /// Process Server Settings
        /// </summary>
        private async void ProcessServerSettings()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            //save latest version
            App.AppSettings.ActiveServerSettings = ServerSettings;
            lblResult.Text = "";
            btnFinish.IsVisible = false;
            btnCheck.IsVisible = true;
            imFinish.IsVisible = false;

            if (!ValidateServerSettings())
                UserDialogs.Instance.Alert(AppResources.welcome_msg_connectionDataIncomplete + "\n\n" +
                                           AppResources.welcome_msg_correctOnPreviousPage);
            else if (!ValidateServerUrl())
                UserDialogs.Instance.Alert(AppResources.welcome_msg_connectionDataInvalid + "\n\n" +
                                           AppResources.welcome_msg_correctOnPreviousPage);
            else
            {
                lblResult.Text = AppResources.welcome_info_checkingConnection + Environment.NewLine;

                App.ShowLoading();
                //get Domoticz version to check settings
                App.ApiService.Server = App.AppSettings.ActiveServerSettings;
                var result = await App.ApiService.GetVersion();

                if (result != null)
                {
                    lblResult.Text = AppResources.welcome_msg_serverVersion + ": " + result.version;
                    var devices = await App.ApiService.GetDevices();
                    if (devices?.result != null)
                    {
                        lblResult.Text += Environment.NewLine + devices.result.Length +
                                          AppResources.welcome_msg_numberOfDevices.Replace("%1$d", "");
                        btnFinish.IsVisible = true;
                        btnCheck.IsVisible = false;
                        imFinish.IsVisible = true;
                    }
                    else
                    {
                        lblResult.Text = App.ApiService.Response != null
                            ? App.ApiService.Response.ReasonPhrase
                            : AppResources.failed_to_get_switches;
                        if (App.ApiService.Response != null && App.ApiService.Response.ReasonPhrase == "OK")
                            lblResult.Text = AppResources.failed_to_get_switches;
                    }
                }
                else
                {
                    lblResult.Text = App.ApiService.Response != null
                             ? App.ApiService.Response.ReasonPhrase
                             : AppResources.error_notConnected;
                    if (App.ApiService.Response != null && App.ApiService.Response.ReasonPhrase == "OK")
                        lblResult.Text = AppResources.error_notConnected;
                }

                App.HideLoading();
            }
            IsBusy = false;
        }

        /// <summary>
        /// Check server settings
        /// </summary>
        private void btnCheck_Clicked(object sender, EventArgs e)
        {
            ProcessServerSettings();
        }

        /// <summary>
        /// Check if HTTPS is selected
        /// </summary>
        private void TxtRemoteProtocol_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            lblSSLWarning.IsVisible = string.Compare(txtRemoteProtocol.Items[txtRemoteProtocol.SelectedIndex], "https",
                                          StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}