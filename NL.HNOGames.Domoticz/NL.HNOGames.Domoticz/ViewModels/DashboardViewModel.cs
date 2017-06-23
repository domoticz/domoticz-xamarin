using System;
using System.Diagnostics;
using System.Threading.Tasks;

using NL.HNOGames.Domoticz.Helpers;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Views;

using Xamarin.Forms;
using NL.HNOGames.Domoticz.Resources;
using Acr.UserDialogs;
using System.Linq;

namespace NL.HNOGames.Domoticz.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        public enum ScreenType
        {
            Dashboard,
            Switches,
            Temperature, 
            Utilities,
            Weather,
        };

        public ScreenType screenType = ScreenType.Dashboard;

        public ObservableRangeCollection<Models.Device> Devices { get; set; }
        public Command LoadFavoriteCommand { get; set; }
        public Command RefreshFavoriteCommand { get; set; }

        public bool OldData = false;

        public Plan screenPlan = null;

        public DashboardViewModel(ScreenType type, Plan plan)
        {
            screenPlan = plan;
            screenType = type;
            Title = plan != null ? plan.Name: AppResources.title_dashboard;
            Devices = new ObservableRangeCollection<Models.Device>();

            LoadFavoriteCommand = new Command(async () => await ExecuteLoadFavoritesCommand(false));
            RefreshFavoriteCommand = new Command(async () => await ExecuteLoadFavoritesCommand(true));

            if (this.LoadCache)
            {
                OldData = true;
                Devices = Helpers.Cache.GetCache<ObservableRangeCollection<Models.Device>>(screenPlan != null ? screenPlan.idx + screenType.ToString() : screenType.ToString());
                if (Devices == null)
                    Devices = new ObservableRangeCollection<Models.Device>();
                this.LoadCache = false;
            }
        }

        async Task ExecuteRefreshFavoritesCommand()
        {
            await ExecuteLoadFavoritesCommand(true);
        }

        async Task ExecuteLoadFavoritesCommand(Boolean refresh)
        {
            if (Devices == null || refresh)
            {
                if (IsBusy)
                    return;
                IsBusy = true;
            }

            try
            {
                DevicesModel items = null;
                switch(screenType)
                {
                    case ScreenType.Dashboard:
                        items = await App.ApiService.GetFavorites(null);
                        break;
                    case ScreenType.Switches:
                        items = await App.ApiService.GetDevices(screenPlan == null ? 0 : int.Parse(screenPlan.idx), screenPlan == null ? "light" : null);
                        break;
                    case ScreenType.Weather:
                        items = await App.ApiService.GetWeather(null);
                        break;
                    case ScreenType.Temperature:
                        items = await App.ApiService.GetTemperature(null);
                        break;
                    case ScreenType.Utilities:
                        items = await App.ApiService.getUtilities(null);
                        break;
                }

                if (items.result != null && items.result.Length > 0) {
                    if (!App.AppSettings.NoSort)
                        items.result = items.result.OrderBy(o => o.Name).ToArray();

                    foreach (Models.Device d in items.result)
                    {
                        if (!App.AppSettings.ShowExtraData)
                            d.ShowExtraData = false;
                        d.IsDashboard = true;
                    }

                    Devices.ReplaceRange(items.result);
                    Helpers.Cache.SetCache(screenPlan != null ? screenPlan.idx + screenType.ToString() : screenType.ToString(), Devices);
                    OldData = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                if (!OverviewTabbedPage.EmptyDialogShown)
                {
                    UserDialogs.Instance.Alert("Unable to load items.");
                    OverviewTabbedPage.EmptyDialogShown = true;
                }
            }

            IsBusy = false;
        }
    }
}