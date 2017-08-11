using NL.HNOGames.Domoticz.Resources;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NL.HNOGames.Domoticz.Views.Dialog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SliderPopup
    {
        readonly Models.Device _oDevice;
        readonly Command _cmFinish;

        public SliderPopup(Models.Device device, Command finish = null)
        {
            _oDevice = device;
            _cmFinish = finish;
            InitializeComponent();
            
            lvlTitle.Text = string.Format(AppResources.set_level_switch.Replace("%1$s", "{0}").Replace(" to %2$d", "").Replace("\"",""), _oDevice.Name);

            sDimmer.MaximumValue = device.MaxDimLevel;
            sDimmer.UpperValue = device.LevelInt;
            sDimmer.LowerValue = 1;
            sDimmer.MinThumbHidden = true;
            sDimmer.ShowTextAboveThumbs = true;
        }

        private async Task btnSave_Clicked(object sender, EventArgs e)
        {
            await App.ApiService.SetDimmer(_oDevice.idx, sDimmer.UpperValue + 1);
            _cmFinish?.Execute(null);
            await PopupNavigation.PopAsync();
        }

        // Method for animation child in PopupPage
        // Invoced after custom animation end
        protected new virtual Task OnAppearingAnimationEnd()
        {
            return Content.FadeTo(0.5);
        }

        // Method for animation child in PopupPage
        // Invoked before custom animation begin
        protected new virtual Task OnDisappearingAnimationBegin()
        {
            return Content.FadeTo(1);
        }

        protected override bool OnBackButtonPressed()
        {
            // Prevent hide popup
            //return base.OnBackButtonPressed();
            return true;
        }
    }
}
