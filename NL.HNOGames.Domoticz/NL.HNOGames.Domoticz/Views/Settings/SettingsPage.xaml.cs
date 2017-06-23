using Acr.UserDialogs;
using Newtonsoft.Json;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using NL.HNOGames.Domoticz.Views.Dialog;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private SelectMultipleBasePage<ScreenModel> oEnableScreenPage;
        private Command GoToMainScreen;

        public SettingsPage(Command mainScreen)
        {
            this.GoToMainScreen = mainScreen;
            InitializeComponent();
            Title = AppResources.action_settings;

            //Startup Settings
            txtStartScherm.Items.Clear();
            txtStartScherm.Items.Add(AppResources.title_dashboard);
            txtStartScherm.Items.Add(AppResources.title_switches);
            txtStartScherm.Items.Add(AppResources.title_scenes);
            txtStartScherm.Items.Add(AppResources.title_temperature);
            txtStartScherm.Items.Add(AppResources.title_weather);
            txtStartScherm.Items.Add(AppResources.title_utilities);
            txtStartScherm.SelectedIndex = App.AppSettings.StartupScreen;
            txtStartScherm.SelectedIndexChanged += (sender, args) =>
            {
                App.AppSettings.StartupScreen = txtStartScherm.SelectedIndex;
            };


            //Dashboard sort
            swNoSort.IsToggled = App.AppSettings.NoSort;
            lblSort.Text = App.AppSettings.NoSort ? AppResources.sort_dashboardLikeServer_on : AppResources.sort_dashboardLikeServer_off;
            swNoSort.Toggled += (sender, args) =>
            {
                App.AppSettings.NoSort = swNoSort.IsToggled;
                lblSort.Text = App.AppSettings.NoSort ? AppResources.sort_dashboardLikeServer_on : AppResources.sort_dashboardLikeServer_off;
            };

            //Dashboard show switches
            swShowSwitch.IsToggled = App.AppSettings.ShowSwitches;
            lblShowSwitch.Text = App.AppSettings.ShowSwitches ? AppResources.switch_buttons_on : AppResources.switch_buttons_off;
            swShowSwitch.Toggled += (sender, args) =>
            {
                App.AppSettings.ShowSwitches = swShowSwitch.IsToggled;
                lblShowSwitch.Text = App.AppSettings.ShowSwitches ? AppResources.switch_buttons_on : AppResources.switch_buttons_off;
            };

            //Enable notifications
            swEnableNotifications.IsToggled = App.AppSettings.EnableNotifications;
            swEnableNotifications.Toggled += (sender, args) =>
            {
                App.AppSettings.EnableNotifications = swEnableNotifications.IsToggled;
            };

            //Dashboard extra data
            swExtraData.IsToggled = App.AppSettings.ShowExtraData;
            lblExtraData.Text = App.AppSettings.ShowExtraData ? AppResources.show_extra_data_on : AppResources.show_extra_data_off;
            swExtraData.Toggled += (sender, args) =>
            {
                App.AppSettings.ShowExtraData = swExtraData.IsToggled;
                lblExtraData.Text = App.AppSettings.ShowExtraData ? AppResources.show_extra_data_on : AppResources.show_extra_data_off;
            };
        }

        /// <summary>
        /// Save the enable screen selection
        /// </summary>
        void ExecuteSaveEnableScreensCommand()
        {
            if (oEnableScreenPage != null)
            {
                App.AppSettings.EnabledScreens = oEnableScreenPage.GetAllItems();
                GoToMainScreen.Execute(null);
            }
        }

        /// <summary>
        /// Import settings
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
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Export settings
        /// </summary>
        private async Task btnExportSettings_Clicked(object sender, EventArgs e)
        {
            var rootFolder = SpecialFolder.Current.Pictures;
            IFolder folder = await rootFolder.CreateFolderAsync("Domoticz",
                CreationCollisionOption.OpenIfExists);
            IFile file = await folder.CreateFileAsync("domoticz_settings.txt",
                CreationCollisionOption.ReplaceExisting);

            await file.WriteAllTextAsync(JsonConvert.SerializeObject(App.AppSettings));

            UserDialogs.Instance.Toast(AppResources.settings_exported);
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

        /// <summary>
        /// Enable or Disable screens
        /// </summary>
        private async Task btnEnableScreens_Clicked(object sender, EventArgs e)
        {
            var items = App.AppSettings.EnabledScreens;
            if (items == null)
            {
                //setup default screens, all turned on!
                items = new List<ScreenModel>();
                items.Add(new ScreenModel { ID = "Dashboard", Name = AppResources.title_dashboard, IsSelected = true });
                items.Add(new ScreenModel { ID = "Switch", Name = AppResources.title_switches, IsSelected = true });
                items.Add(new ScreenModel { ID = "Scene", Name = AppResources.title_scenes, IsSelected = true });
                items.Add(new ScreenModel { ID = "Temperature", Name = AppResources.title_temperature, IsSelected = true });
                items.Add(new ScreenModel { ID = "Weather", Name = AppResources.title_weather, IsSelected = true });
                items.Add(new ScreenModel { ID = "Utilities", Name = AppResources.title_utilities, IsSelected = true });
            }

            oEnableScreenPage = new SelectMultipleBasePage<ScreenModel>(items, new Command(() => ExecuteSaveEnableScreensCommand()))
            {
                Title = AppResources.enable_items
            };
            await Navigation.PushAsync(oEnableScreenPage);
        }

        private async Task btnServerSetup_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ServerSettingsPage());
        }

        private async Task btnShowLog_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ServerLogsPage());
        }

        private async Task btnUserVars_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserVariablesPage());
        }

        private async Task btnEvents_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EventsPage());
        }
    }
}
