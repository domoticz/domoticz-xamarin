using Android.Content;
using Android.Widget;
using NL.HNOGames.Domoticz.Controls;
using NL.HNOGames.Domoticz.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TitleViewSearchBar), typeof(TitleViewSearchBarRenderer))]
namespace NL.HNOGames.Domoticz.Droid.Renderer
{
   public class TitleViewSearchBarRenderer : SearchBarRenderer
   {
      public TitleViewSearchBarRenderer(Context context)
        : base(context)
      { }

      protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
      {
         base.OnElementChanged(e);

         if (Control != null)
         {
            // Hide the search plate line by setting the background color to transparent
            var plateId = Control.Resources.GetIdentifier("android:id/search_plate", null, null);
            var searchPlate = Control.FindViewById(plateId);
            if (searchPlate != null)
               searchPlate.SetBackgroundColor(Android.Graphics.Color.Transparent);

            // Replace the search icon by the arrow left to close the search bar
            var iconId = Control.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
            var searchIcon = Control.FindViewById(iconId) as ImageView;
            if (searchIcon != null)
            {
               searchIcon.SetImageResource(Resource.Drawable.baseline_arrow_back_white_24);
               if (e.NewElement != null && !Color.Default.Equals(e.NewElement.CancelButtonColor))
                  searchIcon.SetColorFilter(e.NewElement.CancelButtonColor.ToAndroid());
               searchIcon.Click += OnSearchIconClick;
            }

            // Disable the submit button and set the color equal to the text color of the search plate
            Control.SubmitButtonEnabled = false;
            var submitId = Control.Resources.GetIdentifier("android:id/search_go_btn", null, null);
            var submitButton = Control.FindViewById<ImageView>(submitId);
            if (submitButton != null && e.NewElement != null && !Color.Default.Equals(e.NewElement.CancelButtonColor))
               submitButton.SetColorFilter(e.NewElement.CancelButtonColor.ToAndroid());
         }
      }

      private void OnSearchIconClick(object sender, System.EventArgs e)
      {
         if (Element is TitleViewSearchBar searchBar)
            searchBar.SendCancelled();
      }
   }
}