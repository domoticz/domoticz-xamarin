using System;
using System.Threading.Tasks;

using NL.HNOGames.Domoticz.Helpers;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Views;

using Xamarin.Forms;
using NL.HNOGames.Domoticz.Resources;
using System.Linq;

namespace NL.HNOGames.Domoticz.ViewModels
{
    public class CameraViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Camera> Cameras { get; set; }

        public Command LoadCamerasCommand { get; set; }

        public CameraViewModel()
        {
            Title = AppResources.title_cameras;
            Cameras = new ObservableRangeCollection<Camera>();
            LoadCamerasCommand = new Command(async () => await ExecuteLoadCamerasCommand());
        }

        private async Task ExecuteLoadCamerasCommand()
        {
            try
            {
                App.AddLog("Loading camera list");
                var items = await App.ApiService.GetCameras();
                if (items?.result != null && items.result.Length > 0)
                {
                    foreach (var item in items.result)
                        item.ImageBytes = await App.ApiService.GetCameraStream(item.idx);

                    if(Cameras == null) Cameras = new ObservableRangeCollection<Camera>();
                    Cameras.ReplaceRange(items.result);
                }
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
                if (!OverviewTabbedPage.EmptyDialogShown)
                {
                    OverviewTabbedPage.EmptyDialogShown = true;
                    App.ShowToast(AppResources.error_notConnected);
                }
            }
        }
    }
}