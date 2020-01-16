using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using System;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.Controls
{
    /// <summary>
    /// Defines the <see cref="TintedCachedImage" />
    /// </summary>
    public class TintedCachedImage : CachedImage
    {
        #region Variables

        /// <summary>
        /// Defines the TintColorProperty
        /// </summary>
        public static BindableProperty TintColorProperty = BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(TintedCachedImage), Color.Transparent, propertyChanged: UpdateColor);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the TintColor
        /// </summary>
        public Color TintColor
        {
            get { return (Color)GetValue(TintColorProperty); }
            set { SetValue(TintColorProperty, value); }
        }

        #endregion

        #region Private

        /// <summary>
        /// The UpdateColor
        /// </summary>
        /// <param name="bindable">The bindable<see cref="BindableObject"/></param>
        /// <param name="oldColor">The oldColor<see cref="object"/></param>
        /// <param name="newColor">The newColor<see cref="object"/></param>
        private static void UpdateColor(BindableObject bindable, object oldColor, object newColor)
        {
            try
            {
                var oldcolor = (Color)oldColor;
                var newcolor = (Color)newColor;

                if (!oldcolor.Equals(newcolor))
                {
                    var view = (TintedCachedImage)bindable;
                    var transformations = new System.Collections.Generic.List<ITransformation>() {
                    new TintTransformation((int)(newcolor.R * 255), (int)(newcolor.G * 255), (int)(newcolor.B * 255), (int)(newcolor.A * 255)) {
                        EnableSolidColor = true
                    }
                };
                    view.Transformations = transformations;
                }
            }
            catch (Exception) { }
        }

        #endregion
    }
}
