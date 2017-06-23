using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using NL.HNOGames.Domoticz.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NL.HNOGames.Domoticz.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OverviewTabbedPage : TabbedPage
    {
        OverviewViewModel viewModel;
        public static bool EmptyDialogShown = false;

        public OverviewTabbedPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new OverviewViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.RefreshPlansCommand.Execute(null);
        }

        /// <summary>
        /// Show action sheet with plans
        /// </summary>
        public async void OnShowPlansClick(object o, EventArgs e)
        {
            if (viewModel.Plans != null && viewModel.Plans.Count > 0)
            {
                List<String> plans = new List<string>();
                foreach (Plan p in viewModel.Plans)
                    plans.Add(p.Name);
                var selectedPlanName = await DisplayActionSheet(AppResources.title_plans, null, null, plans.ToArray());
                var selectedPlan = viewModel.Plans.Where(q => q.Name == selectedPlanName).FirstOrDefault();
                if (selectedPlan != null)
                    await Navigation.PushAsync(new DashboardPage( DashboardViewModel.ScreenType.Switches, selectedPlan));
            }
        }

        /// <summary>
        /// Show all settings
        /// </summary>
        public async void OnSettingsClick(object o, EventArgs e)
        {
            await Navigation.PushAsync(new Settings.SettingsPage(new Command(async () => await BreakingSettingsChanged())));
        }

        /// <summary>
        /// Refresh mainscreen
        /// </summary>
        async Task BreakingSettingsChanged()
        {
            App.SetMainPage();
        }
    }
}
