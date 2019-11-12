using NL.HNOGames.Domoticz.Resources;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NL.HNOGames.Domoticz.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GraphTabbedPage
    {
        private GraphPage _lastKnownPage;

        public GraphTabbedPage(Models.Device device,
            string sensor = "temp")
        {
            InitializeComponent();
            App.AddLog("Loading screen: Graph");
            Title = device.Name;
            BarBackgroundColor = Color.FromHex("#22272B");
            BarTextColor = Color.White;

            Children.Add(new GraphPage(device, sensor)
            {
                Title = AppResources.button_status_day,
                IconImageSource = Device.RuntimePlatform == Device.iOS ? "ic_show_chart.png" : null,
            });
            Children.Add(new GraphPage(device, sensor, Data.ConstantValues.GraphRange.Month)
            {
                Title = AppResources.button_status_month,
                IconImageSource = Device.RuntimePlatform == Device.iOS ? "ic_show_chart.png" : null,
            });
            Children.Add(new GraphPage(device, sensor, Data.ConstantValues.GraphRange.Year)
            {
                Title = AppResources.button_status_year,
                IconImageSource = Device.RuntimePlatform == Device.iOS ? "ic_show_chart.png" : null,
            });
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            if (CurrentPage.IsEnabled)
                _lastKnownPage = (GraphPage) CurrentPage;
            else
                CurrentPage = _lastKnownPage;
        }

        private void ToolbarItem_Activated(object sender, EventArgs e)
        {
            _lastKnownPage?.FilterAsync();
        }
    }
}