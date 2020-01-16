using NL.HNOGames.Domoticz.Helpers;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using NL.HNOGames.Domoticz.Views;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Device = NL.HNOGames.Domoticz.Models.Device;

namespace NL.HNOGames.Domoticz.ViewModels
{
    /// <summary>
    /// Defines the <see cref="DashboardViewModel" />
    /// </summary>
    public class DashboardViewModel : BaseViewModel
    {
        #region Variables

        /// <summary>
        /// Defines the ScreenType
        /// </summary>
        public ScreenTypeEnum ScreenType;

        /// <summary>
        /// Defines the SomethingFound
        /// </summary>
        public bool SomethingFound = true;

        /// <summary>
        /// Defines the OldData
        /// </summary>
        public bool OldData;

        /// <summary>
        /// Defines the ScreenPlan
        /// </summary>
        public Plan ScreenPlan;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardViewModel"/> class.
        /// </summary>
        /// <param name="type">The type<see cref="ScreenTypeEnum"/></param>
        /// <param name="plan">The plan<see cref="Plan"/></param>
        public DashboardViewModel(ScreenTypeEnum type, Plan plan)
        {
            ScreenPlan = plan;
            ScreenType = type;
            Title = plan != null ? plan.Name : AppResources.title_dashboard;
            Devices = new ObservableRangeCollection<Models.Device>();

            LoadFavoriteCommand = new Command(async () => await ExecuteLoadFavoritesCommand(false));
            RefreshFavoriteCommand = new Command(async () => await ExecuteLoadFavoritesCommand(true));
            RefreshActionCommand = new Command(async () => await ExecuteLoadFavoritesCommand(false));

            if (!LoadCache) return;
            OldData = true;
            Devices = Cache.GetCache<ObservableRangeCollection<Models.Device>>(ScreenPlan != null ? ScreenPlan.idx + ScreenType : ScreenType.ToString()) ??
                      new ObservableRangeCollection<Models.Device>();
            LoadCache = false;
        }

        #endregion

        #region Delegates

        /// <summary>
        /// The SetListViewVisibility
        /// </summary>
        /// <param name="visible">The visible<see cref="bool"/></param>
        public delegate void SetListViewVisibility(bool visible);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the SetListViewVisibilityMethod
        /// </summary>
        public SetListViewVisibility SetListViewVisibilityMethod { get; set; }

        /// <summary>
        /// Gets or sets the Devices
        /// </summary>
        public ObservableRangeCollection<Models.Device> Devices { get; set; }

        /// <summary>
        /// Gets or sets the LoadFavoriteCommand
        /// </summary>
        public Command LoadFavoriteCommand { get; set; }

        /// <summary>
        /// Gets or sets the RefreshFavoriteCommand
        /// </summary>
        public Command RefreshFavoriteCommand { get; set; }

        /// <summary>
        /// Gets or sets the RefreshActionCommand
        /// </summary>
        public Command RefreshActionCommand { get; set; }

        #endregion

        #region Private

        /// <summary>
        /// The ExecuteLoadFavoritesCommand
        /// </summary>
        /// <param name="refresh">The refresh<see cref="bool"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task ExecuteLoadFavoritesCommand(bool refresh)
        {
            if (Devices == null || refresh)
            {
                if (IsBusy)
                    return;
                IsBusy = true;
            }

            try
            {
                DevicesModel items;
                switch (ScreenType)
                {
                    case ScreenTypeEnum.Dashboard:
                        items = await App.ApiService.GetFavorites(null);
                        break;
                    case ScreenTypeEnum.Switches:
                        items = await App.ApiService.GetDevices(ScreenPlan == null ? 0 : int.Parse(ScreenPlan.idx), ScreenPlan == null ? "light" : null);
                        break;
                    case ScreenTypeEnum.Weather:
                        items = await App.ApiService.GetWeather(null);
                        break;
                    case ScreenTypeEnum.Temperature:
                        items = await App.ApiService.GetTemperature(null);
                        break;
                    case ScreenTypeEnum.Utilities:
                        items = await App.ApiService.GetUtilities(null);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (items.result != null && items.result.Length > 0)
                {
                    SomethingFound = true;
                    if (!App.AppSettings.NoSort)
                        items.result = items.result.OrderBy(o => o.Name).ToArray();

                    foreach (var d in items.result)
                    {
                        d.ShowExtraData = ScreenType != ScreenTypeEnum.Dashboard || App.AppSettings.ShowExtraData;
                        d.IsDashboard = ScreenType == ScreenTypeEnum.Dashboard;
                    }

                    if (Devices != null)
                    {
                        Devices.ReplaceRange(items.result);
                        Cache.SetCache(ScreenPlan != null ? ScreenPlan.idx + ScreenType : ScreenType.ToString(), Devices);
                    }
                    OldData = false;
                }
                else
                {
                    SomethingFound = false;
                    Devices = new ObservableRangeCollection<Device>();
                    Cache.SetCache(ScreenPlan != null ? ScreenPlan.idx + ScreenType : ScreenType.ToString(), Devices);
                }
                SetListViewVisibilityMethod?.Invoke(SomethingFound);
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
                if (!OverviewTabbedPage.EmptyDialogShown)
                {
                    OverviewTabbedPage.EmptyDialogShown = true;
                    App.ShowToast(AppResources.error_notConnected);
                }
            }

            IsBusy = false;
        }

        #endregion

        /// <summary>
        /// Defines the ScreenTypeEnum
        /// </summary>
        public enum ScreenTypeEnum
        {
            /// <summary>
            /// Defines the Dashboard
            /// </summary>
            Dashboard,

            /// <summary>
            /// Defines the Switches
            /// </summary>
            Switches,

            /// <summary>
            /// Defines the Temperature
            /// </summary>
            Temperature,

            /// <summary>
            /// Defines the Utilities
            /// </summary>
            Utilities,

            /// <summary>
            /// Defines the Weather
            /// </summary>
            Weather,

            /// <summary>
            /// Defines the Plan
            /// </summary>
            Plan,
        };
    }
}
