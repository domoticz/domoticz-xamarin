namespace NL.HNOGames.Domoticz.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            LoadApplication(new NL.HNOGames.Domoticz.App());

            OxyPlot.Xamarin.Forms.Platform.UWP.PlotViewRenderer.Init();
        }
    }
}