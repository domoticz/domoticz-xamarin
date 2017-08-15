using System;
using System.Threading.Tasks;

using NL.HNOGames.Domoticz.Helpers;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Views;

using Xamarin.Forms;
using NL.HNOGames.Domoticz.Resources;
using System.Linq;

namespace NL.HNOGames.Domoticz.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        public enum ScreenTypeEnum
        {
            Dashboard,
            Switches,
            Temperature, 
            Utilities,
            Weather,
        };

        public ScreenTypeEnum ScreenType;

        public ObservableRangeCollection<Models.Device> Devices { get; set; }

        public Command LoadFavoriteCommand { get; set; }
        public Command RefreshFavoriteCommand { get; set; }
        public Command RefreshActionCommand { get; set; }

        public bool OldData;
        public Plan ScreenPlan;

        public DashboardViewModel(ScreenTypeEnum type, Plan plan)
        {
            ScreenPlan = plan;
            ScreenType = type;
            Title = plan != null ? plan.Name: AppResources.title_dashboard;
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
                switch(ScreenType)
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

                if (items.result != null && items.result.Length > 0) {
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
    }
}