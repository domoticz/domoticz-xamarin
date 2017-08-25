using System;
using System.Threading.Tasks;
using NL.HNOGames.Domoticz.Helpers;
using Xamarin.Forms;
using Acr.UserDialogs;
using NL.HNOGames.Domoticz.Resources;

namespace NL.HNOGames.Domoticz.ViewModels
{
    public class OverviewViewModel : BaseViewModel
    {
        public delegate void PlansLoaded();

        public PlansLoaded PlansLoadedMethod { get; set; }

        public ObservableRangeCollection<Models.Plan> Plans { get; set; }
        public Command LoadPlansCommand { get; set; }
        public Command RefreshPlansCommand { get; set; }
        public Command LoadVersionCommand { get; set; }

        public OverviewViewModel()
        {
            Title = "Domoticz";
            Plans = new ObservableRangeCollection<Models.Plan>();

            LoadPlansCommand = new Command(async () => await ExecuteLoadPlansCommand(false));
            RefreshPlansCommand = new Command(async () => await ExecuteLoadPlansCommand(true));
            LoadVersionCommand = new Command(async () => await ExecuteLoadVersionCommand());
        }

        /// <summary>
        /// see if there are updates
        /// </summary>
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
    }
}