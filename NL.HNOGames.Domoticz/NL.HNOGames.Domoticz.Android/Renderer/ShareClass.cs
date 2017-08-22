using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using NL.HNOGames.Domoticz.Controls;
using NL.HNOGames.Domoticz.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.Linq;
using System;

[assembly: Dependency(typeof(ShareClass))]
namespace NL.HNOGames.Domoticz.Droid.Renderer
{
	public class ShareClass : Activity, IShare
	{
        public void Share(string subject, string message, byte[] bytes)
        {
            var intent = new Intent(Intent.ActionSend);
            if (!string.IsNullOrEmpty(subject)) intent.PutExtra(Intent.ExtraSubject, subject);
            if (!string.IsNullOrEmpty(message)) intent.PutExtra(Intent.ExtraText, message);
            intent.SetType("image/png");

            var path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads
                + Java.IO.File.Separator + "camera.png");
            System.IO.File.WriteAllBytes(path.Path, bytes.Concat(new byte[] { (byte)0xD9 }).ToArray());
            intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.FromFile(path));
            Forms.Context.StartActivity(Intent.CreateChooser(intent, "Share Image"));
        }
    }
}
