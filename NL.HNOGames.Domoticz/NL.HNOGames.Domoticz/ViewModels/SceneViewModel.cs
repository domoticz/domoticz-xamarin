using System;
using System.Threading.Tasks;

using NL.HNOGames.Domoticz.Helpers;
using NL.HNOGames.Domoticz.Views;

using Xamarin.Forms;
using NL.HNOGames.Domoticz.Resources;

namespace NL.HNOGames.Domoticz.ViewModels
{
    public class SceneViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Models.Scene> Devices { get; set; }
        public Command LoadFavoriteCommand { get; set; }
        public Command RefreshFavoriteCommand { get; set; }
        public Command RefreshActionCommand { get; set; }
        public bool OldData;


        public delegate void SetListViewVisibility(bool visible);

        public SetListViewVisibility SetListViewVisibilityMethod { get; set; }
        public bool SomethingFound = true;


        public SceneViewModel()
        {
            Title = AppResources.title_scenes;
            Devices = new ObservableRangeCollection<Models.Scene>();

            LoadFavoriteCommand = new Command(async () => await ExecuteLoadFavoritesCommand(false));
            RefreshFavoriteCommand = new Command(async () => await ExecuteLoadFavoritesCommand(true));
            RefreshActionCommand = new Command(async () => await ExecuteLoadFavoritesCommand(false));

            if (!LoadCache) return;
            OldData = true;
            Devices = Cache.GetCache<ObservableRangeCollection<Models.Scene>>(GetType().Name) ?? new ObservableRangeCollection<Models.Scene>();
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
                var items = await App.ApiService.GetScenes(null);
                if (items.result != null && items.result.Length > 0)
                {
                    SomethingFound = true;
                    if (Devices != null)
                    {
                        Devices.ReplaceRange(items.result);
                        Cache.SetCache(GetType().Name, Devices);
                    }
                    OldData = false;
                }
                else
                {
                    SomethingFound = false;
                    Devices = new ObservableRangeCollection<Models.Scene>();
                    Cache.SetCache(GetType().Name, Devices);
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
    }
}