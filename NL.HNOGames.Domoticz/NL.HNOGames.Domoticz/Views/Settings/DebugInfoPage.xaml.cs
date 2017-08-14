using System;
using Plugin.Share;
using NL.HNOGames.Domoticz.Resources;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    public partial class DebugInfoPage
    {
        public DebugInfoPage()
        {
            InitializeComponent();

            txtInfo.Text = App.AppSettings.DebugInfo;
            swEnableDebugging.IsToggled = App.AppSettings.EnableDebugging;
            swEnableDebugging.Toggled += (sender, args) =>
            {
                App.AppSettings.EnableDebugging = swEnableDebugging.IsToggled;
                txtInfo.Text = App.AppSettings.EnableDebugging
                    ? "Debugging started, we only save the debug information for 1 app sessions!!"
                    : "";
                swEnableJSONDebugging.IsEnabled = App.AppSettings.EnableDebugging;
            };

            swEnableJSONDebugging.IsToggled = App.AppSettings.EnableJSONDebugging;
            swEnableJSONDebugging.Toggled += (sender, args) =>
            {
                App.AppSettings.EnableJSONDebugging = swEnableJSONDebugging.IsToggled;
            };
        }

        private async void ToolbarItem_Activated(object sender, EventArgs e)
        {
            //App.AppSettings.EnableDebugging, AppResources.category_debug + " Domoticz"
            var oMessage = new Plugin.Share.Abstractions.ShareMessage
            {
                Text = App.AppSettings.DebugInfo,
                Title = AppResources.category_debug + "- Domoticz"
            };
            await CrossShare.Current.Share(oMessage);
        }
    }
}