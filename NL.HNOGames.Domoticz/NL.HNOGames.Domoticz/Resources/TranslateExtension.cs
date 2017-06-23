using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Resources;
using System.Reflection;

namespace NL.HNOGames.Domoticz.Resources
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        public string Text { get; set; }
        public ResourceManager resManager;

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return null;
            if(resManager == null)
                resManager = new ResourceManager(typeof(AppResources));
            return resManager.GetString(Text, CultureInfo.CurrentCulture);
        }
    }
}
