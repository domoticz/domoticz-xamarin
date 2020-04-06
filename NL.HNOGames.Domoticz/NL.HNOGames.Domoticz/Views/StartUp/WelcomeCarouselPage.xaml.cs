using Acr.UserDialogs;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NL.HNOGames.Domoticz.Views.StartUp
{
    /// <summary>
    /// Defines the <see cref="WelcomeCarouselPage" />
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomeCarouselPage
    {
        #region Variables

        /// <summary>
        /// Defines the _oServerSettings
        /// </summary>
        private ServerSettings _oServerSettings;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="WelcomeCarouselPage"/> class.
        /// </summary>
        public WelcomeCarouselPage()
        {
            ServerSettings = App.AppSettings.ActiveServerSettings ?? new ServerSettings();
            InitializeComponent();
            BindingContext = ServerSettings;
            btnCheck.IsVisible = true;

            lblSSLWarning.IsVisible = string.Compare(txtRemoteProtocol.Items[txtRemoteProtocol.SelectedIndex], "https",
                                          StringComparison.OrdinalIgnoreCase) == 0;

            //set entry flow
            txtRemoteServerAddress.Completed += (object sender, EventArgs e) => { txtRemotePort.Focus(); };
            txtRemotePort.Completed += (object sender, EventArgs e) => { txtRemoteDirectory.Focus(); };
            txtRemoteDirectory.Completed += (object sender, EventArgs e) => { txtRemoteUsername.Focus(); };
            txtRemoteUsername.Completed += (object sender, EventArgs e) => { txtRemotePassword.Focus(); };
            txtLocalServerAddress.Completed += (object sender, EventArgs e) => { txtLocalPort.Focus(); };
            txtLocalPort.Completed += (object sender, EventArgs e) => { txtLocalDirectory.Focus(); };
            txtLocalDirectory.Completed += (object sender, EventArgs e) => { txtLocalUsername.Focus(); };
            txtLocalUsername.Completed += (object sender, EventArgs e) => { txtLocalPassword.Focus(); };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ServerSettings
        /// </summary>
        private ServerSettings ServerSettings
        {
            //Property that will be used to get and set the item
            get => _oServerSettings;
            set
            {
                _oServerSettings = value;
                BindingContext = _oServerSettings;
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// Validate mandatory settings
        /// </summary>
        /// <returns>The <see cref="bool"/></returns>
        public bool ValidateServerSettings()
        {
            if (string.IsNullOrEmpty(ServerSettings.REMOTE_SERVER_URL))
                return false;
            if (!ServerSettings.IS_LOCAL_SERVER_ADDRESS_DIFFERENT) return true;
            return !string.IsNullOrEmpty(ServerSettings.REMOTE_SERVER_URL);
        }

        /// <summary>
        /// Validate mandatory settings
        /// </summary>
        /// <returns>The <see cref="bool"/></returns>
        public bool ValidateServerUrl()
        {
            return !ServerSettings.LOCAL_SERVER_URL.Contains("http") &&
                   !ServerSettings.REMOTE_SERVER_URL.Contains("http");
        }

        #endregion

        #region Private

        /// <summary>
        /// Show local settings
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="ToggledEventArgs"/></param>
        private void swEnableLocalSettings_Toggled(object sender, ToggledEventArgs e)
        {
            lyLocalSettings.IsVisible = swEnableLocalSettings.IsToggled;
        }

        /// <summary>
        /// Connection Finished
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnFinish_Clicked(object sender, EventArgs e)
        {
            App.AppSettings.WelcomeCompleted = true;
            App.SetMainPage();
        }

        /// <summary>
        /// The ProcessServerSettings
        /// </summary>
        private async void ProcessServerSettings()
        {
            if (IsBusy)
                return;

            IsBusy = true;
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

                App.ApiService.Server = App.AppSettings.ActiveServerSettings;
                App.ShowLoading();
                var result = await App.ApiService.GetVersion();
                if (result != null)
                {
                    lblResult.Text = AppResources.welcome_msg_serverVersion + ": " + result.version;
                    var devices = await App.ApiService.GetDevices();
                    if (devices?.result != null)
                    {
                        lblResult.Text += Environment.NewLine + devices.result.Count() +
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
                    if (!(await App.ApiService.CheckHttpCertificate()))
                        lblResult.Text = AppResources.http_certificate;
                }

                App.HideLoading();
            }
            IsBusy = false;
        }

        /// <summary>
        /// Check server settings
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnCheck_Clicked(object sender, EventArgs e)
        {
            ProcessServerSettings();
        }

        /// <summary>
        /// show next page
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnNext_Clicked(object sender, EventArgs e)
        {
            var pageCount = Children.Count;
            if (pageCount < 2)
                return;

            var index = Children.IndexOf(CurrentPage);
            index++;
            if (index >= pageCount)
                index = 0;

            CurrentPage = Children[index];
        }

        /// <summary>
        /// reset all values
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnReset_Clicked(object sender, EventArgs e)
        {
            swEnableLocalSettings.IsToggled = false;
            txtRemoteProtocol.SelectedIndex = 0;
            txtRemotePort.Text = "";
            txtRemoteServerAddress.Text = "";
            txtRemoteUsername.Text = "";
            txtRemotePassword.Text = "";
            txtRemoteDirectory.Text = "";

            txtLocalProtocol.SelectedIndex = 0;
            txtLocalPort.Text = "";
            txtLocalServerAddress.Text = "";
            txtLocalUsername.Text = "";
            txtLocalPassword.Text = "";
            txtLocalDirectory.Text = "";
        }

        /// <summary>
        /// show previous page
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnPrevious_Clicked(object sender, EventArgs e)
        {
            var pageCount = Children.Count;
            if (pageCount < 2)
                return;

            var index = Children.IndexOf(CurrentPage);
            index--;
            if (index <= 0)
                index = 0;
            CurrentPage = Children[index];
        }

        /// <summary>
        /// Check if HTTPS is selected
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void TxtRemoteProtocol_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            lblSSLWarning.IsVisible = string.Compare(txtRemoteProtocol.Items[txtRemoteProtocol.SelectedIndex], "https",
                                          StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>
        /// show demo account
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void BtnDemo_OnClicked(object sender, EventArgs e)
        {
            CurrentPage = Children[2];
            swEnableLocalSettings.IsToggled = false;
            txtRemoteProtocol.SelectedIndex = 1;
            txtRemotePort.Text = "1883";
            txtRemoteServerAddress.Text = "gandalf.domoticz.com";
            txtRemoteUsername.Text = "admin";
            txtRemotePassword.Text = "D@m@t1czCl0ud";

            //clear local info
            txtLocalProtocol.SelectedIndex = 0;
            txtLocalPort.Text = "";
            txtLocalServerAddress.Text = "";
            txtLocalUsername.Text = "";
            txtLocalPassword.Text = "";
            txtLocalDirectory.Text = "";
        }

        #endregion
    }
}
