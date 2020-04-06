using Acr.UserDialogs;
using NL.HNOGames.Domoticz.Helpers;
using NL.HNOGames.Domoticz.Resources;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.ViewModels
{
    /// <summary>
    /// Defines the <see cref="OverviewViewModel" />
    /// </summary>
    public class OverviewViewModel : BaseViewModel
    {
        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="OverviewViewModel"/> class.
        /// </summary>
        public OverviewViewModel()
        {
            Title = "Domoticz";
            Plans = new ObservableRangeCollection<Models.Plan>();

            LoadPlansCommand = new Command(async () => await ExecuteLoadPlansCommand(false));
            RefreshPlansCommand = new Command(async () => await ExecuteLoadPlansCommand(true));
            LoadVersionCommand = new Command(async () => await ExecuteLoadVersionCommand());
        }

        #endregion

        #region Delegates

        /// <summary>
        /// The PlansLoaded
        /// </summary>
        public delegate void PlansLoaded();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the PlansLoadedMethod
        /// </summary>
        public PlansLoaded PlansLoadedMethod { get; set; }

        /// <summary>
        /// Gets or sets the Plans
        /// </summary>
        public ObservableRangeCollection<Models.Plan> Plans { get; set; }

        /// <summary>
        /// Gets or sets the LoadPlansCommand
        /// </summary>
        public Command LoadPlansCommand { get; set; }

        /// <summary>
        /// Gets or sets the RefreshPlansCommand
        /// </summary>
        public Command RefreshPlansCommand { get; set; }

        /// <summary>
        /// Gets or sets the LoadVersionCommand
        /// </summary>
        public Command LoadVersionCommand { get; set; }

        #endregion

        #region Private

        /// <summary>
        /// see if there are updates
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task ExecuteLoadVersionCommand()
        {
            try
            {
                var version = await App.ApiService.GetVersion();
                if (version != null && version.HaveUpdate)
                {
                    //Server update available: %1$s to version: %2$s
                    App.ShowToast(AppResources.update_available_enhanced
                        .Replace("%1$s", version.version)
                        .Replace("%2$s", version.Revision.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                }
            }
            catch (Exception)
            {
                UserDialogs.Instance.Alert("Unable to load version of domoticz.");
            }
        }

        /// <summary>
        /// load the house plan
        /// </summary>
        /// <param name="refresh">The refresh<see cref="bool"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task ExecuteLoadPlansCommand(bool refresh)
        {
            if (Plans == null || refresh)
            {
                if (IsBusy)
                    return;
                IsBusy = true;
            }

            try
            {
                var items = await App.ApiService.GetPlans();
                if (items.result != null && items.result.Length > 0)
                {
                    Plans?.ReplaceRange(items.result);
                }

                PlansLoadedMethod?.Invoke();
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }
            IsBusy = false;
        }

        #endregion
    }
}
