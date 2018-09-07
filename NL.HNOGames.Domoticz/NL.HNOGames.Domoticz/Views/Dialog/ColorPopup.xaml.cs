using Acr.UserDialogs;
using NL.HNOGames.Domoticz.Resources;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NL.HNOGames.Domoticz.Views.Dialog
{
   [XamlCompilation(XamlCompilationOptions.Compile)]
   public sealed partial class ColorPopup
   {
      private readonly Models.Device _oDevice;
      private readonly Command _cmFinish;

      /// <summary>
      /// Constructor
      /// </summary>
      public ColorPopup(Models.Device device, Command finish = null)
      {
         _oDevice = device;
         _cmFinish = finish;
         InitializeComponent();
         if (device.ParsedColor != null)
            colorMixer.EditorsColor = new Color(device.ParsedColor.r.Value, device.ParsedColor.g.Value, device.ParsedColor.b.Value);
         colorMixer.TextColor = btnCancelButton.TextColor;
      }

      /// <summary>
      /// Save the new color 
      /// </summary>
      private async void btnSave_Clicked(object sender, EventArgs e)
      {
         if (_oDevice.Protected)
         {
            var r = await UserDialogs.Instance.PromptAsync(AppResources.welcome_remote_server_password,
                inputType: InputType.Password);
            await Task.Delay(500);
            if (r.Ok)
            {
                var result = await App.ApiService.SetColor(_oDevice.idx, colorMixer.ColorVal.Value, r.Text);
                if (!result)
                   App.ShowToast(AppResources.security_wrong_code);
               _cmFinish?.Execute(null);
               await PopupNavigation.Instance.PopAsync();
            }
         }
         else
         {
            await App.ApiService.SetColor(_oDevice.idx, colorMixer.ColorVal.Value);
            _cmFinish?.Execute(null);
            await PopupNavigation.Instance.PopAsync();
         }
      }

      /// <summary>
      /// Cancel the new slider value
      /// </summary>
      private async void btnCancel_Clicked(object sender, EventArgs e)
      {
         await PopupNavigation.Instance.PopAsync();
      }
   }
}