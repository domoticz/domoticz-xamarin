using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace NL.HNOGames.Domoticz.Controls
{
   /// <summary>
   /// Title view containing a search bar
   /// </summary>
   [Preserve(AllMembers = true)]
   [XamlCompilation(XamlCompilationOptions.Compile)]
   public partial class SearchTitleView : ContentView
   {
      #region Variables

      /// <summary>
      /// Bindable property for the <see cref="Title"/> property
      /// </summary>
      public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(SearchTitleView), string.Empty,
         propertyChanged: OnTitlePropertyChanged);

      /// <summary>
      /// Bindable property for the <see cref="SearchImage"/> property
      /// </summary>
      public static readonly BindableProperty SearchImageProperty = BindableProperty.Create(nameof(SearchImage), typeof(ImageSource), typeof(SearchTitleView), null,
         propertyChanged: OnSearchImageChanged);

      /// <summary>
      /// Binding property for <see cref="PlaceHolderText"/> property
      /// </summary>
      public static readonly BindableProperty PlaceHolderTextProperty = BindableProperty.Create(nameof(PlaceHolderText), typeof(string), typeof(SearchTitleView), string.Empty,
         propertyChanged: OnPlaceHolderTextChanged);

      /// <summary>
      /// Binding property for <see cref="SearchText"/> property
      /// </summary>
      public static readonly BindableProperty SearchTextProperty = BindableProperty.Create(nameof(SearchText), typeof(string), typeof(SearchTitleView), null, BindingMode.TwoWay,
         propertyChanged: OnSearchTextChanged);

      /// <summary>
      /// Binding property for <see cref="SearchCommand"/> property
      /// </summary>
      public static readonly BindableProperty SearchCommandProperty = BindableProperty.Create(nameof(SearchCommand), typeof(ICommand), typeof(SearchTitleView), null,
         propertyChanged: OnSearchCommandChanged);

      /// <summary>
      /// Bindable property for the <see cref="ClearCommand"/> property
      /// </summary>
      public static readonly BindableProperty ClearCommandProperty = BindableProperty.Create(nameof(ClearCommand), typeof(ICommand), typeof(SearchTitleView), null,
         propertyChanged: OnClearCommandChanged);

      /// <summary>
      /// Bindable property for the <see cref="TitleStyle"/> property
      /// </summary>
      public static readonly BindableProperty TitleStyleProperty = BindableProperty.Create(nameof(TitleStyle), typeof(Style), typeof(SearchTitleView), null,
         propertyChanged: OnTitleStylePropertyChanged);

      /// <summary>
      /// Bindable property for the <see cref="SearchImageStyle"/> property
      /// </summary>
      public static readonly BindableProperty SearchImageStyleProperty = BindableProperty.Create(nameof(SearchImageStyle), typeof(Style), typeof(SearchTitleView), null,
         propertyChanged: OnSearchImageStyleChanged);

      /// <summary>
      /// Bindable property for the <see cref="SearchBarStyle"/> property
      /// </summary>
      public static readonly BindableProperty SearchBarStyleProperty = BindableProperty.Create(nameof(SearchBarStyle), typeof(Style), typeof(SearchTitleView), null,
         propertyChanged: OnSearchBarStyleChanged);

      /// <summary>
      /// Bindable property for the <see cref="IsSearchIconVisible"/> property
      /// </summary>
      public static readonly BindableProperty IsSearchIconVisibleProperty = BindableProperty.Create(nameof(IsSearchIconVisible), typeof(bool?), typeof(SearchTitleView), null,
         propertyChanged: IsSearchIconVisibleChanged);

      #endregion

      #region Constructor & Destructor

      /// <summary>
      /// Default constructor
      /// </summary>
      public SearchTitleView()
      {
         InitializeComponent();
         searchIcon.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(OnSearchIconTapped) });
         searchBar.TextChanged += OnSearchTextChanged;
         searchBar.Cancelled += (s, e) => OnCancelled();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Title
      /// </summary>
      public string Title
      {
         get => (string)GetValue(TitleProperty);
         set => SetValue(TitleProperty, value);
      }

      /// <summary>
      /// Search image
      /// </summary>
      public ImageSource SearchImage
      {
         get => (ImageSource)GetValue(SearchImageProperty);
         set => SetValue(SearchImageProperty, value);
      }

      /// <summary>
      /// Get or set the search bar placeholder text
      /// </summary>
      public string PlaceHolderText
      {
         get => (string)GetValue(PlaceHolderTextProperty);
         set => SetValue(PlaceHolderTextProperty, value);
      }

      /// <summary>
      /// Get or set the search bar text
      /// </summary>
      public string SearchText
      {
         get => (string)GetValue(SearchTextProperty);
         set => SetValue(SearchTextProperty, value);
      }

      /// <summary>
      /// Get or set the search command
      /// </summary>
      public ICommand SearchCommand
      {
         get => (ICommand)GetValue(SearchCommandProperty);
         set => SetValue(SearchCommandProperty, value);
      }

      /// <summary>
      /// Get or set the clear command
      /// </summary>
      public ICommand ClearCommand
      {
         get => (ICommand)GetValue(ClearCommandProperty);
         set => SetValue(ClearCommandProperty, value);
      }

      /// <summary>
      /// Get or set the style of the <see cref="Title"/>
      /// </summary>
      public Style TitleStyle
      {
         get => (Style)GetValue(TitleStyleProperty);
         set => SetValue(TitleStyleProperty, value);
      }

      /// <summary>
      /// Get or set the style of the <see cref="SearchImage"/>
      /// </summary>
      public Style SearchImageStyle
      {
         get => (Style)GetValue(SearchImageStyleProperty);
         set => SetValue(SearchImageStyleProperty, value);
      }

      /// <summary>
      /// Get or set the style of the search bar
      /// </summary>
      public Style SearchBarStyle
      {
         get => (Style)GetValue(SearchBarStyleProperty);
         set => SetValue(SearchBarStyleProperty, value);
      }

      /// <summary>
      /// Get or set the visibility of the search icon
      /// </summary>
      public bool? IsSearchIconVisible
      {
         get => (bool?)GetValue(IsSearchIconVisibleProperty) ?? false;
         set => SetValue(IsSearchIconVisibleProperty, value);
      }

      #endregion

      #region Protected

      /// <summary>
      /// This method is called when the size of the element is set during a layout cycle.
      /// This method is called directly before the Xamarin.Forms.VisualElement.SizeChanged
      /// event is emitted. Implement this method to add class handling for this event.
      /// </summary>
      /// <param name="width">The new width of the element.</param>
      /// <param name="height">The new height of the element.</param>
      protected override void OnSizeAllocated(double width, double height)
      {
         base.OnSizeAllocated(width, height);

         // Manually layout the search bar as we want it to follow directly the size of the title view
         searchBar.Layout(new Rectangle(0, 0, width, height));
      }

      #endregion

      #region Private

      /// <summary>
      /// Handle the change of the <see cref="Title"/> property
      /// </summary>
      /// <param name="bindable">The bindable object</param>
      /// <param name="o">The old value</param>
      /// <param name="n">The new value</param>
      private static void OnTitlePropertyChanged(BindableObject bindable, object o, object n)
      {
         if (bindable is SearchTitleView view && view?.title != null && n is string value)
         {
            view.title.Text = value;
            view.title.Style = view.TitleStyle;
         }
      }

      /// <summary>
      /// Handle the change of the <see cref="SearchImage"/> property
      /// </summary>
      /// <param name="bindable">The bindable object</param>
      /// <param name="o">The old value</param>
      /// <param name="n">The new value</param>
      private static void OnSearchImageChanged(BindableObject bindable, object o, object n)
      {
         if (bindable is SearchTitleView view && view?.title != null && n is ImageSource value)
         {
            view.searchIcon.Source = value;
            view.searchIcon.Style = view.SearchImageStyle;
         }
      }

      /// <summary>
      /// Handle the change of the <see cref="PlaceHolderText"/> property
      /// </summary>
      /// <param name="bindable">The bindable object</param>
      /// <param name="o">The old value</param>
      /// <param name="n">The new value</param>
      private static void OnPlaceHolderTextChanged(BindableObject bindable, object o, object n)
      {
         if (bindable is SearchTitleView view && view.searchBar != null)
            view.searchBar.Placeholder = n as string;
      }

      /// <summary>
      /// Handle the change of the <see cref="SearchText"/> property
      /// </summary>
      /// <param name="bindable">The bindable object</param>
      /// <param name="o">The old value</param>
      /// <param name="n">The new value</param>
      private static void OnSearchTextChanged(BindableObject bindable, object o, object n)
      {
         if (n == null)
            return;
         if (bindable is SearchTitleView view && view.searchBar != null)
         {
            if (!string.Equals(view.searchBar.Text, n as string, StringComparison.Ordinal))
            {
               view.searchBar.Text = n as string;
               if (!view.searchBar.IsVisible && !string.IsNullOrEmpty(view.searchBar.Text))
                  view.OnSearchIconTapped();
            }
         }
      }

      /// <summary>
      /// Handle the change of the <see cref="SearchCommand"/> property
      /// </summary>
      /// <param name="bindable">The bindable object</param>
      /// <param name="o">The old value</param>
      /// <param name="n">The new value</param>
      private static void OnSearchCommandChanged(BindableObject bindable, object o, object n)
      {
         if (bindable is SearchTitleView view)
         {
            view.SearchCommand = n as ICommand;
            if (view.searchBar != null && n is ICommand value)
               view.searchBar.SearchCommand = value;
         }
      }

      /// <summary>
      /// Handle the change of the <see cref="ClearCommand"/> property
      /// </summary>
      /// <param name="bindable">The bindable object</param>
      /// <param name="o">The old value</param>
      /// <param name="n">The new value</param>
      private static void OnClearCommandChanged(BindableObject bindable, object o, object n)
      {
         if (bindable is SearchTitleView view)
            view.ClearCommand = n as ICommand;
      }

      /// <summary>
      /// Handle the change of the <see cref="TitleStyle"/> property
      /// </summary>
      /// <param name="bindable">The bindable object</param>
      /// <param name="o">The old value</param>
      /// <param name="n">The new value</param>
      private static void OnTitleStylePropertyChanged(BindableObject bindable, object o, object n)
      {
         if (bindable is SearchTitleView view && view?.title != null && n is Style value)
            view.title.Style = value;
      }

      /// <summary>
      /// Handle the change of the <see cref="SearchImageStyle"/> property
      /// </summary>
      /// <param name="bindable">The bindable object</param>
      /// <param name="o">The old value</param>
      /// <param name="n">The new value</param>
      private static void OnSearchImageStyleChanged(BindableObject bindable, object o, object n)
      {
         if (bindable is SearchTitleView view && view.searchIcon != null && n is Style value)
            view.searchIcon.Style = value;
      }

      /// <summary>
      /// Handle the change of the <see cref="SearchBarStyle"/> property
      /// </summary>
      /// <param name="bindable">The bindable object</param>
      /// <param name="o">The old value</param>
      /// <param name="n">The new value</param>
      private static void OnSearchBarStyleChanged(BindableObject bindable, object o, object n)
      {
         if (bindable is SearchTitleView view && view.searchBar != null && n is Style value)
            view.searchBar.Style = value;
      }

      /// <summary>
      /// Handle the change of the <see cref="IsSearchIconVisible"/> property
      /// </summary>
      /// <param name="bindable">The bindable object</param>
      /// <param name="o">The old value</param>
      /// <param name="n">The new value</param>
      private static void IsSearchIconVisibleChanged(BindableObject bindable, object o, object n)
      {
         if (bindable is SearchTitleView view && view.searchBar != null && n != null && n is bool value)
         {
            if (view.searchBar.IsVisible && !string.IsNullOrEmpty(view.searchBar.Text))
               return; // We are currently searching
            view.BatchBegin();
            try
            {
               view.searchBar.IsVisible = false;
               view.searchBar.Text = string.Empty;
               view.title.IsVisible = true;
               view.searchIcon.IsVisible = value;
            }
            finally
            {
               view.BatchCommit();
            }
         }
      }

      /// <summary>
      /// Show the search bar
      /// </summary>
      private void OnSearchIconTapped()
      {
         BatchBegin();
         try
         {
            title.IsVisible = false;
            searchIcon.IsVisible = false;
            searchBar.IsVisible = true;
            searchBar.Focus();
         }
         finally
         {
            BatchCommit();
         }
      }

      /// <summary>
      /// Cancel the search and hide the search bar
      /// </summary>
      private void OnCancelled()
      {
         BatchBegin();
         try
         {
            searchBar.IsVisible = false;
            searchBar.Text = string.Empty;
            title.IsVisible = true;
            searchIcon.IsVisible = true;
         }
         finally
         {
            BatchCommit();
         }

         if (ClearCommand != null && ClearCommand.CanExecute(null))
            ClearCommand.Execute(null);
      }

      /// <summary>
      /// Handle a change in the search bar text
      /// </summary>
      /// <param name="sender">Sender object</param>
      /// <param name="e">Event arguments</param>
      private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
      {
         if (!string.Equals(SearchText, e.NewTextValue, StringComparison.Ordinal))
         {
            SearchText = e.NewTextValue;
            OnPropertyChanged(nameof(SearchText));
         }
         searchBar.SearchCommandParameter = e.NewTextValue;
      }

      #endregion
   }
}