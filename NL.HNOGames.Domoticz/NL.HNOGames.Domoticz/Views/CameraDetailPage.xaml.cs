using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NL.HNOGames.Domoticz.Models;
using System.IO;
using NL.HNOGames.Domoticz.Controls;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace NL.HNOGames.Domoticz.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraDetailPage
    {
        private readonly Camera _selectedCamera;

        public CameraDetailPage(Camera camera)
        {
            _selectedCamera = camera;
            InitializeComponent();

            Title = camera.Name;
            cameraImage.Source = ImageSource.FromStream(() => new MemoryStream(camera.ImageBytes));
        }

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
    }
}