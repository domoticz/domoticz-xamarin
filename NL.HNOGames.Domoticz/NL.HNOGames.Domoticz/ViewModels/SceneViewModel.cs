using NL.HNOGames.Domoticz.Helpers;
using NL.HNOGames.Domoticz.Resources;
using NL.HNOGames.Domoticz.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.ViewModels
{
    /// <summary>
    /// Defines the <see cref="SceneViewModel" />
    /// </summary>
    public class SceneViewModel : BaseViewModel
    {
        #region Variables

        /// <summary>
        /// Defines the OldData
        /// </summary>
        public bool OldData;

        /// <summary>
        /// Defines the SomethingFound
        /// </summary>
        public bool SomethingFound = true;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneViewModel"/> class.
        /// </summary>
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
        /// Gets or sets the Devices
        /// </summary>
        public ObservableRangeCollection<Models.Scene> Devices { get; set; }

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

        /// <summary>
        /// Gets or sets the SetListViewVisibilityMethod
        /// </summary>
        public SetListViewVisibility SetListViewVisibilityMethod { get; set; }

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

        #endregion
    }
}
