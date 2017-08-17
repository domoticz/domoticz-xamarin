using Acr.UserDialogs;
using NL.HNOGames.Domoticz.Data;
using NL.HNOGames.Domoticz.Helpers;
using NL.HNOGames.Domoticz.Resources;
using NL.HNOGames.Domoticz.Views;
using NL.HNOGames.Domoticz.Views.StartUp;
using Plugin.TextToSpeech;
using System;
using System.Linq;
using DLToolkit.Forms.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace NL.HNOGames.Domoticz
{
    public partial class App
    {
        private static InitFirebase _initFirebase;
        public delegate void InitFirebase();

        public static ConnectionService ConnectionService { get; private set; }
        public static DataService ApiService { get; private set; }
        public static Settings AppSettings { get; set; }

        public static Models.ConfigModel ServerConfig { get; set; }
        private static IProgressDialog _loadingDialog;

        /// <summary>
        /// Restart Firebase
        /// </summary>
        public static void RestartFirebase()
        {
            _initFirebase?.Invoke();
        }

        /// <summary>
        /// Load the server config
        /// </summary>
        /// <returns></returns>
        public static Models.ConfigModel GetServerConfig()
        {
            if (ServerConfig != null) return ServerConfig;
            ApiService.RefreshConfig();
            ServerConfig = AppSettings.ServerConfig;
            return ServerConfig;
        }

        /// <summary>
        /// Show a loading screen
        /// </summary>
        public static void ShowLoading()
        {
            if (_loadingDialog == null)
                _loadingDialog = UserDialogs.Instance.Loading("Loading", maskType: MaskType.Gradient);
            _loadingDialog.Show();
        }

        /// <summary>
        /// Hide the loading screen
        /// </summary>
        public static void HideLoading()
        {
            _loadingDialog?.Hide();
        }

        public App()
        {
            Init();
        }

        public App(InitFirebase firebase)
        {
            _initFirebase = firebase;
            Init();
        }

        /// <summary>
        /// Init the page
        /// </summary>
        private void Init()
        {
            InitializeComponent();

            FlowListView.Init();
            AppSettings = new Settings { DebugInfo = string.Empty };

            ConnectionService = new ConnectionService();
            ApiService = new DataService { Server = AppSettings.ActiveServerSettings };

            if (Current.Resources == null)
                Current.Resources = new ResourceDictionary();

            Current.Resources.MergedWith = AppSettings.DarkTheme ? (new Themes.Dark()).GetType() : (new Themes.Base()).GetType();
            SetMainPage();
        }

        /// <summary>
        /// Log information
        /// </summary>
        public static void AddLog(string text)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(text);
                AppSettings.AddDebugInfo(text);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("cant write log: " + ex.Message);
            }
        }

        /// <summary>
        /// Show toast information
        /// </summary>
        public static void ShowToast(string text)
        {
            if (string.IsNullOrEmpty(text))
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

        /// <summary>
        /// Setup mainpage
        /// </summary>
        public static void SetMainPage()
        {
            try
            {
                if (!AppSettings.WelcomeCompleted || AppSettings.ActiveServerSettings == null)
                {
                    //show welcome settings screens
                    Current.MainPage = new NavigationPage(new WelcomeCarouselPage());
                }
                else
                {
                    //check if we need a refresh of the config
                    ApiService.RefreshConfig();
                    GetServerConfig();

                    var oOverviewTabbedPage = new OverviewTabbedPage
                    {
                        BarBackgroundColor = Color.FromHex("#22272B"),
                        BarTextColor = Color.White,
                    };

                    var screens = AppSettings.EnabledScreens;
                    if (screens == null || screens.Any(o => o.IsSelected && o.ID == "Dashboard"))
                    {
                        oOverviewTabbedPage.Children.Add(new DashboardPage(ViewModels.DashboardViewModel.ScreenTypeEnum.Dashboard)
                        {
                            Title = AppResources.title_dashboard,
                            Icon = Device.RuntimePlatform == Device.iOS ? "ic_dashboard_white.png" : null,
                        });
                    }
                    if (screens == null || screens.Any(o => o.IsSelected && o.ID == "Switch"))
                    {
                        oOverviewTabbedPage.Children.Add(new DashboardPage(ViewModels.DashboardViewModel.ScreenTypeEnum.Switches)
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
                        oOverviewTabbedPage.Children.Add(new DashboardPage(ViewModels.DashboardViewModel.ScreenTypeEnum.Temperature)
                        {
                            Title = AppResources.title_temperature,
                            Icon = Device.RuntimePlatform == Device.iOS ? "ic_wb_sunny_white.png" : null,
                        });
                    }
                    if (screens == null || screens.Any(o => o.IsSelected && o.ID == "Weather"))
                    {
                        oOverviewTabbedPage.Children.Add(new DashboardPage(ViewModels.DashboardViewModel.ScreenTypeEnum.Weather)
                        {
                            Title = AppResources.title_weather,
                            Icon = Device.RuntimePlatform == Device.iOS ? "ic_wb_cloudy_white.png" : null,
                        });
                    }
                    if (screens == null || screens.Any(o => o.IsSelected && o.ID == "Utilities"))
                    {
                        oOverviewTabbedPage.Children.Add(
                            new DashboardPage(ViewModels.DashboardViewModel.ScreenTypeEnum.Utilities)
                            {
                                Title = AppResources.title_utilities,
                                Icon = Device.RuntimePlatform == Device.iOS ? "ic_highlight_white.png" : null,
                            });
                    }

                    oOverviewTabbedPage.SelectedItem = oOverviewTabbedPage.Children[AppSettings.StartupScreen];
                    Current.MainPage = new NavigationPage(oOverviewTabbedPage);
                }
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
        }
    }
}
