using NL.HNOGames.Domoticz.Helpers;
using NL.HNOGames.Domoticz.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        bool isBusy = false;
        bool loadCache = true;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public bool LoadCache
        {
            get { return loadCache; }
            set { SetProperty(ref loadCache, value); }
        }

        /// <summary>
        /// Private backing field to hold the title
        /// </summary>
        string title = string.Empty;
        /// <summary>
        /// Public property to set and get the title of the item
        /// </summary>
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
    }
}

