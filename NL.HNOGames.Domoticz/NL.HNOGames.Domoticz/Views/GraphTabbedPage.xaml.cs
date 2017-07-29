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
    public partial class GraphTabbedPage : TabbedPage
    {
        private GraphPage _lastKnownPage;

        public GraphTabbedPage(Models.Device device,
            String sensor = "temp")
        {
            InitializeComponent();
            App.AddLog("Loading screen: Graph");
            this.Title = device.Name;
            BarBackgroundColor = Color.FromHex("#22272B");
            BarTextColor = Color.White;

            this.Children.Add(new GraphPage(device, sensor, Data.ConstantValues.GraphRange.Day)
            {
                Title = AppResources.button_status_day,
                Icon = Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS ? "ic_show_chart.png" : null,
            });
            this.Children.Add(new GraphPage(device, sensor, Data.ConstantValues.GraphRange.Month)
            {
                Title = AppResources.button_status_month,
                Icon = Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS ? "ic_show_chart.png" : null,
            });
            this.Children.Add(new GraphPage(device, sensor, Data.ConstantValues.GraphRange.Year)
            {
                Title = AppResources.button_status_year,
                Icon = Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS ? "ic_show_chart.png" : null,
            });
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            if (CurrentPage.IsEnabled)
                _lastKnownPage = (GraphPage)CurrentPage;
            else
                CurrentPage = _lastKnownPage;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void ToolbarItem_Activated(object sender, EventArgs e)
        {
            if(_lastKnownPage != null)
                _lastKnownPage.FilterAsync();
        }
    }
}
