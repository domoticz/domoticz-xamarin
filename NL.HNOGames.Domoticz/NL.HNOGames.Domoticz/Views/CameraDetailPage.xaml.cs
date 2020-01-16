using NL.HNOGames.Domoticz.Controls;
using NL.HNOGames.Domoticz.Models;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NL.HNOGames.Domoticz.Views
{
    /// <summary>
    /// Defines the <see cref="CameraDetailPage" />
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraDetailPage
    {
        #region Variables

        /// <summary>
        /// Defines the _selectedCamera
        /// </summary>
        private readonly Camera _selectedCamera;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraDetailPage"/> class.
        /// </summary>
        /// <param name="camera">The camera<see cref="Camera"/></param>
        public CameraDetailPage(Camera camera)
        {
            _selectedCamera = camera;
            InitializeComponent();

            Title = camera.Name;
            cameraImage.Source = ImageSource.FromStream(() => new MemoryStream(camera.ImageBytes));
        }

        #endregion

        #region Private

        /// <summary>
        /// The MenuItem_OnClicked
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void MenuItem_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                    {
                        await DisplayAlert("Need storage", "We need storage permissions for saving the camera image",
                            "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Storage))
                        status = results[Permission.Storage];
                }
                if (status != PermissionStatus.Granted) return;
                DependencyService.Get<IShare>().Share(Title, "", _selectedCamera.ImageBytes);
                App.AddLog("Sharing Image");
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }
        }

        #endregion
    }
}
