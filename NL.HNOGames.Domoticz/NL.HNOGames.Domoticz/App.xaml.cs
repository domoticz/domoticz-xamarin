using Acr.UserDialogs;
using NL.HNOGames.Domoticz.Data;
using NL.HNOGames.Domoticz.Helpers;
using NL.HNOGames.Domoticz.Resources;
using NL.HNOGames.Domoticz.Views;
using NL.HNOGames.Domoticz.Views.StartUp;
using Plugin.TextToSpeech;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace NL.HNOGames.Domoticz
{
    public partial class App : Application
    {
        public static bool ShowAds { get; set; }

        public static ConnectionService ConnectionService { get; private set; }
        public static DataService ApiService { get; private set; }
        public static Settings AppSettings { get; set; }

        public static NL.HNOGames.Domoticz.Models.ConfigModel ServerConfig { get; set; }
        private static IProgressDialog loadingDialog = null;

        /// <summary>
        /// Load the server config
        /// </summary>
        /// <returns></returns>
        public static NL.HNOGames.Domoticz.Models.ConfigModel getServerConfig()
        {
            if (ServerConfig == null)
            {
                ApiService.RefreshConfig();
                ServerConfig = AppSettings.ServerConfig;
            }
            return ServerConfig;
        }

        /// <summary>
        /// Show a loading screen
        /// </summary>
        public static void ShowLoading()
        {
            if (loadingDialog == null)
                loadingDialog = UserDialogs.Instance.Loading("Loading", maskType: MaskType.Gradient);
            loadingDialog.Show();
        }

        /// <summary>
        /// Hide the loading screen
        /// </summary>
        public static void HideLoading()
        {
            if (loadingDialog == null)
                return;//nothing to hide
            else
                loadingDialog.Hide();
        }

        public App()
        {
            ShowAds = false;//default

            InitializeComponent();

            AppSettings = new Settings();
            AppSettings.DebugInfo = String.Empty;

            ConnectionService = new ConnectionService();
            ApiService = new DataService();
            ApiService.Server = App.AppSettings.ActiveServerSettings;

            if (App.Current.Resources == null)
                App.Current.Resources = new ResourceDictionary();
            if (App.AppSettings.DarkTheme)
                App.Current.Resources.MergedWith = (new Themes.Dark()).GetType();
            else
                App.Current.Resources.MergedWith = (new Themes.Base()).GetType();

            SetMainPage();
        }

        /// <summary>
        /// Log information
        /// </summary>
        public static void AddLog(String text)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(text);
                App.AppSettings.AddDebugInfo(text);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("cant write log: " + ex.Message);
            }
        }

        /// <summary>
        /// Show toast information
        /// </summary>
        public static void ShowToast(String text)
        {
            if (String.IsNullOrEmpty(text))
                return;
            try
            {
                AddLog(text);
                UserDialogs.Instance.Toast(text);
                if (AppSettings.TalkBackEnabled)
                    Device.BeginInvokeOnMainThread(async () => { await CrossTextToSpeech.Current.Speak(text); });
            }
            catch (Exception ex)
            {
                AddLog("cant show toast: " + ex.Message);
            }
        }

        public static void SetMainPage()
        {
            try
            {
                if (!App.AppSettings.WelcomeCompleted || App.AppSettings.ActiveServerSettings == null)
                {
                    //show welcome settings screens
                    Current.MainPage = new NavigationPage(new WelcomeCarouselPage());
                }
                else
                {
                    //check if we need a refresh of the config
                    ApiService.RefreshConfig();
                    getServerConfig();

                    OverviewTabbedPage oOverviewTabbedPage = new OverviewTabbedPage
                    {
                        BarBackgroundColor = Color.FromHex("#22272B"),
                        BarTextColor = Color.White,
                    };

                    var screens = AppSettings.EnabledScreens;
                    if (screens == null || screens.Any(o => o.IsSelected && o.ID == "Dashboard"))
                    {
                        oOverviewTabbedPage.Children.Add(new DashboardPage(ViewModels.DashboardViewModel.ScreenType.Dashboard)
                        {
                            Title = AppResources.title_dashboard,
                            Icon = Device.RuntimePlatform == Device.iOS ? "ic_dashboard_white.png" : null,
                        });
                    }
                    if (screens == null || screens.Any(o => o.IsSelected && o.ID == "Switch"))
                    {
                        oOverviewTabbedPage.Children.Add(new DashboardPage(ViewModels.DashboardViewModel.ScreenType.Switches)
                        {
                            Title = AppResources.title_switches,
                            Icon = Device.RuntimePlatform == Device.iOS ? "ic_view_carousel_white.png" : null,
                        });
                    }
                    if (screens == null || screens.Any(o => o.IsSelected && o.ID == "Scene"))
                    {
                        oOverviewTabbedPage.Children.Add(new ScenePage()
                        {
                            Title = AppResources.title_scenes,
                            Icon = Device.RuntimePlatform == Device.iOS ? "ic_lightbulb_outline.png" : null,
                        });
                    }
                    if (screens == null || screens.Any(o => o.IsSelected && o.ID == "Temperature"))
                    {
                        oOverviewTabbedPage.Children.Add(new DashboardPage(ViewModels.DashboardViewModel.ScreenType.Temperature)
                        {
                            Title = AppResources.title_temperature,
                            Icon = Device.RuntimePlatform == Device.iOS ? "ic_wb_sunny_white.png" : null,
                        });
                    }
                    if (screens == null || screens.Any(o => o.IsSelected && o.ID == "Weather"))
                    {
                        oOverviewTabbedPage.Children.Add(new DashboardPage(ViewModels.DashboardViewModel.ScreenType.Weather)
                        {
                            Title = AppResources.title_weather,
                            Icon = Device.RuntimePlatform == Device.iOS ? "ic_wb_cloudy_white.png" : null,
                        });
                    }
                    if (screens == null || screens.Any(o => o.IsSelected && o.ID == "Utilities"))
                    {
                        oOverviewTabbedPage.Children.Add(new DashboardPage(ViewModels.DashboardViewModel.ScreenType.Utilities)
                        {
                            Title = AppResources.title_utilities,
                            Icon = Device.RuntimePlatform == Device.iOS ? "ic_highlight_white.png" : null,
                        });
                    }

                    oOverviewTabbedPage.SelectedItem = oOverviewTabbedPage.Children[App.AppSettings.StartupScreen];
                    Current.MainPage = new NavigationPage(oOverviewTabbedPage);
                }
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }
        }
    }
}
