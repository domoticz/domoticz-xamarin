using Acr.UserDialogs;
using DLToolkit.Forms.Controls;
using NL.HNOGames.Domoticz.Data;
using NL.HNOGames.Domoticz.Helpers;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using NL.HNOGames.Domoticz.Views;
using NL.HNOGames.Domoticz.Views.StartUp;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using Plugin.Multilingual;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Device = Xamarin.Forms.Device;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace NL.HNOGames.Domoticz
{
    /// <summary>
    /// Defines the <see cref="App" />
    /// </summary>
    public partial class App
    {
        #region Variables

        /// <summary>
        /// Defines the _loadingDialog
        /// </summary>
        private static IProgressDialog _loadingDialog;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            Init();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the ConnectionService
        /// </summary>
        public static ConnectionService ConnectionService { get; private set; }

        /// <summary>
        /// Gets the ApiService
        /// </summary>
        public static DataService ApiService { get; private set; }

        /// <summary>
        /// Gets the AppSettings
        /// </summary>
        public static Settings AppSettings { get; private set; }

        /// <summary>
        /// Gets the SunRiseInfo
        /// </summary>
        public static SunRiseModel SunRiseInfo { get; private set; }

        /// <summary>
        /// Gets or sets the ServerConfig
        /// </summary>
        private static ConfigModel ServerConfig { get; set; }

        #endregion

        #region Public

        /// <summary>
        /// Load the server config
        /// </summary>
        /// <returns></returns>
        public static ConfigModel GetServerConfig()
        {
            if (ServerConfig != null) return ServerConfig;
            ApiService.RefreshConfig();
            ServerConfig = AppSettings.ServerConfig;
            return ServerConfig;
        }

        /// <summary>
        /// Load the server config
        /// </summary>
        /// <returns></returns>
        public static async Task<SunRiseModel> GetSunRiseInfoAsync()
        {
            if (SunRiseInfo != null) return SunRiseInfo;
            SunRiseInfo = await ApiService.GetSunRise();
            return SunRiseInfo;
        }

        /// <summary>
        /// Show a loading screen
        /// </summary>
        /// <param name="text">The text<see cref="string"/></param>
        public static void ShowLoading(string text = null)
        {
            if (string.IsNullOrEmpty(text))
                text = AppResources.text_loading;

            try
            {
                _loadingDialog = UserDialogs.Instance.Loading(text, maskType: MaskType.Gradient);
                _loadingDialog.Show();
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
        }

        /// <summary>
        /// Show a loading screen with cancel button
        /// </summary>
        /// <param name="text">The text<see cref="string"/></param>
        /// <param name="cancelText">The cancelText<see cref="string"/></param>
        /// <param name="cts">The cts<see cref="CancellationTokenSource"/></param>
        public static void ShowLoading(string text, string cancelText, CancellationTokenSource cts)
        {
            if (string.IsNullOrEmpty(text))
                text = AppResources.text_loading;

            Action ca = null;
            if (cts != null)
                ca = () => cts.Cancel();

            try
            {
                _loadingDialog = UserDialogs.Instance.Loading(text, ca, cancelText, maskType: MaskType.Gradient);
                _loadingDialog.Show();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// Hide the loading screen
        /// </summary>
        public static void HideLoading()
        {
            _loadingDialog?.Hide();
        }

        /// <summary>
        /// Log information
        /// </summary>
        /// <param name="text">The text<see cref="string"/></param>
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
        /// <param name="text">The text<see cref="string"/></param>
        public static async void ShowToast(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;
            try
            {
                AddLog(text);
                UserDialogs.Instance.Toast(text);
                if (AppSettings.TalkBackEnabled) 
                    await TextToSpeech.SpeakAsync(text);
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
                            IconImageSource = Device.RuntimePlatform == Device.iOS ? "ic_dashboard_white.png" : null,
                        });
                    }
                    if (screens == null || screens.Any(o => o.IsSelected && o.ID == "Switch"))
                    {
                        oOverviewTabbedPage.Children.Add(new DashboardPage(ViewModels.DashboardViewModel.ScreenTypeEnum.Switches)
                        {
                            Title = AppResources.title_switches,
                            IconImageSource = Device.RuntimePlatform == Device.iOS ? "ic_view_carousel_white.png" : null,
                        });
                    }
                    if (screens == null || screens.Any(o => o.IsSelected && o.ID == "Scene"))
                    {
                        oOverviewTabbedPage.Children.Add(new ScenePage()
                        {
                            Title = AppResources.title_scenes,
                            IconImageSource = Device.RuntimePlatform == Device.iOS ? "ic_lightbulb_outline.png" : null,
                        });
                    }
                    if (screens == null || screens.Any(o => o.IsSelected && o.ID == "Temperature"))
                    {
                        oOverviewTabbedPage.Children.Add(new DashboardPage(ViewModels.DashboardViewModel.ScreenTypeEnum.Temperature)
                        {
                            Title = AppResources.title_temperature,
                            IconImageSource = Device.RuntimePlatform == Device.iOS ? "ic_wb_sunny_white.png" : null,
                        });
                    }
                    if (screens == null || screens.Any(o => o.IsSelected && o.ID == "Weather"))
                    {
                        oOverviewTabbedPage.Children.Add(new DashboardPage(ViewModels.DashboardViewModel.ScreenTypeEnum.Weather)
                        {
                            Title = AppResources.title_weather,
                            IconImageSource = Device.RuntimePlatform == Device.iOS ? "ic_wb_cloudy_white.png" : null,
                        });
                    }
                    if (screens == null || screens.Any(o => o.IsSelected && o.ID == "Utilities"))
                    {
                        oOverviewTabbedPage.Children.Add(
                            new DashboardPage(ViewModels.DashboardViewModel.ScreenTypeEnum.Utilities)
                            {
                                Title = AppResources.title_utilities,
                                IconImageSource = Device.RuntimePlatform == Device.iOS ? "ic_highlight_white.png" : null,
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

        #endregion

        #region Private

        /// <summary>
        /// Init the page
        /// </summary>
        private void Init()
        {
            InitializeComponent();

            FlowListView.Init();
            AppSettings = new Settings { DebugInfo = string.Empty };

            SetLanguage();
            ConnectionService = new ConnectionService();
            ApiService = new DataService { Server = AppSettings.ActiveServerSettings };

            if (Current.Resources == null)
                Current.Resources = new ResourceDictionary();

            SetTheme();
            SetMainPage();
            CheckFingerprint();
        }

        /// <summary>
        /// Store the latest know token
        /// </summary>
        /// <param name="token"></param>
        private async void registerAsync(string token)
        {
            AddLog(string.Format("GCM: Push Notification - Device Registered - Token : {0}", token));
            var Id = UsefulBits.GetDeviceID();
            bool bSuccess = await ApiService.RegisterDevice(Id, token);
            if (bSuccess)
                AddLog("GCM: Device registered on Domoticz");
            else
                AddLog("GCM: Device not registered on Domoticz");
        }

        /// <summary>
        /// Check fingerprint security
        /// </summary>
        private static async void CheckFingerprint()
        {
            if (AppSettings.EnableFingerprintSecurity)
            {
                var result = await CrossFingerprint.Current.AuthenticateAsync(new AuthenticationRequestConfiguration(AppResources.category_startup_security, String.Empty)
                {
                    AllowAlternativeAuthentication = true,
                    CancelTitle = AppResources.cancel,
                    FallbackTitle = (Device.RuntimePlatform != Device.Android) ? AppResources.welcome_local_server_password : string.Empty,
                });
                switch (result.Status)
                {
                    case FingerprintAuthenticationResultStatus.Succeeded:
                        break;
                    case FingerprintAuthenticationResultStatus.FallbackRequested:
                        var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password, inputType: InputType.Password);
                        await Task.Delay(500);
                        if (!r.Ok || string.IsNullOrEmpty(r.Text) ||
                           (r.Text != AppSettings.ActiveServerSettings.LOCAL_SERVER_PASSWORD && r.Text != AppSettings.ActiveServerSettings.REMOTE_SERVER_PASSWORD))
                        {
                            App.AddLog("Not authorized for login");
                            DependencyService.Get<ICloseApplication>().Close();//close the application
                        }
                        break;
                    default:// All other options
                        App.AddLog("Not authorized for login");
                        DependencyService.Get<ICloseApplication>().Close();//close the application
                        break;
                }
            }
        }

        /// <summary>
        /// Set language
        /// </summary>
        private static void SetLanguage()
        {
            try
            {
                //set language
                if (!string.IsNullOrEmpty(App.AppSettings.SpecifiedLanguage))
                {
                    CrossMultilingual.Current.CurrentCultureInfo = CrossMultilingual.Current.NeutralCultureInfoList.ToList().First(element => element.EnglishName.Contains(App.AppSettings.SpecifiedLanguage));
                    AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;
                }
                else
                {
                    AppResources.Culture = CrossMultilingual.Current.DeviceCultureInfo;
                    App.AppSettings.SpecifiedLanguage = CrossMultilingual.Current.DeviceCultureInfo.EnglishName;
                }
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// Set the theme of the app (Dark or light)
        /// </summary>
        private static void SetTheme()
        {
            Type merge;
            if (AppSettings.DarkTheme)
            {
                if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
                    merge = (new Themes.DarkAndroid()).GetType();
                else
                    merge = (new Themes.DarkiOS()).GetType();
            }
            else
                merge = (new Themes.Base()).GetType();
            Current.Resources.MergedWith = merge;
        }

        #endregion

        /// <summary>
        /// On start, register the notification services
        /// </summary>
        protected override void OnStart()
        {
            //// Handle when your app starts
            //CrossFirebasePushNotification.Current.Subscribe("general");
            //CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            //{
            //    registerAsync(p.Token);
            //    System.Diagnostics.Debug.WriteLine($"TOKEN REC: {p.Token}");
            //};
            //registerAsync(CrossFirebasePushNotification.Current.Token);

            //CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            //{
            //    try
            //    {
            //        AddLog("GCM: Notification received");
            //        AddLog(p.Data.ToString());

            //        var body = p.Data.ContainsKey("body") ? p.Data["body"].ToString() : null;
            //        var title = p.Data.ContainsKey("title") ? p.Data["title"].ToString() : null;
            //        if (string.IsNullOrEmpty(title))
            //            title = p.Data.ContainsKey("subject") ? p.Data["subject"].ToString() : null;
            //        if (string.Compare(title, body, true) == 0)
            //            title = "Domoticz";

            //        // Show dialog
            //        UserDialogs.Instance.Alert(body, title, AppResources.ok);
            //    }
            //    catch (Exception)
            //    { }
            //};
        }
    }
}
