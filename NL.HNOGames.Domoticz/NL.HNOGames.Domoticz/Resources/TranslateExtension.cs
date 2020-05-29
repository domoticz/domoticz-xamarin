using Plugin.Multilingual;
using System;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NL.HNOGames.Domoticz.Resources
{
    /// <summary>
    /// Defines the <see cref="TranslateExtension" />
    /// </summary>
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
#region Constants

/// <summary>
/// Defines the ResourceId
/// </summary>
const string ResourceId = "NL.HNOGames.Domoticz.Resources.AppResources";

#endregion

#region Variables

/// <summary>
/// Defines the resmgr
/// </summary>
static readonly Lazy<ResourceManager> resmgr = new Lazy<ResourceManager>(() => new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly));

#endregion

#region Properties

/// <summary>
/// Gets or sets the Text
/// </summary>
public string Text { get; set; }

#endregion

#region Public

/// <summary>
/// The ProvideValue
/// </summary>
/// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/></param>
/// <returns>The <see cref="object"/></returns>
public object ProvideValue(IServiceProvider serviceProvider)
{
    if (Text == null)
return "";

    var ci = CrossMultilingual.Current.CurrentCultureInfo;
    var translation = resmgr.Value.GetString(Text, ci);
    if (translation == null)
    {
#if DEBUG
throw new ArgumentException(
    string.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
    "Text");
#else
				translation = Text; // returns the key, which GETS DISPLAYED TO THE USER
#endif
    }
    return translation;
}

#endregion
    }
}
