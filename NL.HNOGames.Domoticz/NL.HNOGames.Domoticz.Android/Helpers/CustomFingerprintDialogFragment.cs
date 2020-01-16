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
        #region Public

        /// <summary>
        /// On Create view
        /// </summary>
        /// <param name="inflater">The inflater<see cref="LayoutInflater"/></param>
        /// <param name="container">The container<see cref="ViewGroup"/></param>
        /// <param name="savedInstanceState">The savedInstanceState<see cref="Bundle"/></param>
        /// <returns>The <see cref="View"/></returns>
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

        #endregion
    }
}
