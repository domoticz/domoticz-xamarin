using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Plugin.Fingerprint.Dialog;

namespace NL.HNOGames.Domoticz.Droid.Helpers
{
   /// <summary>
   /// Custom finger print dialog
   /// </summary>
   public class CustomFingerprintDialogFragment : FingerprintDialogFragment
   {
      /// <summary>
      /// On Create view
      /// </summary>
      public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
      {
         base.DefaultColor = Color.ParseColor("#0A203D");
         var view = base.OnCreateView(inflater, container, savedInstanceState);
         if (view != null)
         {
            var image = view.FindViewById<ImageView>(Resource.Id.fingerprint_imgFingerprint);
            if (image != null)
               image.LayoutParameters = new LinearLayout.LayoutParams(100, 100) { Gravity = GravityFlags.CenterHorizontal, BottomMargin = 20 };
         }
         return view;
      }
   }
}