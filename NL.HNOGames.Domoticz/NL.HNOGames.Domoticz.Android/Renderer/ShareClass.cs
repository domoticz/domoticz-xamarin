using Android.App;
using Android.Content;
using NL.HNOGames.Domoticz.Controls;
using NL.HNOGames.Domoticz.Droid.Renderer;
using System.Linq;
using Xamarin.Forms;


[assembly: Dependency(typeof(ShareClass))]
namespace NL.HNOGames.Domoticz.Droid.Renderer
{
    /// <summary>
    /// Defines the <see cref="ShareClass" />
    /// </summary>
    public class ShareClass : Activity, IShare
    {
        #region Public

        /// <summary>
        /// The Share
        /// </summary>
        /// <param name="subject">The subject<see cref="string"/></param>
        /// <param name="message">The message<see cref="string"/></param>
        /// <param name="image">The image<see cref="byte[]"/></param>
        public void Share(string subject, string message, byte[] image)
        {
            if (image == null)
                return;

            var intent = new Intent(Intent.ActionSend);
            if (!string.IsNullOrEmpty(subject)) intent.PutExtra(Intent.ExtraSubject, subject);
            if (!string.IsNullOrEmpty(message)) intent.PutExtra(Intent.ExtraText, message);
            intent.SetType("image/png");

            var path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads
                + Java.IO.File.Separator + "camera.png");

            System.IO.File.WriteAllBytes(path.Path, image.Concat(new byte[] { (byte)0xD9 }).ToArray());
            intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.FromFile(path));
            Forms.Context.StartActivity(Intent.CreateChooser(intent, "Share Image"));
        }

        #endregion
    }
}
