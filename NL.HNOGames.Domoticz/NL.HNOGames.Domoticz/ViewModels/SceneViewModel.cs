using System;
using System.Diagnostics;
using System.Threading.Tasks;

using NL.HNOGames.Domoticz.Helpers;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Views;

using Xamarin.Forms;
using NL.HNOGames.Domoticz.Resources;
using Acr.UserDialogs;

namespace NL.HNOGames.Domoticz.ViewModels
{
    public class SceneViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Models.Scene> Devices { get; set; }
        public Command LoadFavoriteCommand { get; set; }
        public Command RefreshFavoriteCommand { get; set; }
        public Command RefreshActionCommand { get; set; }
        public bool OldData = false;

        public SceneViewModel()
        {
            Title = AppResources.title_scenes;
            Devices = new ObservableRangeCollection<Models.Scene>();

            LoadFavoriteCommand = new Command(async () => await ExecuteLoadFavoritesCommand(false));
            RefreshFavoriteCommand = new Command(async () => await ExecuteLoadFavoritesCommand(true));
            RefreshActionCommand = new Command(async () => await ExecuteLoadFavoritesCommand(false));
            if (this.LoadCache)
            {
                OldData = true;
                Devices = Helpers.Cache.GetCache<ObservableRangeCollection<Models.Scene>>(this.GetType().Name);
                if (Devices == null)
                    Devices = new ObservableRangeCollection<Models.Scene>();
                this.LoadCache = false;
            }
        }

        async Task ExecuteRefreshFavoritesCommand()
        {
            await ExecuteLoadFavoritesCommand(true);
        }

        async Task ExecuteLoadFavoritesCommand(bool refresh)
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
                    Devices.ReplaceRange(items.result);
                    Helpers.Cache.SetCache(this.GetType().Name, Devices);
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