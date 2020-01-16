using NL.HNOGames.Domoticz.Resources;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NL.HNOGames.Domoticz.Views
{
    /// <summary>
    /// Defines the <see cref="GraphTabbedPage" />
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GraphTabbedPage
    {
        #region Variables

        /// <summary>
        /// Defines the _lastKnownPage
        /// </summary>
        private GraphPage _lastKnownPage;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphTabbedPage"/> class.
        /// </summary>
        /// <param name="device">The device<see cref="Models.Device"/></param>
        /// <param name="sensor">The sensor<see cref="string"/></param>
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

        #endregion

        #region Private

        /// <summary>
        /// The ToolbarItem_Activated
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void ToolbarItem_Activated(object sender, EventArgs e)
        {
            _lastKnownPage?.FilterAsync();
        }

        #endregion

        /// <summary>
        /// The OnCurrentPageChanged
        /// </summary>
        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            if (CurrentPage.IsEnabled)
                _lastKnownPage = (GraphPage)CurrentPage;
            else
                CurrentPage = _lastKnownPage;
        }
    }
}
