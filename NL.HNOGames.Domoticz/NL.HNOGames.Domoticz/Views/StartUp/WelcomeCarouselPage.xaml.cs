using Acr.UserDialogs;
using Newtonsoft.Json;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using PCLStorage;
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

        public ServerSettings ServerSettings
        { //Property that will be used to get and set the item
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
        /// Read File Content
        /// </summary>
        public async Task<string> ReadFileContent(string fileName, IFolder rootFolder)
        {
            var exist = await rootFolder.CheckExistsAsync(fileName);
            if (exist != ExistenceCheckResult.FileExists) return null;
            var file = await rootFolder.GetFileAsync(fileName);
            var text = await file.ReadAllTextAsync();
            return text;
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
            //if (String.IsNullOrEmpty(ServerSettings.REMOTE_SERVER_PORT))
            //    return false;
            if (!ServerSettings.IS_LOCAL_SERVER_ADDRESS_DIFFERENT) return true;
            return !string.IsNullOrEmpty(ServerSettings.REMOTE_SERVER_URL);
            //if (String.IsNullOrEmpty(ServerSettings.REMOTE_SERVER_PORT))
            //    return false;
        }

        /// <summary>
        /// Validate mandatory settings
        /// </summary>
        public bool ValidateServerUrl()
        {
            return !ServerSettings.LOCAL_SERVER_URL.Contains("http") && !ServerSettings.REMOTE_SERVER_URL.Contains("http");
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
                    var devices = await App.ApiService.GetDevices();
                    if (devices?.result != null)
                    {
                        lblResult.Text += Environment.NewLine + devices.result.Count() + AppResources.welcome_msg_numberOfDevices.Replace("%1$d", "");
                        btnFinish.IsVisible = true;
                        btnCheck.IsVisible = false;
                        imFinish.IsVisible = true;
                    }
                    else
                    {
                        lblResult.Text = App.ApiService.response != null ? App.ApiService.response.ReasonPhrase : AppResources.failed_to_get_switches;
                    }
                }
                else
                    lblResult.Text = App.ApiService.response != null ? App.ApiService.response.ReasonPhrase : AppResources.error_timeout;

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
                var folder = await rootFolder.CreateFolderAsync("Domoticz",
                   CreationCollisionOption.OpenIfExists);
                var fileContent = await ReadFileContent("domoticz_settings.txt", folder);
                var settingsObject = JsonConvert.DeserializeObject<Helpers.Settings>(fileContent);
                App.AppSettings = settingsObject;

                App.ShowToast(AppResources.settings_imported);
                ServerSettings = App.AppSettings.ActiveServerSettings;
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }
        }

        /// <summary>
        /// check if we have a settings file that can be imported
        /// </summary>
        private async Task ContentPage_Appearing(object sender, EventArgs e)
        {
            var rootFolder = SpecialFolder.Current.Pictures;
            var folder = await rootFolder.CreateFolderAsync("Domoticz",
               CreationCollisionOption.OpenIfExists);

            var exist = await folder.CheckExistsAsync("domoticz_settings.txt");
            btnImportSettings.IsVisible = exist == ExistenceCheckResult.FileExists;
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
