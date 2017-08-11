using NL.HNOGames.Domoticz.Helpers;

namespace NL.HNOGames.Domoticz.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        private bool _isBusy;
        private bool _loadCache = true;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public bool LoadCache
        {
            get => _loadCache;
            set => SetProperty(ref _loadCache, value);
        }

        /// <summary>
        /// Private backing field to hold the title
        /// </summary>
        private string _title = string.Empty;
        /// <summary>
        /// Public property to set and get the title of the item
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}

