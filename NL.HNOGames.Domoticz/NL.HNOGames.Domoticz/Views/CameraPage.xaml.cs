using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NL.HNOGames.Domoticz.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NL.HNOGames.Domoticz.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPage
    {
        private readonly CameraViewModel _viewModel;

        public CameraPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new CameraViewModel();
        }

        /// <summary>
        /// On Appearing of the screen
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadCamerasCommand.Execute(null);
        }

        private void ListView_OnFlowItemTapped(object sender, ItemTappedEventArgs e)
        {
        }
    }
}