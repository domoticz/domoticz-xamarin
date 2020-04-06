using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.Controls
{
    /// <summary>
    /// Defines the <see cref="MultiLineLabel" />
    /// </summary>
    public class MultiLineLabel : Label
    {
        #region Variables

        /// <summary>
        /// Defines the _defaultLineSetting
        /// </summary>
        private static int _defaultLineSetting = -1;

        /// <summary>
        /// Defines the LinesProperty
        /// </summary>
        public static readonly BindableProperty LinesProperty = BindableProperty.Create(nameof(Lines), typeof(int), typeof(MultiLineLabel), _defaultLineSetting);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Lines
        /// </summary>
        public int Lines
        {
            get { return (int)GetValue(LinesProperty); }
            set { SetValue(LinesProperty, value); }
        }

        #endregion
    }
}
