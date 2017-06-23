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
    public class OverviewViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Models.Plan> Plans { get; set; }
        public Command LoadPlansCommand { get; set; }
        public Command RefreshPlansCommand { get; set; }

        public OverviewViewModel()
        {
            Title = "Domoticz";
            Plans = new ObservableRangeCollection<Models.Plan>();

            LoadPlansCommand = new Command(async () => await ExecuteLoadPlansCommand(false));
            RefreshPlansCommand = new Command(async () => await ExecuteLoadPlansCommand(true));
        }

        async Task ExecuteRefreshFavoritesCommand()
        {
            await ExecuteLoadPlansCommand(true);
        }

        async Task ExecuteLoadPlansCommand(Boolean refresh)
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
                    Plans.ReplaceRange(items.result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                UserDialogs.Instance.Alert("Unable to load items.");
            }

            IsBusy = false;
        }
    }
}