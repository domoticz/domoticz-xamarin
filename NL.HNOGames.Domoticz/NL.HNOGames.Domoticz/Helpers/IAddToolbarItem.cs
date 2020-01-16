using System;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.Helpers
{
    /// <summary>
    /// Defines the <see cref="IAddToolbarItem" />
    /// </summary>
    public interface IAddToolbarItem
    {
        #region Properties

        /// <summary>
        /// Gets the CellBackgroundColor
        /// </summary>
        Color CellBackgroundColor { get; }

        /// <summary>
        /// Gets the CellTextColor
        /// </summary>
        Color CellTextColor { get; }

        /// <summary>
        /// Gets the MenuBackgroundColor
        /// </summary>
        Color MenuBackgroundColor { get; }

        /// <summary>
        /// Gets the RowHeight
        /// </summary>
        float RowHeight { get; }

        /// <summary>
        /// Gets the ShadowColor
        /// </summary>
        Color ShadowColor { get; }

        /// <summary>
        /// Gets the ShadowOpacity
        /// </summary>
        float ShadowOpacity { get; }

        /// <summary>
        /// Gets the ShadowRadius
        /// </summary>
        float ShadowRadius { get; }

        /// <summary>
        /// Gets the ShadowOffsetDimension
        /// </summary>
        float ShadowOffsetDimension { get; }

        /// <summary>
        /// Gets the TableWidth
        /// </summary>
        float TableWidth { get; }

        #endregion

        /// <summary>
        /// Defines the ToolbarItemAdded
        /// </summary>
        event EventHandler ToolbarItemAdded;
    }
}
