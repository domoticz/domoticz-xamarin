using Acr.UserDialogs;
using NL.HNOGames.Domoticz.Resources;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NL.HNOGames.Domoticz.Views.Dialog
{
    /// <summary>
    /// Defines the <see cref="ColorPopup" />
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public sealed partial class ColorPopup
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
        /// Initializes a new instance of the <see cref="ColorPopup"/> class.
        /// </summary>
        /// <param name="device">The device<see cref="Models.Device"/></param>
        /// <param name="finish">The finish<see cref="Command"/></param>
        public ColorPopup(Models.Device device, Command finish = null)
        {
            _oDevice = device;
            _cmFinish = finish;
            InitializeComponent();
            if (device.ParsedColor != null)
                colorMixer.ColorVal.Value = new Color(device.ParsedColor.r.Value, device.ParsedColor.g.Value, device.ParsedColor.b.Value);
            colorMixer.TextColor = btnCancelButton.TextColor;
        }

        #endregion

        #region Private

        /// <summary>
        /// Save the new color
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
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
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        #endregion
    }
}
