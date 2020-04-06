using NL.HNOGames.Domoticz.Resources;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NL.HNOGames.Domoticz.Views.Dialog
{
    /// <summary>
    /// Defines the <see cref="SliderPopup" />
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public sealed partial class SliderPopup
    {
        #region Variables

        /// <summary>
        /// Defines the _oDevice
        /// </summary>
        private readonly Models.Device _oDevice;

        /// <summary>
        /// Defines the _cmFinish
        /// </summary>
        private readonly Command _cmFinish;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SliderPopup"/> class.
        /// </summary>
        /// <param name="device">The device<see cref="Models.Device"/></param>
        /// <param name="finish">The finish<see cref="Command"/></param>
        public SliderPopup(Models.Device device, Command finish = null)
        {
            _oDevice = device;
            _cmFinish = finish;
            InitializeComponent();

            lvlTitle.Text =
                string.Format(
                    AppResources.set_level_switch.Replace("%1$s", "{0}").Replace("%2$d", "").Replace("\"", ""),
                    _oDevice.Name);

            try
            {
                sDimmer.MaxValue = device.MaxDimLevel;
                sDimmer.Value = device.LevelInt;
            }
            catch (Exception)
            {
                sDimmer.MaxValue = 100;
                sDimmer.Value = 1;
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// Save the new slider value
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            await App.ApiService.SetDimmer(_oDevice.idx, float.Parse(sDimmer.Value.ToString()) + 1);
            _cmFinish?.Execute(null);
            await PopupNavigation.Instance.PopAsync();
        }

        /// <summary>
        /// Cancel the new slider value
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        #endregion
    }
}
