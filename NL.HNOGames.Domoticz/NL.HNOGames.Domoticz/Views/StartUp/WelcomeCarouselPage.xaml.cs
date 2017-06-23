using Acr.UserDialogs;
using Newtonsoft.Json;
using NL.HNOGames.Domoticz.Helpers;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NL.HNOGames.Domoticz.Views.StartUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomeCarouselPage : CarouselPage, INotifyPropertyChanged
    {
        private ServerSettings oServerSettings;

        public ServerSettings ServerSettings
        { //Property that will be used to get and set the item
            get { return oServerSettings; }
            set
            {
                oServerSettings = value;
                this.BindingContext = oServerSettings;
            }
        }


        public WelcomeCarouselPage()
        {
            ServerSettings = App.AppSettings.ActiveServerSettings;
            if (ServerSettings == null)
                ServerSettings = new ServerSettings();

            InitializeComponent();
            this.BindingContext = ServerSettings;
            btnCheck.IsVisible = true;
        }

        /// <summary>
        /// Read File Content
        /// </summary>
        public async Task<string> ReadFileContent(string fileName, IFolder rootFolder)
        {
            ExistenceCheckResult exist = await rootFolder.CheckExistsAsync(fileName);
            string text = null;
            if (exist == ExistenceCheckResult.FileExists)
            {
                IFile file = await rootFolder.GetFileAsync(fileName);
                text = await file.ReadAllTextAsync();
            }
            return text;
        }

        private void swEnableLocalSettings_Toggled(object sender, ToggledEventArgs e)
        {
            if (swEnableLocalSettings.IsToggled)
                lyLocalSettings.IsVisible = true;
            else
                lyLocalSettings.IsVisible = false;
        }

        /// <summary>
        /// Validate mandatory settings
        /// </summary>
        public bool ValidateServerSettings()
        {
            if (String.IsNullOrEmpty(ServerSettings.REMOTE_SERVER_URL))
                return false;
            if (String.IsNullOrEmpty(ServerSettings.REMOTE_SERVER_PORT))
                return false;
            if (ServerSettings.IS_LOCAL_SERVER_ADDRESS_DIFFERENT)
            {
                if (String.IsNullOrEmpty(ServerSettings.REMOTE_SERVER_URL))
                    return false;
                if (String.IsNullOrEmpty(ServerSettings.REMOTE_SERVER_PORT))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Validate mandatory settings
        /// </summary>
        public bool ValidateServerUrl()
        {
            if (ServerSettings.LOCAL_SERVER_URL.Contains("http") ||
                ServerSettings.REMOTE_SERVER_URL.Contains("http"))
                return false;
            return true;
        }


        /// <summary>
        /// Connection Finished
        /// </summary>
        private async Task btnFinish_Clicked(object sender, EventArgs e)
        {
            App.AppSettings.WelcomeCompleted = true;
            App.SetMainPage();
        }

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
                UserDialogs.Instance.Alert(AppResources.welcome_msg_connectionDataIncomplete + "\n\n" + AppResources.welcome_msg_correctOnPreviousPage);
            else if (!ValidateServerUrl())
                UserDialogs.Instance.Alert(AppResources.welcome_msg_connectionDataInvalid + "\n\n" + AppResources.welcome_msg_correctOnPreviousPage);
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
                    var devices = await App.ApiService.GetDevices(0, null);
                    if (devices != null && devices.result != null)
                    {
                        lblResult.Text += Environment.NewLine + devices.result.Count() + AppResources.welcome_msg_numberOfDevices.Replace("%1$d", "");
                        btnFinish.IsVisible = true;
                        btnCheck.IsVisible = false;
                        imFinish.IsVisible = true;
                    }
                    else
                    {
                        lblResult.Text = App.ApiService.response != null ? App.ApiService.response.ReasonPhrase.ToString() : AppResources.failed_to_get_switches;
                    }
                }
                else
                    lblResult.Text = App.ApiService.response != null ? App.ApiService.response.ReasonPhrase.ToString() : AppResources.error_timeout;

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
        /// IMport settings
        /// </summary>
        private async Task btnImportSettings_Clicked(object sender, EventArgs e)
        {
            try
            {
                var rootFolder = SpecialFolder.Current.Pictures;
                IFolder folder = await rootFolder.CreateFolderAsync("Domoticz",
                   CreationCollisionOption.OpenIfExists);

                string fileContent = await this.ReadFileContent("domoticz_settings.txt", folder);
                var settingsObject = JsonConvert.DeserializeObject<Helpers.Settings>(fileContent);
                App.AppSettings = settingsObject;

                UserDialogs.Instance.Toast(AppResources.settings_imported);
                ServerSettings = App.AppSettings.ActiveServerSettings;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// check if we have a settings file that can be imported
        /// </summary>
        private async Task ContentPage_Appearing(object sender, EventArgs e)
        {
            var rootFolder = SpecialFolder.Current.Pictures;
            IFolder folder = await rootFolder.CreateFolderAsync("Domoticz",
               CreationCollisionOption.OpenIfExists);

            ExistenceCheckResult exist = await folder.CheckExistsAsync("domoticz_settings.txt");
            if (exist == ExistenceCheckResult.FileExists)
                btnImportSettings.IsVisible = true;
            else
                btnImportSettings.IsVisible = false;
        }
    }
}
