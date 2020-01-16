using NL.HNOGames.Domoticz.Helpers;
using NL.HNOGames.Domoticz.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NL.HNOGames.Domoticz.Views
{
    /// <summary>
    /// Defines the <see cref="CameraPage" />
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPage
    {
        #region Variables

        /// <summary>
        /// Defines the _viewModel
        /// </summary>
        private readonly CameraViewModel _viewModel;

        /// <summary>
        /// Defines the _oTimer
        /// </summary>
        private Timer _oTimer;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraPage"/> class.
        /// </summary>
        public CameraPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new CameraViewModel();

            switch (Device.Idiom)
            {
                case TargetIdiom.Phone:
                    listView.FlowColumnCount = 2;
                    break;
                case TargetIdiom.Tablet:
                    listView.FlowColumnCount = 3;
                    break;
                case TargetIdiom.Desktop:
                    listView.FlowColumnCount = 3;
                    break;
                case TargetIdiom.Unsupported:
                    listView.FlowColumnCount = 3;
                    break;
                default:
                    listView.FlowColumnCount = 2;
                    break;
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// Camera item selected
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="ItemTappedEventArgs"/></param>
        private async void ListView_OnFlowItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as Models.Camera;
            await Navigation.PushAsync(new CameraDetailPage(item));
        }

        #endregion

        /// <summary>
        /// On Appearing of the screen
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _oTimer = new Timer((o) =>
            {
                Device.BeginInvokeOnMainThread(() => _viewModel.LoadCamerasCommand.Execute(null));
            }, null, 0, 5000);
        }

        /// <summary>
        /// On dis appearing
        /// </summary>
        protected override void OnDisappearing()
        {
            _oTimer?.Cancel();
            _oTimer?.Dispose();
            base.OnDisappearing();
        }
    }
}
