using NL.HNOGames.Domoticz.Helpers;

namespace NL.HNOGames.Domoticz.ViewModels
{
    /// <summary>
    /// Defines the <see cref="BaseViewModel" />
    /// </summary>
    public class BaseViewModel : ObservableObject
    {
        #region Variables

        /// <summary>
        /// Defines the _isBusy
        /// </summary>
        private bool _isBusy;

        /// <summary>
        /// Defines the _loadCache
        /// </summary>
        private bool _loadCache = true;

        /// <summary>
        /// Private backing field to hold the title
        /// </summary>
        private string _title = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether IsBusy
        /// </summary>
        public bool IsBusy { get => _isBusy; set => SetProperty(ref _isBusy, value); }

        /// <summary>
        /// Gets or sets a value indicating whether LoadCache
        /// </summary>
        public bool LoadCache { get => _loadCache; set => SetProperty(ref _loadCache, value); }

        /// <summary>
        /// Gets or sets the Title
        /// Public property to set and get the title of the item
        /// </summary>
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        #endregion
    }
}
