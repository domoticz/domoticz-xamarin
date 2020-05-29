using Acr.UserDialogs;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    /// <summary>
    /// Defines the <see cref="BeaconConfigPage" />
    /// </summary>
    public partial class BeaconConfigPage
    {
        #region Variables

        /// <summary>
        /// The callback method
        /// </summary>
        private readonly CallbackDelegate _Callback;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BeaconConfigPage"/> class.
        /// </summary>
        public BeaconConfigPage(CallbackDelegate callback)
        {
            _Callback = callback;
            InitializeComponent();
        }

        #endregion

        #region Delegates

        /// <summary>
        /// Delegate for the callback method
        /// </summary>
        public delegate void CallbackDelegate(BeaconModel beacon);

        #endregion

        #region Private

        /// <summary>
        /// save the beacon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUUID.Text))
                UserDialogs.Instance.Toast(AppResources.txt_beacon_UUID_error);
            bool isValid = Guid.TryParse(txtUUID.Text, out Guid uuID);
            if (!isValid)
                UserDialogs.Instance.Toast(AppResources.txt_beacon_UUID_error2);
            else
            {
                if (string.IsNullOrEmpty(txtMajor.Text))
                    txtMajor.Text = "0";
                if (string.IsNullOrEmpty(txtMinor.Text))
                    txtMinor.Text = "0";
                if (ushort.TryParse(txtMinor.Text, out ushort minor))
                {
                    if (ushort.TryParse(txtMajor.Text, out ushort major))
                    {
                        var name = await GetNameAsync();
                        if (!string.IsNullOrEmpty(name))
                        {
                            var beacon = new BeaconModel()
                            {
                                UUID = uuID,
                                Id = uuID.ToString(),
                                Name = name,
                                Enabled = true,
                                Minor = minor,
                                Major = major
                            };
                            _Callback.Invoke(beacon);
                            await Navigation.PopAsync();
                        }
                    }
                    await Navigation.PopAsync();
                }
            }
        }

        private async Task<string> GetNameAsync()
        {
            var result = await UserDialogs.Instance.PromptAsync(new PromptConfig() { InputType = InputType.Default, Title = AppResources.beacon });
            if (result.Ok)
                return result.Value;
            return null;
        }

        #endregion
    }
}
