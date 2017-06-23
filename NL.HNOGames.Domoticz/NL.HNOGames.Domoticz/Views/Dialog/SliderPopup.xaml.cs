using NL.HNOGames.Domoticz.Resources;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NL.HNOGames.Domoticz.Views.Dialog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SliderPopup : PopupPage
    {
        Models.Device oDevice;
        Command cmFinish = null;

        public SliderPopup(Models.Device device, Command finish = null)
        {
            oDevice = device;
            cmFinish = finish;
            InitializeComponent();
            
            lvlTitle.Text = String.Format(AppResources.set_level_switch.Replace("%1$s", "{0}").Replace(" to %2$d", "").Replace("\"",""), oDevice.Name);

            sDimmer.MaximumValue = device.MaxDimLevel;
            sDimmer.UpperValue = device.LevelInt;
            sDimmer.LowerValue = 1;
            sDimmer.MinThumbHidden = true;
            sDimmer.ShowTextAboveThumbs = true;
        }

        private async Task btnSave_Clicked(object sender, EventArgs e)
        {
            await App.ApiService.SetDimmer(oDevice.idx, sDimmer.UpperValue + 1);
            if (cmFinish != null)
                cmFinish.Execute(null);
            await PopupNavigation.PopAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        // Method for animation child in PopupPage
        // Invoced after custom animation end
        protected virtual Task OnAppearingAnimationEnd()
        {
            return Content.FadeTo(0.5);
        }

        // Method for animation child in PopupPage
        // Invoked before custom animation begin
        protected virtual Task OnDisappearingAnimationBegin()
        {
            return Content.FadeTo(1); ;
        }

        protected override bool OnBackButtonPressed()
        {
            // Prevent hide popup
            //return base.OnBackButtonPressed();
            return true;
        }

        // Invoced when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return default value - CloseWhenBackgroundIsClicked
            return base.OnBackgroundClicked();
        }

    }
}
