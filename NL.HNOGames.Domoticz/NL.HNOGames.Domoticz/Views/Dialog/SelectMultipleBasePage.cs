using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using System.Reflection;
using NL.HNOGames.Domoticz.Resources;
using NL.HNOGames.Domoticz.Controls;

namespace NL.HNOGames.Domoticz.Views.Dialog
{
    internal interface IWrappedSelection<T>
    {
        bool IsSelected { get; set; }
        T Item { get; set; }

        event PropertyChangedEventHandler PropertyChanged;
    }

    public class SelectMultipleBasePage<T> : ContentPage
    {
#pragma warning disable 693
        private class WrappedSelection<T> : INotifyPropertyChanged, IWrappedSelection<T>
#pragma warning restore 693
        {
            private T _item;
            private bool _isSelected;

            public T Item
            {
                get => _item;
                set
                {
                    _item = value;
                    _isSelected = (bool) _item.GetType().GetRuntimeProperty("IsSelected").GetValue(_item);
                }
            }

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

            public event PropertyChangedEventHandler PropertyChanged = delegate { };
        }

        private class WrappedItemSelectionTemplate : ViewCell
        {
            public WrappedItemSelectionTemplate()
            {
                var name = new Label();
                name.SetBinding(Label.TextProperty, new Binding("Item.Name"));
                name.FontSize = 16;
                name.Margin = new Thickness(10);
                name.VerticalOptions = LayoutOptions.Center;
                name.Style = (Style) Application.Current.Resources["DetailType"];

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
        }

        private readonly List<WrappedSelection<T>> _wrappedItems;

        public SelectMultipleBasePage(IEnumerable<T> items, Command finish = null)
        {
            var cmFinish = finish;
            _wrappedItems = items.Select(item => new WrappedSelection<T>() {Item = item}).ToList();

            Style = (Style) Application.Current.Resources["ContentPageType"];
            if (App.AppSettings.DarkTheme)
                BackgroundColor = Color.FromHex("#263238");

            var ly = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(10)
            };
            if (App.AppSettings.DarkTheme)
                ly.BackgroundColor = Color.FromHex("#263238");

            var mainList = new ListView()
            {
                ItemsSource = _wrappedItems,
                ItemTemplate = new DataTemplate(typeof(WrappedItemSelectionTemplate)),
            };

            if (App.AppSettings.DarkTheme)
                mainList.BackgroundColor = Color.FromHex("#263238");
            mainList.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem == null) return;
                var o = (WrappedSelection<T>) e.SelectedItem;
                o.IsSelected = !o.IsSelected;
                ((ListView) sender).SelectedItem = null; //de-select
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
            if (Device.RuntimePlatform == Device.WinPhone)
            {
                mainList.RowHeight = 40;
                ToolbarItems.Add(new ToolbarItem(AppResources.filterOn_all, "check.png", SelectAll,
                    ToolbarItemOrder.Primary));
            }
            else
            {
                mainList.RowHeight = 60;
                ToolbarItems.Add(new ToolbarItem(AppResources.filterOn_all, null, SelectAll, ToolbarItemOrder.Primary));
            }
        }

        private void SelectAll()
        {
            foreach (var wi in _wrappedItems)
            {
                wi.IsSelected = true;
            }
        }

        public List<T> GetAllItems()
        {
            return _wrappedItems.Select(wrappedItem => wrappedItem.Item).ToList();
        }
    }
}