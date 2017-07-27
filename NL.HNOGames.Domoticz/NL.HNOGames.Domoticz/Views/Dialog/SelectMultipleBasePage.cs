using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using System.Reflection;
using NL.HNOGames.Domoticz.Resources;

namespace NL.HNOGames.Domoticz.Views.Dialog
{
    public class SelectMultipleBasePage<T> : ContentPage
    {
        Command cmFinish = null;

        public class WrappedSelection<T> : INotifyPropertyChanged
        {
            public T item;
            bool isSelected = false;

            public T Item
            {
                get
                {
                    return item;
                }
                set
                {
                    item = value;
                    isSelected = (bool)item.GetType().GetRuntimeProperty("IsSelected").GetValue(item);
                }
            }

            public bool IsSelected
            {
                get
                {
                    return isSelected;
                }
                set
                {
                    if (isSelected != value)
                    {
                        isSelected = value;
                        PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
                        Item.GetType().GetRuntimeProperty("IsSelected").SetValue(Item, value);
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged = delegate { };

        }

        public class WrappedItemSelectionTemplate : ViewCell
        {
            public WrappedItemSelectionTemplate() : base()
            {
                Label name = new Label();
                name.SetBinding(Label.TextProperty, new Binding("Item.Name"));
                name.FontSize = 16;
                name.Margin = new Thickness(10);
                name.VerticalOptions = LayoutOptions.Center;
                name.Style = (Style)Application.Current.Resources["DetailType"];

                Switch mainSwitch = new Switch();
                mainSwitch.SetBinding(Switch.IsToggledProperty, new Binding("IsSelected"));

                RelativeLayout layout = new RelativeLayout();
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

        public List<WrappedSelection<T>> WrappedItems = new List<WrappedSelection<T>>();

        public SelectMultipleBasePage(List<T> items, Command finish = null)
        {
            cmFinish = finish;
            WrappedItems = items.Select(item => new WrappedSelection<T>() { Item = item }).ToList();

            this.Style = (Style)Application.Current.Resources["ContentPageType"];
            if(App.AppSettings.DarkTheme)
                this.BackgroundColor = Color.FromHex("#263238");

            StackLayout ly = new StackLayout();
            ly.Orientation = StackOrientation.Vertical;
            ly.Padding = new Thickness(10);
            if (App.AppSettings.DarkTheme)
                ly.BackgroundColor = Color.FromHex("#263238");

            ListView mainList = new ListView()
            {
                ItemsSource = WrappedItems,
                ItemTemplate = new DataTemplate(typeof(WrappedItemSelectionTemplate)),
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

            Button oSave = new Button();
            oSave.Text = AppResources.ok;
            oSave.HorizontalOptions = LayoutOptions.CenterAndExpand;
            oSave.Margin = new Thickness(10);
            oSave.Clicked += (o, e) =>
            {
                if (cmFinish != null)
                    cmFinish.Execute(null);
                Navigation.PopAsync();
            };

            ly.Children.Add(mainList);
            ly.Children.Add(oSave);
            Content = ly;
            if (Device.RuntimePlatform == Device.WinPhone)
            {
                mainList.RowHeight = 40;
                ToolbarItems.Add(new ToolbarItem(AppResources.filterOn_all, "check.png", SelectAll, ToolbarItemOrder.Primary));
            }
            else
            {
                mainList.RowHeight = 60;
                ToolbarItems.Add(new ToolbarItem(AppResources.filterOn_all, null, SelectAll, ToolbarItemOrder.Primary));
            }
        }

        void SelectAll()
        {
            foreach (var wi in WrappedItems)
            {
                wi.IsSelected = true;
            }
        }

        void SelectNone()
        {
            foreach (var wi in WrappedItems)
            {
                wi.IsSelected = false;
            }
        }

        public List<T> GetSelection()
        {
            return WrappedItems.Where(item => item.IsSelected).Select(wrappedItem => wrappedItem.Item).ToList();
        }

        public List<T> GetAllItems()
        {
            return WrappedItems.Select(wrappedItem => wrappedItem.Item).ToList();
        }
    }
}
