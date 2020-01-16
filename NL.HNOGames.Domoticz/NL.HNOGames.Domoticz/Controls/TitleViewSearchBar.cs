using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace NL.HNOGames.Domoticz.Controls
{
    /// <summary>
    /// Defines the <see cref="TitleViewSearchBar" />
    /// </summary>
    [Preserve(AllMembers = true)]
    public class TitleViewSearchBar : SearchBar
    {
        #region Variables

        /// <summary>
        /// Defines the CancelButtonTextProperty
        /// </summary>
        public static readonly BindableProperty CancelButtonTextProperty = BindableProperty.Create(nameof(CancelButtonText), typeof(string), typeof(TitleViewSearchBar), null);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the CancelButtonText
        /// </summary>
        public string CancelButtonText { get => (string)GetValue(CancelButtonTextProperty); set => SetValue(CancelButtonTextProperty, value); }

        #endregion

        #region Public

        /// <summary>
        /// The SendCancelled
        /// </summary>
        public void SendCancelled()
        {
            Cancelled?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        /// <summary>
        /// Defines the Cancelled
        /// </summary>
        public event EventHandler Cancelled;
    }
}
