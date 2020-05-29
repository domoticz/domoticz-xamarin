using System;
using UIKit;

namespace NL.HNOGames.OOTT.iOS
{
    /// <summary>
    /// Application class
    /// </summary>
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            try
            {
                // if you want to use a different Application Delegate class from "AppDelegate"
                // you can specify it here.
                UIApplication.Main(args, null, "AppDelegate");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
