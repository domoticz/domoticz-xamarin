using System;
using System.ComponentModel;
using System.Linq;
using UIKit;
using NL.HNOGames.Domoticz.Controls;
using NL.HNOGames.Domoticz.iOS.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TitleViewSearchBar), typeof(TitleViewSearchBarRenderer))]
namespace NL.HNOGames.Domoticz.iOS.Renderer
{
   public class TitleViewSearchBarRenderer : SearchBarRenderer
   {
      private string CancelButtonText => (Element is TitleViewSearchBar searchBar) ? searchBar.CancelButtonText : null;
      private bool _Subscribed = false;
      protected override void Dispose(bool disposing)
      {
         if (disposing)
         {
            Control.CancelButtonClicked -= OnCancelClicked;
            _Subscribed = false;
         }
         base.Dispose(disposing);
      }

      protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
      {
         base.OnElementChanged(e);

         if (e.NewElement != null)
         {
            if (Control != null && !_Subscribed)
            {
               Control.CancelButtonClicked += OnCancelClicked;
               _Subscribed = true;
            }
            SetHideIcon();
            UpdateCancelButton();
         }
      }

      protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         base.OnElementPropertyChanged(sender, e);

         if (e.PropertyName == SearchBar.TextProperty.PropertyName || e.PropertyName == VisualElement.IsVisibleProperty.PropertyName)
            UpdateCancelButton();
      }

      private void SetHideIcon()
      {
         if (Control == null || IntPtr.Zero == Control.Handle)
            return;

         var textField = Control.GetSubViews<UITextField>().FirstOrDefault();
         if (textField != null)
            textField.LeftViewMode = UITextFieldViewMode.Never;
      }

      private void UpdateCancelButton()
      {
         if (Control == null || IntPtr.Zero == Control.Handle)
            return;

         if (Element != null && Element.IsVisible)
            Control.ShowsCancelButton = true;

         var button = Control.GetSubViews<UIButton>().FirstOrDefault();
         if (button != null)
         {
            string text = CancelButtonText;
            if (!string.IsNullOrEmpty(text))
            {
               button.SetTitle(text, UIControlState.Normal);
               button.SetTitle(text, UIControlState.Highlighted);
               button.SetTitle(text, UIControlState.Disabled);
            }
            UIColor color = button.TitleColor(UIControlState.Normal);
            button.SetTitleColor(color, UIControlState.Disabled);
         }
      }

      private void OnCancelClicked(object sender, EventArgs e)
      {
         if (Element is TitleViewSearchBar searchBar)
            searchBar.SendCancelled();
      }
   }
}