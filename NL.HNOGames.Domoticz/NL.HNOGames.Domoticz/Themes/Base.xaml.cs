using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NL.HNOGames.Domoticz.Themes
{
    /// <summary>
    /// Defines the <see cref="Base" />
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Base : ResourceDictionary
    {
        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Base"/> class.
        /// </summary>
        public Base()
        {
            InitializeComponent();
        }

        #endregion
    }
}
