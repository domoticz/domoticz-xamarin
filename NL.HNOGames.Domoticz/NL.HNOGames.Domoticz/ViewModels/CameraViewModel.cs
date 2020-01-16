using NL.HNOGames.Domoticz.Helpers;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using NL.HNOGames.Domoticz.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.ViewModels
{
    /// <summary>
    /// Defines the <see cref="CameraViewModel" />
    /// </summary>
    public class CameraViewModel : BaseViewModel
    {
        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraViewModel"/> class.
        /// </summary>
        public CameraViewModel()
        {
            Title = AppResources.cameraActivity_name;
            Cameras = new ObservableRangeCollection<Camera>();
            LoadCamerasCommand = new Command(async () => await ExecuteLoadCamerasCommand());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Cameras
        /// </summary>
        public ObservableRangeCollection<Camera> Cameras { get; set; }

        /// <summary>
        /// Gets or sets the LoadCamerasCommand
        /// </summary>
        public Command LoadCamerasCommand { get; set; }

        #endregion

        #region Private

        /// <summary>
        /// The ExecuteLoadCamerasCommand
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
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

                    if (Cameras == null) Cameras = new ObservableRangeCollection<Camera>();
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

        #endregion
    }
}
