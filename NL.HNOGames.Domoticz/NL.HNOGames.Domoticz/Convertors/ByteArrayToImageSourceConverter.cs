using System;
using System.IO;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.Convertors
{
    /// <summary>
    /// Defines the <see cref="ByteArrayToImageSourceConverter" />
    /// </summary>
    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        #region Public

        /// <summary>
        /// The Convert
        /// </summary>
        /// <param name="value">The value<see cref="object"/></param>
        /// <param name="targetType">The targetType<see cref="Type"/></param>
        /// <param name="parameter">The parameter<see cref="object"/></param>
        /// <param name="culture">The culture<see cref="System.Globalization.CultureInfo"/></param>
        /// <returns>The <see cref="object"/></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            var bArray = (byte[])value;

            var imgsrc = ImageSource.FromStream(() =>
            {
                var ms = new MemoryStream(bArray)
                {
                    Position = 0
                };
                return ms;
            });

            return imgsrc;
        }

        /// <summary>
        /// The ConvertBack
        /// </summary>
        /// <param name="value">The value<see cref="object"/></param>
        /// <param name="targetType">The targetType<see cref="Type"/></param>
        /// <param name="parameter">The parameter<see cref="object"/></param>
        /// <param name="culture">The culture<see cref="System.Globalization.CultureInfo"/></param>
        /// <returns>The <see cref="object"/></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
