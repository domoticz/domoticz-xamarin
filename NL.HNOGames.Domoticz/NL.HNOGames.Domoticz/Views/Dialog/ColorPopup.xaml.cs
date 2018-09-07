using Rg.Plugins.Popup.Services;
using System;
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
         //await App.ApiService.SetDimmer(_oDevice.idx, sDimmer.UpperValue + 1);
         _cmFinish?.Execute(null);
         await PopupNavigation.Instance.PopAsync();
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