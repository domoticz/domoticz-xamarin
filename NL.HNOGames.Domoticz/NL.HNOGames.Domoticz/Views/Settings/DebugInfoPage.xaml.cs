using System;

using NL.HNOGames.Domoticz.ViewModels;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

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
    }
}