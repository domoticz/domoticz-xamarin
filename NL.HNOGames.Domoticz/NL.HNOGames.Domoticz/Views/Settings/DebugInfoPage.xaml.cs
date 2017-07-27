using System;

using NL.HNOGames.Domoticz.ViewModels;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Plugin.Share;
using NL.HNOGames.Domoticz.Resources;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    public partial class DebugInfoPage : ContentPage
    {
        public DebugInfoPage()
        {
            InitializeComponent();

            txtInfo.Text = App.AppSettings.DebugInfo;

            swEnableDebugging.IsToggled = App.AppSettings.EnableDebugging;
            swEnableDebugging.Toggled += (sender, args) =>
            {
                App.AppSettings.EnableDebugging = swEnableDebugging.IsToggled;
                if (App.AppSettings.EnableDebugging)
                    txtInfo.Text = "Debugging started, we only save the debug information for 1 app sessions!!";
                else
                    txtInfo.Text = "";
                swEnableJSONDebugging.IsEnabled = App.AppSettings.EnableDebugging;
            };

            swEnableJSONDebugging.IsToggled = App.AppSettings.EnableJSONDebugging;
            swEnableJSONDebugging.Toggled += (sender, args) =>
            {
                App.AppSettings.EnableJSONDebugging = swEnableJSONDebugging.IsToggled;
            };
        }

        private async Task ToolbarItem_Activated(object sender, EventArgs e)
        {
            //App.AppSettings.EnableDebugging, AppResources.category_debug + " Domoticz"
            Plugin.Share.Abstractions.ShareMessage oMessage = new Plugin.Share.Abstractions.ShareMessage();
            oMessage.Text = App.AppSettings.DebugInfo;
            oMessage.Title = AppResources.category_debug + "- Domoticz";
            await CrossShare.Current.Share(oMessage);
        }
    }
}