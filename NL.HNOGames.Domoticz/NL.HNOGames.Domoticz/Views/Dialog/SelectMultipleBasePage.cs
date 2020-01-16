using NL.HNOGames.Domoticz.Controls;
using NL.HNOGames.Domoticz.Resources;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.Views.Dialog
{
    /// <summary>
    /// Defines the <see cref="IWrappedSelection{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface IWrappedSelection<T>
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether IsSelected
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets the Item
        /// </summary>
        T Item { get; set; }

        #endregion

        /// <summary>
        /// Defines the PropertyChanged
        /// </summary>
        event PropertyChangedEventHandler PropertyChanged;
    }

    /// <summary>
    /// Defines the <see cref="SelectMultipleBasePage{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SelectMultipleBasePage<T> : ContentPage
    {
        #region Variables

        /// <summary>
        /// Defines the _wrappedItems
        /// </summary>
        private readonly List<WrappedSelection<T>> _wrappedItems;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectMultipleBasePage{T}"/> class.
        /// </summary>
        /// <param name="items">The items<see cref="IEnumerable{T}"/></param>
        /// <param name="finish">The finish<see cref="Command"/></param>
        public SelectMultipleBasePage(IEnumerable<T> items, Command finish = null)
        {
            var cmFinish = finish;
            _wrappedItems = items.Select(item => new WrappedSelection<T>() { Item = item }).ToList();

            Style = (Style)Application.Current.Resources["ContentPageType"];
            if (App.AppSettings.DarkTheme)
                BackgroundColor = Color.FromHex("#263238");

            var ly = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(10)
            };

            switch (Device.Idiom)
            {
                case TargetIdiom.Tablet:
                    ly.Padding = new Thickness(150, 30, 150, 30);
                    break;
                case TargetIdiom.Desktop:
                    ly.Padding = new Thickness(150, 30, 150, 30);
                    break;
                default:
                    ly.Padding = new Thickness(20, 20, 20, 20);
                    break;
            }

            if (App.AppSettings.DarkTheme)
                ly.BackgroundColor = Color.FromHex("#263238");

            var mainList = new ListView()
            {
                ItemsSource = _wrappedItems,
                ItemTemplate = new DataTemplate(typeof(WrappedItemSelectionTemplate)),
                SeparatorVisibility = SeparatorVisibility.None
            };

            if (App.AppSettings.DarkTheme)
                mainList.BackgroundColor = Color.FromHex("#263238");
            mainList.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem == null) return;
                var o = (WrappedSelection<T>)e.SelectedItem;
                o.IsSelected = !o.IsSelected;
                ((ListView)sender).SelectedItem = null; //de-select
         };

            var oSave = new ExtendedButton
            {
                Text = AppResources.ok,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(10)
            };
            oSave.Padding = new Thickness(20, 0, 20, 0);
            oSave.Clicked += (o, e) =>
            {
                cmFinish?.Execute(null);
                Navigation.PopAsync();
            };

            ly.Children.Add(mainList);
            ly.Children.Add(oSave);
            Content = ly;

            mainList.RowHeight = 60;
            ToolbarItems.Add(new ToolbarItem(AppResources.filterOn_all, null, SelectAll, ToolbarItemOrder.Primary));
        }

        #endregion

        #region Public

        /// <summary>
        /// The GetAllItems
        /// </summary>
        /// <returns>The <see cref="List{T}"/></returns>
        public List<T> GetAllItems()
        {
            return _wrappedItems.Select(wrappedItem => wrappedItem.Item).ToList();
        }

        #endregion

        #region Private

        /// <summary>
        /// The SelectAll
        /// </summary>
        private void SelectAll()
        {
            foreach (var wi in _wrappedItems)
            {
                wi.IsSelected = true;
            }
        }

        #endregion

#pragma warning disable 693
        /// <summary>
        /// Defines the <see cref="WrappedSelection{T}" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class WrappedSelection<T> : INotifyPropertyChanged, IWrappedSelection<T>
        {
            #region Variables

            /// <summary>
            /// Defines the _item
            /// </summary>
            private T _item;

            /// <summary>
            /// Defines the _isSelected
            /// </summary>
            private bool _isSelected;

            #endregion

            #region Properties

            /// <summary>
            /// Gets or sets the Item
            /// </summary>
            public T Item
            {
                get => _item;
                set
                {
                    _item = value;
                    _isSelected = (bool)_item.GetType().GetRuntimeProperty("IsSelected").GetValue(_item);
                }
            }

            /// <summary>
            /// Gets or sets a value indicating whether IsSelected
            /// </summary>
            public bool IsSelected
            {
                get => _isSelected;
                set
                {
                    if (_isSelected == value) return;
                    _isSelected = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
                    Item.GetType().GetRuntimeProperty("IsSelected").SetValue(Item, value);
                }
            }

            #endregion

            /// <summary>
            /// Defines the PropertyChanged
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged = delegate { };
        }

        /// <summary>
        /// Defines the <see cref="WrappedItemSelectionTemplate" />
        /// </summary>
        private class WrappedItemSelectionTemplate : ViewCell
        {
            #region Constructor & Destructor

            /// <summary>
            /// Initializes a new instance of the <see cref="WrappedItemSelectionTemplate"/> class.
            /// </summary>
            public WrappedItemSelectionTemplate()
            {
                var name = new Label();
                name.SetBinding(Label.TextProperty, new Binding("Item.Name"));
                name.FontSize = 16;
                name.Margin = new Thickness(10);
                name.VerticalOptions = LayoutOptions.Center;
                name.Style = (Style)Application.Current.Resources["DetailType"];

                var mainSwitch = new Switch();
                mainSwitch.SetBinding(Switch.IsToggledProperty, new Binding("IsSelected"));

                var layout = new RelativeLayout();
                layout.Children.Add(name,
                    Constraint.Constant(5),
                    Constraint.Constant(5),
                    Constraint.RelativeToParent(p => p.Width - 60),
                    Constraint.RelativeToParent(p => p.Height - 10)
                );
                layout.Children.Add(mainSwitch,
                    Constraint.RelativeToParent(p => p.Width - 55),
                    Constraint.Constant(5),
                    Constraint.Constant(50),
                    Constraint.RelativeToParent(p => p.Height - 10)
                );
                View = layout;
            }

            #endregion
        }
    }
}
