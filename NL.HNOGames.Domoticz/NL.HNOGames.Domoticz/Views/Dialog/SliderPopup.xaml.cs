using NL.HNOGames.Domoticz.Resources;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NL.HNOGames.Domoticz.Views.Dialog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public sealed partial class SliderPopup
    {
        private readonly Models.Device _oDevice;
        private readonly Command _cmFinish;

        public SliderPopup(Models.Device device, Command finish = null)
        {
            _oDevice = device;
            _cmFinish = finish;
            InitializeComponent();

            lvlTitle.Text =
                string.Format(
                    AppResources.set_level_switch.Replace("%1$s", "{0}").Replace("%2$d", "").Replace("\"", ""),
                    _oDevice.Name);

            sDimmer.MaxValue = device.MaxDimLevel;
            sDimmer.Value = device.LevelInt;
        }

        /// <summary>
        /// Save the new slider value
        /// </summary>
        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            await App.ApiService.SetDimmer(_oDevice.idx, float.Parse(sDimmer.Value.ToString()) + 1);
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