using Acr.UserDialogs;
using Newtonsoft.Json;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NL.HNOGames.Domoticz.Views.StartUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomeCarouselPage
    {
        private ServerSettings _oServerSettings;

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

        /// <summary>
        /// INit welcome screens
        /// </summary>
        public WelcomeCarouselPage()
        {
            ServerSettings = App.AppSettings.ActiveServerSettings ?? new ServerSettings();
            InitializeComponent();
            BindingContext = ServerSettings;
            btnCheck.IsVisible = true;
        }

        /// <summary>
        /// Show local settings
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
            if (!ServerSettings.IS_LOCAL_SERVER_ADDRESS_DIFFERENT) return true;
            return !string.IsNullOrEmpty(ServerSettings.REMOTE_SERVER_URL);
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
            App.AppSettings.WelcomeCompleted = true;
            App.SetMainPage();
        }

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
                    }
                }
                else
                    lblResult.Text = App.ApiService.Response != null
                        ? App.ApiService.Response.ReasonPhrase
                        : AppResources.error_timeout;

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
        /// show next page
        /// </summary>
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
        /// show previous page
        /// </summary>
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
    }
}