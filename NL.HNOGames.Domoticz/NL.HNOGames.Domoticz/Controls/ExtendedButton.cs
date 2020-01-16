using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.Controls
{
    /// <summary>
    /// Defines the <see cref="ExtendedButton" />
    /// </summary>
    public class ExtendedButton : Button
    {
        #region Variables

        /// <summary>
        /// Defines the PaddingProperty
        /// </summary>
        public static BindableProperty PaddingProperty = BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(ExtendedButton), default(Thickness), defaultBindingMode: BindingMode.OneWay);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Padding
        /// </summary>
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        #endregion
    }
}
