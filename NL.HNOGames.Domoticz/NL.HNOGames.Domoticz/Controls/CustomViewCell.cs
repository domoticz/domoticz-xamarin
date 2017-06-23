using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.Controls
{
    public class CustomViewCell : ViewCell
    {
        public static BindableProperty RowHeightProperty = BindableProperty.Create(nameof(RowHeight), typeof(int), typeof(int), 0, propertyChanged: UpdateHeight);

        public int RowHeight
        {
            get { return (int)GetValue(RowHeightProperty); }
            set { SetValue(RowHeightProperty, value); }
        }

        private static void UpdateHeight(BindableObject bindable, object oldHeight, object newHeight)
        {
            try
            {
                ((ViewCell)bindable).Height = (int)newHeight;
            }
            catch (Exception) { }
        }
    }
}
