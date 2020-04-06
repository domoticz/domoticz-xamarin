using System;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.Controls
{
    /// <summary>
    /// Defines the <see cref="ExtendedSlider" />
    /// </summary>
    public class ExtendedSlider : Slider
    {
        #region Public

        /// <summary>
        /// The OnTouchDown
        /// </summary>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        public void OnTouchDown(EventArgs e)
        {
            TouchDown?.Invoke(this, e);
        }

        /// <summary>
        /// The OnTouchUpInside
        /// </summary>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        public void OnTouchUpInside(EventArgs e)
        {
            TouchUpInside?.Invoke(this, e);
        }

        /// <summary>
        /// The OnTouchUpOutside
        /// </summary>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        public void OnTouchUpOutside(EventArgs e)
        {
            TouchUpOutside?.Invoke(this, e);
        }

        #endregion

        /// <summary>
        /// Defines the TouchDown
        /// </summary>
        public event EventHandler TouchDown;

        /// <summary>
        /// Defines the TouchUpInside
        /// </summary>
        public event EventHandler TouchUpInside;

        /// <summary>
        /// Defines the TouchUpOutside
        /// </summary>
        public event EventHandler TouchUpOutside;
    }
}
