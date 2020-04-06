using System;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.Controls
{
    /// <summary>
    /// Custom view cell
    /// </summary>
    public class CustomViewCell : ViewCell
    {
        #region Variables

        /// <summary>
        /// Row height property
        /// </summary>
        public static BindableProperty RowHeightProperty = BindableProperty.Create(nameof(RowHeight), typeof(int), typeof(int), 0, propertyChanged: UpdateHeight);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the RowHeight
        /// Row height
        /// </summary>
        public int RowHeight
        {
            get { return (int)GetValue(RowHeightProperty); }
            set { SetValue(RowHeightProperty, value); }
        }

        #endregion

        #region Private

        /// <summary>
        /// Update heights
        /// </summary>
        /// <param name="bindable">The bindable<see cref="BindableObject"/></param>
        /// <param name="oldHeight">The oldHeight<see cref="object"/></param>
        /// <param name="newHeight">The newHeight<see cref="object"/></param>
        private static void UpdateHeight(BindableObject bindable, object oldHeight, object newHeight)
        {
            try
            {
                ((ViewCell)bindable).Height = (int)newHeight;
            }
            catch (Exception)
            { // Just a handler to make sure we dont crash
            }
        }

        #endregion
    }
}
