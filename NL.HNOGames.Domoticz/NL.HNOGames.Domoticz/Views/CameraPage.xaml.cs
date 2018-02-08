using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NL.HNOGames.Domoticz.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NL.HNOGames.Domoticz.Helpers;

namespace NL.HNOGames.Domoticz.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPage
    {
        private readonly CameraViewModel _viewModel;
        private Timer _oTimer;

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

        /// <summary>
        /// On Appearing of the screen
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _oTimer = new Timer((o) => {
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

        /// <summary>
        /// Camera item selected
        /// </summary>
        private async void ListView_OnFlowItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as Models.Camera;
            await Navigation.PushAsync(new CameraDetailPage(item));
        }
    }
}